using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Feed
{
    public partial class ViewFollowers : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Block<FollowsUserDetails> Followers;
                long _usrId = -1;
                int startIndex = 0, size;
                try
                {
                    _usrId = long.Parse(Request.Params.Get("usrId"));
                }
                catch (ArgumentNullException)
                {
                    Response.Clear();
                    Response.StatusCode = 404;
                    Server.Transfer("~/Pages/Errors/InternalError.aspx");
                }

                /* Get size*/
                try
                {
                    size = Int32.Parse(Request.Params.Get("size"));
                }
                catch (ArgumentNullException)
                {
                    size = Settings.Default.PracticaMaD_defaultSize;
                }

                //Cogemos el servicio

                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

                //Get Followers

                Followers = feedUserService.FindUserFollowers(_usrId, startIndex, size);

                dtlFollowers.DataSource = Followers.Items;
                dtlFollowers.DataBind();

                ViewState["lastIndex"] = startIndex;
                ViewState["startIndex"] = startIndex + Followers.Items.Count;
                ViewState["existMoreItems"] = Followers.ExistMoreItems;
                ViewState["numElem"] = Followers.Items.Count;
                ViewState["size"] = size;

                lblNotMoreFollowers.Visible = false;
            }
        }

        protected void btNext_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["existMoreItems"])
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

                long usrId = -1;
                int startIndex = (int)ViewState["startIndex"];
                int size = (int)ViewState["size"];
                int numElem = (int)ViewState["numElem"];

                Block<FollowsUserDetails> Followers;

                try
                {
                    usrId = long.Parse(Request.Params.Get("usrId"));
                }
                catch (ArgumentNullException)
                {
                    Response.Clear();
                    Response.StatusCode = 404;
                    Server.Transfer("~/Pages/Errors/InternalError.aspx");
                }

                Followers = feedUserService.FindUserFollowers(usrId, startIndex, size);

                dtlFollowers.DataSource = Followers.Items;
                dtlFollowers.DataBind();

                ViewState["lastIndex"] = startIndex;
                ViewState["startIndex"] = startIndex + Followers.Items.Count;
                ViewState["existMoreItems"] = Followers.ExistMoreItems;
                ViewState["numElem"] = Followers.Items.Count;

            }
            else
            {
                lblNotMoreFollowers.Visible = true;
            }
        }

        protected void btPrevious_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            long usrId = -1;
            int lastIndex = (int)ViewState["lastIndex"];
            int size = (int)ViewState["size"];

            Block<FollowsUserDetails> Followers;

            try
            {
                usrId = long.Parse(Request.Params.Get("usrId"));
            }
            catch (ArgumentNullException)
            {
                Response.Clear();
                Response.StatusCode = 404;
                Server.Transfer("~/Pages/Errors/InternalError.aspx");
            }

            lastIndex -= size;
            if (lastIndex >= 0)
            {
                Followers = feedUserService.FindUserFollowers(usrId, lastIndex, size);
                dtlFollowers.DataSource = Followers.Items;
                dtlFollowers.DataBind();

                ViewState["lastIndex"] = lastIndex;
                ViewState["startIndex"] = lastIndex + Followers.Items.Count;
                ViewState["numElem"] = Followers.Items.Count;
                ViewState["existMoreItems"] = Followers.ExistMoreItems;
                lblNotMoreFollowers.Visible = false;
            }



        }

    }
}


