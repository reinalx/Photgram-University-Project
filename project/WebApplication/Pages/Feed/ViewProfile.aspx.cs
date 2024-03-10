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
    public partial class ViewProfile : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack )
            {
                int startIndex = 0, size;
                long usrId =-1;


                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();
                IUserService userService = ioCManager.Resolve<IUserService>();

                UserSession userSession = SessionManager.GetUserSession(Context);

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

                /* Get size */
                try
                {
                    size = Int32.Parse(Request.Params.Get("size"));
                }
                catch (ArgumentNullException)
                {
                    size = Settings.Default.PracticaMaD_defaultSize;
                }
                //Creamos la ruta de navegación para los seguidores y seguidos
                lnkViewFollowers.NavigateUrl = Response.ApplyAppPathModifier( "~/Pages/Feed/ViewFollowers.aspx" + "?usrId=" +usrId);
                lnkViewFollows.NavigateUrl = Response.ApplyAppPathModifier("~/Pages/Feed/ViewFollows.aspx" + "?usrId=" + usrId);
                

                //Obtenemos los detalles del usuario y modificamos las etiquetas
                FeedUserDetails feedUserDetails = feedUserService.FindFeedUserDetails(usrId, startIndex, size);

                lblUserName.Text = feedUserDetails.LoginName;
                lblNumFollows.Text += feedUserDetails.NumFollows.ToString();
                lblNumFollowers.Text += feedUserDetails.NumFollowers.ToString();
                lsvPostUser.DataSource = feedUserDetails.PostUser.Items;

                lsvPostUser.DataBind();


                startIndex += feedUserDetails.PostUser.Items.Count;
                
                //Comprobomas si el perfil a visualizar es el de uno mimso 
                if (SessionManager.IsUserAuthenticated(Context))
                {
                    if (userSession.UserProfileId == usrId)
                    {
                        btFollow.Visible = false;
                    }
                    else
                    {
                        //Miramos si el usuario ya lo seguimos 
                        if (userService.IsFollowing(userSession.UserProfileId, usrId))
                        {
                            btFollow.CssClass = "btn btn-secondary";
                            btFollow.Text = "UnFolllow";
                        }
                    }
                }
                

                if (feedUserDetails.PostUser.Items.Count== 0)
                {
                    lblFeedPosts.Visible = true;
                    lsvPostUser.Visible = false;
                }
                else
                {
                    lblFeedPosts.Visible = false;
                    lsvPostUser.Visible = true;
                }

                ViewState.Add("startIndex", startIndex);
                ViewState.Add("existMoreItems", feedUserDetails.PostUser.ExistMoreItems);
                ViewState.Add("numElem", feedUserDetails.PostUser.Items.Count);
                ViewState.Add("size", size);
                ViewState.Add("usrId", usrId);
            }
        }
        protected void btFirstPosts_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            long usrId = (long)ViewState["usrId"];
            int size = (int)ViewState["size"];
            

            Block<Model.Post> postUser = feedUserService.FindPostUser(usrId, 0, size);

            lsvPostUser.DataSource = postUser.Items;
            lsvPostUser.DataBind();

            ViewState["startIndex"] = postUser.Items.Count;
            ViewState["existMoreItems"] = postUser.ExistMoreItems;
            ViewState["numElem"] = postUser.Items.Count;
            lblNotMorePost.Visible = false;

        }

        //BOTON PARA VER MAS POSTS
        protected void btMorePosts_Click(object sender, EventArgs e)
        {
            if ((bool)ViewState["existMoreItems"])
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

                int startIndex = (int)ViewState["startIndex"];
                int size = (int)ViewState["size"];
                long usrId = (long)ViewState["usrId"];


                Block<Model.Post> postsUser = feedUserService.FindPostUser(usrId, startIndex, size);

                lsvPostUser.DataSource = postsUser.Items;
                lsvPostUser.DataBind();

                ViewState["startIndex"] = startIndex + postsUser.Items.Count;
                ViewState["existMoreItems"] = postsUser.ExistMoreItems;
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
            int size = (int)ViewState["size"];
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

        protected void btFollow_Click(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IUserService userService = ioCManager.Resolve<IUserService>();
            IFeedUserService feedUserService = ioCManager.Resolve<IFeedUserService>();

            if (!SessionManager.IsUserAuthenticated(Context))
            {
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/User/Authentication.aspx"));
            }
            if (btFollow.CssClass == "btn btn-primary")
            {
                UserSession userSession = SessionManager.GetUserSession(Context);

                userService.FollowUser(userSession.UserProfileId, lblUserName.Text);

                btFollow.CssClass = "btn btn-secondary";
                btFollow.Text = "Unfollow";
            }
            else
            {
                UserSession userSession = SessionManager.GetUserSession(Context);

                userService.UnFollowUser(userSession.UserProfileId, lblUserName.Text);

                btFollow.CssClass = "btn btn-primary";
                btFollow.Text = "Follow";


            }
        }
    }
}

