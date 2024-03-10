using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Exceptions;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Ninject;
using System;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.UserService
{
    public interface IUserService
    {
        [Inject]
        IUserProfileDao UserProfileDao { set; }

        [Inject]
        IFollowDao FollowDao { set; }


        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userProfileId"> The user profile id. </param>
        /// <param name="oldClearPassword"> The old clear password. </param>
        /// <param name="newClearPassword"> The new clear password. </param>
        /// <exception cref="IncorrectPasswordException"/>
        /// <exception cref="InstanceNotFoundException"/>
        void ChangePassword(long userProfileId, String oldClearPassword,
            String newClearPassword);

        /// <summary>
        /// Finds the user profile details.
        /// </summary>
        /// <param name="userProfileId"> The user profile id. </param>
        /// <returns> The user profile details </returns>
        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        UserProfileDetails FindUserProfileDetails(long userProfileId);

        /// <summary>
        /// Logins the specified login name.
        /// </summary>
        /// <param name="loginName"> Name of the login. </param>
        /// <param name="password"> The password. </param>
        /// <param name="passwordIsEncrypted"> if set to <c> true </c> [password is encrypted]. </param>
        /// <returns> LoginResult </returns>
        /// <exception cref="InstanceNotFoundException"/>
        /// <exception cref="IncorrectPasswordException"/>
        [Transactional]
        LoginResult Login(String loginName, String password,
            Boolean passwordIsEncrypted);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="loginName"> Name of the login. </param>
        /// <param name="clearPassword"> The clear password. </param>
        /// <param name="userProfileDetails"> The user profile details. </param>
        /// <exception cref="DuplicateInstanceException"/>
        [Transactional]
        long RegisterUser(String loginName, String clearPassword,
            UserProfileDetails userProfileDetails);

        /// <summary>
        /// Updates the user profile details.
        /// </summary>
        /// <param name="userProfileId"> The user profile id. </param>
        /// <param name="userProfileDetails"> The user profile details. </param>
        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        void UpdateUserProfileDetails(long userProfileId,
            UserProfileDetails userProfileDetails);

        /// <summary>
        /// Checks if the specified loginName corresponds to a valid user.
        /// </summary>
        /// <param name="loginName"> User loginName. </param>
        /// <returns> Boolean to indicate if the loginName exists </returns>
        bool UserExists(string loginName);

        [Transactional]
        string GetUserName(long usrId);

        //---------------- FUN 4.1 ----------------

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        long FollowUser(long usrId1, string loginNameUser2);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        long UnFollowUser(long usrId1, string loginNameUser2);

        [Transactional]
        bool IsFollowing(long usrId1, long usrId2);

        //----------------- AUTH2.0 ---------------

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        UserProfile GetProfile(string loginName);

    }
}