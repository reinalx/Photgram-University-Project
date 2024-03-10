using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.FollowDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.PostDao;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.UserProfileDao;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.ModelUtil.Exceptions;

using Ninject;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService
{
    public class FeedUserService : IFeedUserService
    {
        [Inject]
        public IUserProfileDao UserProfileDao { get;  set; }
        [Inject]
        public IPostDao PostDao { get;  set; }

        [Inject]
        public IFollowDao FollowDao { get; set; }


        #region IFeedUserService Members

        //-------------FUN 3.1: VISUALIZAR USUARIO-------------
        
        [Transactional]
        public FeedUserDetails FindFeedUserDetails(long usrId, int startIndex, int size)
        {

            UserProfile user = UserProfileDao.Find(usrId);

            FeedUserDetails feedUserDetails = new FeedUserDetails(user.usrId,user.loginName, user.numFollows, user.numFollowers , FindPostUser(usrId, startIndex, size));

            return feedUserDetails;
        }

        [Transactional]
        public Block<Post> FindPostUser(long usrId, int startIndex, int size)
        {
            List<Post> posts = PostDao.FindPostByUserProfileUsrId(usrId, startIndex, size + 1);
            bool existMorePosts = (posts.Count == size + 1);

            if (existMorePosts)
                posts.RemoveAt(size);

            return new Block<Post>(posts, existMorePosts);
        }


        //-------------FUN 3.2: VISUALIZAR SEGUIDOS Y SEGUIDORES DE USUARIO-------------

        [Transactional]
        public Block<FollowsUserDetails> FindUserFollows(long usrId1, int startIndex, int size)
        {

            List<Follow> follows = FollowDao.FindByUserId1ID(usrId1, startIndex, size +1);
            bool existMoreFollows = (follows.Count == size + 1);

            if (existMoreFollows)
                follows.RemoveAt(size);

            List<FollowsUserDetails> followsUserDetails = new List<FollowsUserDetails>();
            UserProfile userFollow = new UserProfile();

            foreach (Follow follow in follows)
            {
                userFollow = UserProfileDao.Find(follow.usrId2);
                followsUserDetails.Add(new FollowsUserDetails(userFollow.usrId, userFollow.loginName));
            }

            return new Block<FollowsUserDetails>(followsUserDetails, existMoreFollows);

        }

        [Transactional]
        public Block<FollowsUserDetails> FindUserFollowers(long usrId2, int startIndex, int size)
        {

            List<Follow> followers = FollowDao.FindByUserId2ID(usrId2, startIndex, size + 1);
            bool existMoreFollowers = (followers.Count == size + 1);

            if (existMoreFollowers)
                followers.RemoveAt(size);

            List<FollowsUserDetails> followsUserDetails = new List<FollowsUserDetails>();
            UserProfile userFollower = new UserProfile();
           
            foreach(Follow follower in followers)
            {
                userFollower = UserProfileDao.Find(follower.usrId1);
                followsUserDetails.Add(new FollowsUserDetails(userFollower.usrId, userFollower.loginName));
            }

            return new Block<FollowsUserDetails>(followsUserDetails, existMoreFollowers);
        }

        //-------------FUN 4.2: VISUALIZAR PANTALLA POR DEFECTO USUARIO------------- //OPCIONAL

        [Transactional]
        public Block<Post> FindDefaultFeedUser(long usrId, int startIndex, int size)
        {
            List<Post> posts = PostDao.FindPostFollowsByUserId(usrId, startIndex, size + 1);
            bool existMorePosts = (posts.Count == size + 1);

            if (existMorePosts)
                posts.RemoveAt(size);
            
            
            return new Block<Post>(posts, existMorePosts);
        }


        #endregion
    }
}
