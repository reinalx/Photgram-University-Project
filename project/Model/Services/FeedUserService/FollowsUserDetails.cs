using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService
{

    [Serializable()]
    public class FollowsUserDetails
    {
        public long UsrId { get; private set; }
        public String LoginNameUser { get; private set; }

        public FollowsUserDetails(long usrId, String loginNameUser)
        {
            this.UsrId = usrId;
            this.LoginNameUser = loginNameUser;
        }

    }
}
