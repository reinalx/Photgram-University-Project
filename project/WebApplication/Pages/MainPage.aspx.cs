using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using System;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages
{
    public partial class MainPage : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect("~/Pages/Feed/DefaultFeed.aspx");
            }
        }
    }
}