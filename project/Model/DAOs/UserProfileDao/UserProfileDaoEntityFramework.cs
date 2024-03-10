using Es.Udc.DotNet.ModelUtil.Dao;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;

namespace Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao
{
    /// <summary>
    /// Specific Operations for Client
    /// </summary>
    public class UserProfileDaoEntityFramework :
        GenericDaoEntityFramework<UserProfile, Int64>, IUserProfileDao
    {
        #region Public Constructors

        /// <summary>
        /// Public Constructor
        /// </summary>
        public UserProfileDaoEntityFramework()
        {
        }

        #endregion Public Constructors

        #region IClientDao Members. Specific Operations

 
        public UserProfile FindByLoginName(String loginName)
        {
            UserProfile user = null;

            DbSet<UserProfile> clients = Context.Set<UserProfile>();

            var result =
                (from u in clients
                 where u.loginName == loginName
                 select u);

            user = result.FirstOrDefault();

            if (user == null)
                throw new InstanceNotFoundException(loginName,
                    typeof(UserProfile).FullName);

            return user;
        }

        #endregion IClientDao Members. Specific Operations
    }
}