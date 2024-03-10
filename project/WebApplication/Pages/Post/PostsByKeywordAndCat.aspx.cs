using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Model.Services.Utils; 
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Web.Properties;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Post
{
    public partial class PostsByKeywordAndCat : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int startIndex, count;
            string cat;
            string keyword;
            long tagId;
            Block<PostPreview> postBlock;

            lnkPrevious.Visible = false;
            lnkNext.Visible = false;
            lblNoPosts.Visible = false;

            /* Get the Service */
            IIoCManager iocManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = iocManager.Resolve<IPostService>();

            try
            {
                tagId = long.Parse(Request.Params.Get("tagId"));
            }
            catch (ArgumentNullException)
            {
                tagId = -1;
            }
            try
            {
                keyword = Request.Params.Get("keyword");
            }
            catch (ArgumentNullException)
            {
                keyword = "";
            }

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

            /* Get Category */
            try
            {
                cat = Request.Params.Get("cat");
            }
            catch (ArgumentNullException)
            {
                cat = "";
            }

            if (tagId != -1)
            {
                postBlock = postService.FindPostsByTags(tagId, startIndex, count);

            }
            else if(cat == "" || cat == null)
            {
                postBlock = postService.FindPosts(keyword, startIndex, count);
            }
            else
            {
                postBlock = postService.FindPosts(keyword, cat, startIndex, count);
            }




            if (postBlock.Items.Count == 0)
            {
                lblNoPosts.Visible = true;
                return;
            }

            this.posts.DataSource = postBlock.Items;
            this.posts.DataBind();

            /* "Previous" link */
            if ((startIndex - count) >= 0)
            {
                String url;
                if (tagId != -1)
                {
                    url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?tagId=" + tagId +
                          "&startIndex=" + (startIndex - count) + "&count=" + count;
                }
                else if (keyword == "")
                {
                    if (cat == "")
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" +
                        "?startIndex=" + (startIndex - count) + "&count=" + count;
                    }
                    else
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?cat=" + cat +
                        "&startIndex=" + (startIndex - count) + "&count=" + count;
                    }
                }
                else
                {
                    if (cat == "")
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?keyword=" + keyword +
                        "&startIndex=" + (startIndex - count) + "&count=" + count;
                    }
                    else
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?keyword=" + keyword + "&cat=" + cat +
                        "&startIndex=" + (startIndex - count) + "&count=" + count;
                    }
                }


                this.lnkPrevious.NavigateUrl =
                    Response.ApplyAppPathModifier(url);
                this.lnkPrevious.Visible = true;
            }

            /* "Next" link */
            if (postBlock.ExistMoreItems)
            {
                String url;
                if (tagId != -1)
                {
                    url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?tagId=" + tagId +
                          "&startIndex=" + (startIndex + count) + "&count=" + count;
                }
                else if (keyword == "")
                {
                    if (cat == "")
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" +
                        "?startIndex=" + (startIndex + count) + "&count=" + count;
                    }
                    else
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?cat=" + cat +
                        "&startIndex=" + (startIndex + count) + "&count=" + count;
                    }
                }
                else
                {
                    if (cat == "")
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?keyword=" + keyword +
                        "&startIndex=" + (startIndex + count) + "&count=" + count;
                    }
                    else
                    {
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx" + "?keyword=" + keyword + "&cat=" + cat +
                        "&startIndex=" + (startIndex + count) + "&count=" + count;
                    }
                }

                this.lnkNext.NavigateUrl =
                    Response.ApplyAppPathModifier(url);
                this.lnkNext.Visible = true;
            }
        }

        protected void likePost(object sender, CommandEventArgs e)
        {

            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();

            if (SessionManager.IsUserAuthenticated(Context))
            {
                long postId = long.Parse(e.CommandArgument.ToString());

                long user = SessionManager.GetUserSession(Context).UserProfileId;

                postService.likePost(user, postId);

                Response.Redirect(Request.RawUrl);
            } else
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx"));
            }
            

        }

        protected bool isPostLiked(string post)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();
            

            try
            {
                if (SessionManager.IsUserAuthenticated(Context))
                {
                    long userId = SessionManager.GetUserSession(Context).UserProfileId;
                    long postId = long.Parse(post);
                    bool result = postService.isPostLiked(userId, postId);
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

    }
}