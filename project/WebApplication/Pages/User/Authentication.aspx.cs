using Es.Udc.DotNet.PracticaMaD.Model.Services.UserService.Exceptions;
using Es.Udc.DotNet.PracticaMaD.Web.HTTP.Session;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using System;
using System.Web.Security;


namespace Es.Udc.DotNet.PracticaMaD.Web.Pages.User
{
    public partial class Authentication : SpecificCulturePage
    {
        protected  void Page_Load(object sender, EventArgs e)
        {
            lblPasswordError.Visible = false;
            lblLoginError.Visible = false;

            
        }

        /// <summary>
        /// Handles the Click event of the btnLogin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance
        /// containing the event data.</param>
        protected void BtnLoginClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SessionManager.Login(Context, txtLogin.Text,
                        txtPassword.Text, checkRememberPassword.Checked, false);

                    FormsAuthentication.
                        RedirectFromLoginPage(txtLogin.Text,
                            checkRememberPassword.Checked);
                }
                catch (InstanceNotFoundException)
                {
                    lblLoginError.Visible = true;
                }
                catch (IncorrectPasswordException)
                {
                    lblPasswordError.Visible = true;
                }
            }
        }

       


    }
}