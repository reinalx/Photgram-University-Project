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
    public partial class ViewFollows : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Block<FollowsUserDetails> Follows;
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

                //Get Follows

                Follows = feedUserService.FindUserFollows(_usrId, startIndex, size);

                dtlFollows.DataSource = Follows.Items;
                dtlFollows.DataBind();

                ViewState["lastIndex"] = startIndex;
                ViewState["startIndex"] = startIndex + Follows.Items.Count;
                ViewState["existMoreItems"] = Follows.ExistMoreItems;
                ViewState["numElem"] = Follows.Items.Count;
                ViewState["size"] = size;

                lblNotMoreFollows.Visible = false;
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

                Block<FollowsUserDetails> Follows;

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

                Follows = feedUserService.FindUserFollows(usrId, startIndex, size);

                dtlFollows.DataSource = Follows.Items;
                dtlFollows.DataBind();

                ViewState["lastIndex"] = startIndex;
                ViewState["startIndex"] = startIndex + Follows.Items.Count;
                ViewState["existMoreItems"] = Follows.ExistMoreItems;
                ViewState["numElem"] = Follows.Items.Count;

            }
            else
            {
                lblNotMoreFollows.Visible = true;
            }
        }

        protected void btPrevious_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            long usrId = -1;
            int lastIndex = (int)ViewState["lastIndex"];
            int size = (int)ViewState["size"];

            Block<FollowsUserDetails> Follows;

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
                Follows = feedUserService.FindUserFollows(usrId, lastIndex, size);
                dtlFollows.DataSource = Follows.Items;
                dtlFollows.DataBind();

                ViewState["lastIndex"] = lastIndex;
                ViewState["startIndex"] = lastIndex + Follows.Items.Count;
                ViewState["numElem"] = Follows.Items.Count;
                ViewState["existMoreItems"] = Follows.ExistMoreItems;
                lblNotMoreFollows.Visible = false;
            }

            

        }

    }
}
