using Es.Udc.DotNet.ModelUtil.IoC;
using Es.Udc.DotNet.PracticaMaD.Model;
using Es.Udc.DotNet.PracticaMaD.Model.Services.PostService;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Model.Services.TagService;

namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.Post
{
    public partial class UploadPost : SpecificCulturePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IPostService postService = ioCManager.Resolve<IPostService>();
                ITagService tagService = ioCManager.Resolve<ITagService>();

                List<Category> categories = postService.FindAllCategories();

                ddlCategory.Items.Clear();

                ddlCategory.DataSource = categories;
                ddlCategory.DataTextField = "categoryName";
                ddlCategory.DataValueField = "categoryName";
                ddlCategory.DataBind();

                ddlCategory.Items.Insert(0, new ListItem("Selecciona una categoría", "-1"));

                List<Tag> tags = tagService.GetAllTags();
                gvTagList.DataSource = tags;
                gvTagList.DataBind();
            }

        }


        protected void Crear_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                IPostService postService = ioCManager.Resolve<IPostService>();
                ITagService tagService = ioCManager.Resolve<ITagService>();


                UserSession userSession = SessionManager.GetUserSession(Context);

                string title = titleInput.Text;
                string descripcion = DescriptionInput.Text;
                string selectedCategoryName = ddlCategory.SelectedValue;
                long categoryId = -1, postId;
                try
                {
                    categoryId = postService.GetCategoryByName(selectedCategoryName).categoryId;
                }
                catch (InstanceNotFoundException)
                {
                    Response.Redirect("~/Pages/Errors/InternalError.aspx");
                }

                string filePath = null;

                if (FileUploadImage.HasFile)
                {
                    string fileName = Path.GetFileName(FileUploadImage.FileName);
                    filePath = ("~/imgSources/" + fileName);
                    FileUploadImage.SaveAs(Server.MapPath(filePath));
                }

                List<Tag> tags = new List<Tag>();
                var rows = gvTagList.Rows;
                int count = gvTagList.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    bool isChecked = ((CheckBox)rows[i].FindControl("addTag")).Checked;
                    if (isChecked)
                    {
                        Tag tag = tagService.FindTagByName(rows[i].Cells[0].Text);
                        if (tag != null)
                        {
                            tags.Add(tag);
                        }
                    }
                }

                double apertura = -1;
                double TExpo = -1;
                double ISO = -1;
                double SectionBalBlncos = -1;

                double.TryParse(AperturaInput.Text, out apertura);
                double.TryParse(TExpoInput.Text, out TExpo);
                double.TryParse(ISOIput.Text, out ISO);
                double.TryParse(SectionBalBlncosInput.Text, out SectionBalBlncos);
               

                postId = postService.CreatePost(title, filePath, descripcion, categoryId, userSession.UserProfileId, apertura, TExpo, ISO, SectionBalBlncos);

                postService.TagPost(postId, tags);
                Response.Redirect(Response.ApplyAppPathModifier("~/Pages/Feed/DefaultFeed.aspx"));


            }
        }
        protected void BtnAddTag(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                IIoCManager ioCManager = (IIoCManager)HttpContext.Current.Application["managerIoC"];
                ITagService tagService = ioCManager.Resolve<ITagService>();
                try
                {
                    Tag tag;
                    tag = tagService.CreateTag(txtTag.Text);
                    txtTag.Text = String.Empty;
                    List<Tag> tags = tagService.GetAllTags();
                    gvTagList.DataSource = tags;
                    gvTagList.DataBind();
                }
                catch (DuplicateInstanceException)
                {
                    lblTagError.Visible = true;
                }
            }
        }
    }
}