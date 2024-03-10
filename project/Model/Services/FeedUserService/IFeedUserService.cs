using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Ninject;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService
{
    public interface IFeedUserService
    {

        [Inject]
        IUserProfileDao UserProfileDao { get; set; }

        [Inject]
        IPostDao PostDao { get;  set; }

        [Inject]
        IFollowDao FollowDao { get; set; }

        //---------------- FUN 3.1 ----------------

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        FeedUserDetails FindFeedUserDetails(long usrId, int startIndex, int size);

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Block<Post> FindPostUser(long usrId, int startIndex, int size);

        //---------------- FUN 3.2 ----------------

        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Block<FollowsUserDetails> FindUserFollows(long usrId, int startIndex, int size);


        /// <exception cref="InstanceNotFoundException"/>
        [Transactional]
        Block<FollowsUserDetails> FindUserFollowers(long usrId, int startIndex, int size);

        //---------------- FUN 4.2 ----------------

        [Transactional]
        Block<Post> FindDefaultFeedUser(long usrId, int startIndex, int size);

    }
}
