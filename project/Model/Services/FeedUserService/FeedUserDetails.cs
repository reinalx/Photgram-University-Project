using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService
{
    [Serializable()]
    public class FeedUserDetails
    {
        #region Properties

        public long UsrId { get; private set; }
        public String LoginName { get; private set; }

        public int NumFollows { get; private set; }
        
        public int NumFollowers { get; private set; }

        public Block<Post> PostUser { get; private set; }


        #endregion

        public FeedUserDetails( long userId, String loginName, int numFollows, int numFollowers, Block<Post> postsUser)
        {
            this.UsrId = userId;
            this.LoginName = loginName;
            this.NumFollowers = numFollowers;
            this.NumFollows = numFollows;
            this.PostUser = postsUser;
        }
        public FeedUserDetails(String loginName, int numFollows, int numFollowers, Block<Post> postsUser)
        {
            this.LoginName = loginName;
            this.NumFollowers = numFollowers;
            this.NumFollows = numFollows;
            this.PostUser = postsUser;
        }

    }
}
