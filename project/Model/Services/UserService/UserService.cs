using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Util;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Ninject;
using System;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.UserService
{
    public class UserService : IUserService
    {
        [Inject]
        public IUserProfileDao UserProfileDao { private get; set; }

        [Inject]
        public IFollowDao FollowDao { private get; set; }



        #region IUserService Members

        /// <exception cref="IncorrectPasswordException"/>
        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        public void ChangePassword(long userProfileId, string oldClearPassword,
            string newClearPassword)
        {
            UserProfile userProfile = UserProfileDao.Find(userProfileId);
            String storedPassword = userProfile.enPassword;

            if (!PasswordEncrypter.IsClearPasswordCorrect(oldClearPassword,
                 storedPassword))
            {
                throw new IncorrectPasswordException(userProfile.loginName);
            }

            userProfile.enPassword =
            PasswordEncrypter.Crypt(newClearPassword);

            UserProfileDao.Update(userProfile);
        }

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        public UserProfileDetails FindUserProfileDetails(long userProfileId)
        {
            UserProfile userProfile = UserProfileDao.Find(userProfileId);

            UserProfileDetails userProfileDetails =
                new UserProfileDetails(userProfile.firstName,
                    userProfile.lastName, userProfile.email,
                    userProfile.language, userProfile.country);

            return userProfileDetails;
        }

        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="IncorrectPasswordException"/>
        [Transactional]
        public LoginResult Login(string loginName, string password, bool passwordIsEncrypted)
        {
            UserProfile userProfile =
                UserProfileDao.FindByLoginName(loginName);

            String storedPassword = userProfile.enPassword;

            if (passwordIsEncrypted)
            {
                if (!password.Equals(storedPassword))
                {
                    throw new IncorrectPasswordException(loginName);
                }
            }
            else
            {
                if (!PasswordEncrypter.IsClearPasswordCorrect(password,
                        storedPassword))
                {
                    throw new IncorrectPasswordException(loginName);
                }
            }

            return new LoginResult(userProfile.usrId, userProfile.firstName,
                storedPassword, userProfile.language, userProfile.country);
        }

        /// <exception cref="DuplicateInstanceException"/>
        [Transactional]
        public long RegisterUser(string loginName, string clearPassword,
            UserProfileDetails userProfileDetails)
        {
            try
            {
                UserProfileDao.FindByLoginName(loginName);

                throw new DuplicateInstanceException(loginName,
                    typeof(UserProfile).FullName);
            }
            catch (InstanceNotFoundException)
            {
                String encryptedPassword = PasswordEncrypter.Crypt(clearPassword);

                UserProfile userProfile = new UserProfile();

                userProfile.loginName = loginName;
                userProfile.enPassword = encryptedPassword;
                userProfile.firstName = userProfileDetails.FirstName;
                userProfile.lastName = userProfileDetails.Lastname;
                userProfile.email = userProfileDetails.Email;
                userProfile.language = userProfileDetails.Language;
                userProfile.country = userProfileDetails.Country;

                UserProfileDao.Create(userProfile);

                return userProfile.usrId;
            }
        }

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        public void UpdateUserProfileDetails(long userProfileId,
            UserProfileDetails userProfileDetails)
        {
            UserProfile userProfile =
                UserProfileDao.Find(userProfileId);

            userProfile.firstName = userProfileDetails.FirstName;
            userProfile.lastName = userProfileDetails.Lastname;
            userProfile.email = userProfileDetails.Email;
            userProfile.language = userProfileDetails.Language;
            userProfile.country = userProfileDetails.Country;
            UserProfileDao.Update(userProfile);
        }

        public bool UserExists(string loginName)
        {
            
            try
            {
                UserProfile userProfile = UserProfileDao.FindByLoginName(loginName);
            }
            catch (InstanceNotFoundException)
            {
                return false;
            }

            return true;
        }

        [Transactional]
        public string GetUserName(long usrId)
        {
            try
            {
                return UserProfileDao.Find(usrId).loginName;
            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }


        //---------------- FUN 4.1: Seguimiento Usuario ----------------

        [Transactional]
        public long FollowUser(long usrId1, string loginNameUser2)
        {
          
            //SUPONGO QUE EL USUARIO EXISTE Y SI NO ESTA AUTENTIFICADO NO LE SALDRA ESTA OPCION

            if (!UserExists(loginNameUser2))
              throw new InstanceNotFoundException(loginNameUser2, "Cannot follow, the user isn't exist");

            UserProfile usr1 = UserProfileDao.Find(usrId1);
            UserProfile usr2 = UserProfileDao.FindByLoginName(loginNameUser2);

            if (FollowDao.ExistsByUserId1AndUserId2(usrId1, usr2.usrId) != -1)
                throw new InstanceNotFoundException(usr2.usrId, "Cannot follow the user, is already follow");

            Follow follow = new Follow();

            follow.usrId1 = usrId1;
            follow.usrId2 = usr2.usrId;

            usr1.numFollows++;
            usr2.numFollowers++;
            UserProfileDao.Update(usr1);
            UserProfileDao.Update(usr2);

            FollowDao.Create(follow);

            return follow.followId;
        }

        [Transactional]
        public long UnFollowUser(long usrId1, string loginNameUser2)
        {

            //SUPONGO QUE EL USUARIO EXISTE Y SI NO ESTA AUTENTIFICADO NO LE SALDRA ESTA OPCION

            if (!UserExists(loginNameUser2))
                throw new InstanceNotFoundException(loginNameUser2, "Cannot follow, the user isn't exist");

            UserProfile usr1 = UserProfileDao.Find(usrId1);
            UserProfile usr2 = UserProfileDao.FindByLoginName(loginNameUser2);

            long followId = FollowDao.ExistsByUserId1AndUserId2(usrId1, usr2.usrId);
            if (followId == -1)
                throw new InstanceNotFoundException(usr2.usrId, "Cannot Unfollow the user, the user is not following");

            usr1.numFollows--;
            usr2.numFollowers--;

            UserProfileDao.Update(usr1);
            UserProfileDao.Update(usr2);

            FollowDao.Remove(followId);

            return followId;
        }

        [Transactional]
        public bool IsFollowing(long usrId1, long usrId2)
        {
            if (FollowDao.ExistsByUserId1AndUserId2(usrId1, usrId2) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion IUserService Members

        //------------- AUTH 2.0 -----------------
        [Transactional]
        public UserProfile GetProfile(string loginName)
        {
            try
            {
                return UserProfileDao.FindByLoginName(loginName);
            }
            catch (InstanceNotFoundException)
            {
                return null;
            }
        }
    }



}