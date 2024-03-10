using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Post
{
    public partial class PostDetails : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();

            long postId;

            try
            {
                postId = long.Parse(Request.Params.Get("postId"));
            }
            catch (ArgumentNullException)
            {
                postId = -1L;
            }


            Model.Services.PostService.PostDetails postDetails = postService.GetPostDetails(postId);


            if (SessionManager.IsUserAuthenticated(Context))
            {
                long userId;
                userId = SessionManager.GetUserSession(Context).UserProfileId;
                if (postDetails.userId.Equals(userId))
                {
                    ImageButtonDelete.Visible = true;
                }

            }


            imageCard.ImageUrl = postDetails.image;
            titleCard.Text = postDetails.title;
            descrCard.Text = postDetails.description;
            categCard.Text = postDetails.categoryName;
            dateCard.Text = postDetails.date.ToString();

            ImageButtonDelete.OnClientClick = $"return confirm('{GetLocalResourceObject("confirmDelete.Text")}');";


            if (postDetails.diaphragmOpen == null || postDetails.diaphragmOpen == -1)
            {
                doCard.Text = GetLocalResourceObject("noData").ToString();
            }
            else
            {
                doCard.Text = postDetails.diaphragmOpen.ToString();
            }
            if (postDetails.whiteBal == null || postDetails.whiteBal == -1)
            {
                wbCard.Text = GetLocalResourceObject("noData").ToString();
            }
            else
            {
                wbCard.Text = postDetails.whiteBal.ToString();
            }
            if (postDetails.timeExp == null || postDetails.timeExp == -1)
            {
                teCard.Text = GetLocalResourceObject("noData").ToString();
            }
            else
            {
                teCard.Text = postDetails.timeExp.ToString();
            }
            if (postDetails.iso == null || postDetails.iso == -1)
            {
                isoCard.Text = GetLocalResourceObject("noData").ToString();
            }
            else
            {
                isoCard.Text = postDetails.iso.ToString();
            }

            String url = "~/Pages/Post/Comments.aspx?postId=" + postId;

            lnkComment.NavigateUrl = Response.ApplyAppPathModifier(url);
            
        }


        protected void deletePost(object sender, CommandEventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();

            if (SessionManager.IsUserAuthenticated(Context))
            {
                long postId = long.Parse(Request.Params.Get("postId"));
                long userId = SessionManager.GetUserSession(Context).UserProfileId;

                postService.DeletePost(userId, postId);
                Response.Redirect(Response.ApplyAppPathModifier ("~/Pages/MainPage.aspx"));
            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx"));
            }
        }


     



    }
}