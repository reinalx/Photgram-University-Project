using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.FeedUserService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;
using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Post
{
    public partial class Comments : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int startIndex, count;
            long postId;
            Block<Comment> commentBlock;

            lnkPrevious.Visible = false;
            lnkNext.Visible = false;
            lblNoComments.Visible = false;

            /* Get the Service */
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = iocManager.Resolve<IPostService>();

            /* Get Start Index */
            try
            {
                startIndex = Int32.Parse(Request.Params.Get("startIndex"));
            }
            catch (ArgumentNullException)
            {
                startIndex = 0;
            }

            /* Get Count */
            try
            {
                count = Int32.Parse(Request.Params.Get("count"));
            }
            catch (ArgumentNullException)
            {
                count = Settings.Default.PracticaMaD_defaultSize;
            }

            /* Get postId */
            try
            {
                postId = long.Parse(Request.Params.Get("postId"));
            }
            catch (ArgumentNullException)
            {
                postId = -1;
            }

            postDetails.NavigateUrl = "~/Pages/Post/PostDetails.aspx?postId=" + postId;

            commentBlock = postService.ShowComments(postId, startIndex, count);




            if (commentBlock.Items.Count == 0)
            {
                lblNoComments.Visible = true;
                return;
            }

            this.posts.DataSource = commentBlock.Items;
            this.posts.DataBind();


            /* "Previous" link */
            if ((startIndex - count) >= 0)
            {
                String url;

                url = "~/Pages/Post/Comments.aspx" + "?postId=" + postId +
                        "&startIndex=" + (startIndex - count) + "&count=" + count;

                this.lnkPrevious.NavigateUrl =
                    Response.ApplyAppPathModifier(url);
                this.lnkPrevious.Visible = true;
            }

            /* "Next" link */
            if (commentBlock.ExistMoreItems)
            {
                String url;

                url = "~/Pages/Post/Comments.aspx" + "?postId=" + postId +
                        "&startIndex=" + (startIndex + count) + "&count=" + count;

                this.lnkNext.NavigateUrl =
                    Response.ApplyAppPathModifier(url);
                this.lnkNext.Visible = true;

               
            }

        }

        protected string getUsername(object id)
        {
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = iocManager.Resolve<IFeedUserService>();
            IUserService userService = iocManager.Resolve<IUserService>();

            string username = userService.GetUserName(long.Parse(id.ToString()));

            return username;
        }

        protected void buttonTextAddCommentId_Click(object sender, EventArgs e)
        {
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = iocManager.Resolve<IPostService>();


            if (SessionManager.IsUserAuthenticated(Context))
            {
                long postId = long.Parse(Request.Params.Get("postId"));
                long userId = SessionManager.GetUserSession(Context).UserProfileId;
                string comentario = textAddCommentId.Text;

                postService.addComent(userId, postId, comentario);

                String url = "Comments.aspx" + "?postId=" + postId;

                Response.Redirect(Response.ApplyAppPathModifier(url));

            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx"));
            }

        }

        protected bool canDelete(string comm)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();


            try
            {
                if (SessionManager.IsUserAuthenticated(Context))
                {
                    long userId = SessionManager.GetUserSession(Context).UserProfileId;
                    long commId = long.Parse(comm);
                    bool result = postService.GetComment(commId).usrId == userId;
                    return result;

                }
                else
                {
                    return false;
                }
            }
            catch (InstanceNotFoundException)
            {
                return false;
            }
        }

        protected void buttonTextDeleteCommentId_Click(object sender, CommandEventArgs e)
        {
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = iocManager.Resolve<IPostService>();


            if (SessionManager.IsUserAuthenticated(Context))
            {
                long userId = SessionManager.GetUserSession(Context).UserProfileId;
                long postId = long.Parse(Request.Params.Get("postId"));
                long commentId2 = long.Parse(e.CommandArgument.ToString());

                Comment comment = postService.GetComment(commentId2);

                bool canDeleteComment = (userId == comment.usrId);
                

                if (canDeleteComment)
                {
                    postService.DeleteComment(userId, commentId2);
                

                }
                else
                {
                    string script = "alert('No puedes borrar este comentario.');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);

                }


                String url = "Comments.aspx" + "?postId=" + postId;

                Response.Redirect(Response.ApplyAppPathModifier(url));


            }
            else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx"));
            }

        }

    }
}