using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Feed
{
    public partial class DefaultFeed : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int startIndex = 0, size;

                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

                UserSession userSession = SessionManager.GetUserSession(Context);

                /* Get size */
                try
                {
                    size = Int32.Parse(Request.Params.Get("size"));
                }
                catch (ArgumentNullException)
                {
                    size = Settings.Default.PracticaMaD_defaultSize;
                }

                Block<Model.Post> feed =
                    feedUserService.FindDefaultFeedUser(userSession.UserProfileId, startIndex, size);

                lsvPostUser.DataSource = feed.Items;
                lsvPostUser.DataBind();
                
                if (feed.Items.Count == 0)
                {
                    lblFeedPosts.Visible = true;
                    lsvPostUser.Visible = false;
                }
                else
                {
                    lblFeedPosts.Visible = false;
                    lsvPostUser.Visible = true;
                }

                startIndex += feed.Items.Count;
                ViewState.Add("startIndex", startIndex );
                ViewState.Add("existMoreItems", feed.ExistMoreItems);
                ViewState.Add("usrId", userSession.UserProfileId);
                ViewState.Add("numElem", feed.Items.Count);
            }
        }

        protected void btFirstPosts_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            long usrId = (long)ViewState["usrId"];
            int size = Settings.Default.PracticaMaD_defaultSize;


            Block<Model.Post> postUser = feedUserService.FindDefaultFeedUser(usrId, 0, size);

            lsvPostUser.DataSource = postUser.Items;
            lsvPostUser.DataBind();

            ViewState["startIndex"] = postUser.Items.Count;
            ViewState["existMoreItems"] = postUser.ExistMoreItems;
            ViewState["numElem"] = postUser.Items.Count;
            lblNotMorePost.Visible = false;
        }

        protected void btMorePosts_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["existMoreItems"])
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

                int startIndex = (int)ViewState["startIndex"];
                int size = Settings.Default.PracticaMaD_defaultSize;
                long usrId = (long)ViewState["usrId"];


                Block<Model.Post> postsUser = feedUserService.FindDefaultFeedUser(usrId, startIndex, size);

                lsvPostUser.DataSource = postsUser.Items;
                lsvPostUser.DataBind();

                ViewState["startIndex"] = startIndex + postsUser.Items.Count;
                ViewState["existMoreItems"] = postsUser.ExistMoreItems;
                ViewState["numElem"] = postsUser.Items.Count;
            }
            else
            {
                lblNotMorePost.Visible = true;
            }
        }

        protected void btBack_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            int startIndex = (int)ViewState["startIndex"];
            int size = Settings.Default.PracticaMaD_defaultSize;
            int numElem = (int)ViewState["numElem"];

            long usrId = (long)ViewState["usrId"];

            if ((startIndex - numElem - size) < 0)
            {
                startIndex = 0;
            }
            else
            {
                startIndex -= size + numElem;
            }

            Block<Model.Post> postsUser = feedUserService.FindDefaultFeedUser(usrId, startIndex, size);

            lsvPostUser.DataSource = postsUser.Items;
            lsvPostUser.DataBind();

            ViewState["startIndex"] = startIndex + postsUser.Items.Count;
            ViewState["existMoreItems"] = postsUser.ExistMoreItems;
            ViewState["numElem"] = postsUser.Items.Count;

            if (lblNotMorePost.Visible == true)
                lblNotMorePost.Visible = false;
        }
    }
}