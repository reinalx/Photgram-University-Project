using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Post
{
    public partial class PostSearch : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
            IPostService postService = ioCManager.Resolve<IPostService>();

            var masterPage = this.Master as PracticaMaD;
            List<Category> categories = postService.FindAllCategories();


            catDropDown.Items.Add(new ListItem("-"));
            foreach (Category category in categories)
                catDropDown.Items.Add(new ListItem(category.categoryName));
            masterPage.SetActiveLnkSearch();

        }

        protected void searchButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                String keywords = this.keywordsTextBox.Text;
                String cat = this.catDropDown.SelectedValue;
                String url;

                if (keywords == "" || keywords == null)
                {
                    if (cat == "-")
                        url = "~/Pages/Post/PostsByKeywordAndCat.aspx";
                    else
                        url = String.Format("~/Pages/Post/PostsByKeywordAndCat.aspx?cat={0}", cat);
                }
                else
                {
                    if (cat == "-")
                        url = String.Format("~/Pages/Post/PostsByKeywordAndCat.aspx?keyword={0}", keywords);
                    else
                        url = String.Format("~/Pages/Post/PostsByKeywordAndCat.aspx?keyword={0}&cat={1}", keywords, cat);
                }

                Response.Redirect(Response.ApplyAppPathModifier(url));
            }
        }
    }
}