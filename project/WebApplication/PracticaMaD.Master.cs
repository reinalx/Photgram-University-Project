using System;
using System.Collections.Generic;
using System.Web;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.TagService;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;

namespace Es.Udc.DotNet.PracticaMaD.Web
{

    public partial class PracticaMaD : System.Web.UI.MasterPage
    {

        public static readonly String USER_SESSION_ATTRIBUTE = "userSession";

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!SessionManager.IsUserAuthenticated(Context))
            {
                if (lnkUpdate != null)
                    lnkUpdate.Visible = false;
                if (lnkMyProfile != null)
                    lnkMyProfile.Visible = false;
                if (lnkLogout != null)
                    lnkLogout.Visible = false;
                if (lblUser != null)
                    lblUser.Visible = false;
                if (usrDropdown != null)
                    usrDropdown.Visible = false;
                if (lnkPhotogram != null)
                    lnkPhotogram.HRef = Response.ApplyAppPathModifier("~/Pages/MainPage.aspx");
                ;


            }
            else
            {
                if (lnkAuthenticate != null)
                    lnkAuthenticate.Visible = false;
                if (lnkMyProfile != null && !IsPostBack)
                    lnkMyProfile.NavigateUrl += "?usrId=" + SessionManager.GetUserSession(Context).UserProfileId;
                if (lnkPhotogram != null)
                    lnkPhotogram.HRef = Response.ApplyAppPathModifier("~/Pages/Feed/DefaultFeed.aspx");
            }

            if (!IsPostBack)
            {
                IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];

                ITagService tagService = iocManager.Resolve<ITagService>();
                List<Tag> tags = tagService.GetAllTags();

                lsvTagCloud.DataSource = tags;
                lsvTagCloud.DataBind();

            }

        }
        public void SetActiveLnkSearch()
        {
            lnkSearch.CssClass = "nav-link active";
        }
        protected string GetFontSize(int numUsed)
        {
            if (numUsed == 0)
                return "0px";
            int baseFontSize = 12;
            int maxSize = 24;
            int minSize = 10;

            int fontSize = baseFontSize + (numUsed * 2);

            fontSize = Math.Max(minSize, Math.Min(maxSize, fontSize));

            return fontSize.ToString() + "px";
        }
        private List<string> availableColors = new List<string> { "#FF0000", "#00FF00", "#0000FF", "#FFFF00", "#FF00FF", "#00FFFF", "#0FFF00" };
        private int colorIndex = 0;

        protected string GetNextColor()
        {
            string color = availableColors[colorIndex];
            colorIndex = (colorIndex + 1) % availableColors.Count;
            return color;
        }


    }
}
