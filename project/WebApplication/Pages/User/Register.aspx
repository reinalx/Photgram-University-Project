<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.User.Register" 
meta:resourcekey="Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head3" runat="server">
    
    <title>PracticaMaD</title>

    <link rel="stylesheet" href="/Content/bootstrap.min.css"/>
    
    <style>
      .bd-placeholder-img {
        font-size: 1.125rem;
        text-anchor: middle;
        -webkit-user-select: none;
        -moz-user-select: none;
        user-select: none;
      }

      @media (min-width: 768px) {
        .bd-placeholder-img-lg {
          font-size: 3.5rem;
        }
      }
    </style>
    <link href="/Css/register.css" rel="stylesheet"/>
    
    <script src="/Scripts/bootstrap.bundle.min.js"></script>

</head>
        <body class="text-center">
    
        <main class="form-signin" >
            <form id="RegisterForm" method="post" runat="server">
                <img class="mb-4" src="/icons/camara.png" alt="" width="72" height="57">
                <h1 class="h3 mb-3 fw-normal">Please register in</h1>

                <div class="form-floating">
                    <span class="entry">
                        <asp:TextBox ID="txtLogin" runat="server" CssClass="form-control"
                                     meta:resourcekey="txtLoginResource1" OnTextChanged="txtLogin_TextChanged"
                                     placeholder="<%$ Resources: lclUserName.Text %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtLogin"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvUserNameResource1"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblLoginError" runat="server" ForeColor="Red" Style="position: relative"
                                   Visible="False" meta:resourcekey="lblLoginError"></asp:Label>
                    </span>
                </div>
                <div class="form-floating">
                    <span class="entryPassword">
                        <asp:TextBox TextMode="Password" ID="txtPassword" runat="server"
                                     CssClass="form-control" placeholder="<%$ Resources: lclPassword.Text %>" meta:resourcekey="txtPasswordResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvPasswordResource1"></asp:RequiredFieldValidator>

                    </span>
                </div>
                <div class="form-floating">
                    <span class="entryPassword">
                        <asp:TextBox TextMode="Password" ID="txtRetypePassword" runat="server" CssClass="form-control"
                                     placeholder="<%$ Resources: lclRetypePassword.Text %>" meta:resourcekey="txtRetypePasswordResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRetypePassword" runat="server" ControlToValidate="txtRetypePassword"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvRetypePasswordResource1"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvPasswordCheck" runat="server" ControlToCompare="txtPassword"
                                              ControlToValidate="txtRetypePassword" meta:resourcekey="cvPasswordCheck"></asp:CompareValidator></span>
                </div>
                
                <div class="form-floating">
                    <span class="entry">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"
                                     placeholder="<%$ Resources: lclFirstName.Text %>" meta:resourcekey="txtFirstNameResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvFirstNameResource1"></asp:RequiredFieldValidator></span>
                </div>
                
                <div class="form-floating">
                    <span class="entry">
                        <asp:TextBox ID="txtSurname" runat="server" CssClass="form-control"
                                     placeholder="<%$ Resources: lclSurname.Text %>" meta:resourcekey="txtSurnameResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvSurnameResource1"></asp:RequiredFieldValidator></span>
                </div>
                
                <div class="form-floating">
                    <span
                        class="entry">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                     placeholder="<%$ Resources: lclEmail.Text %>" meta:resourcekey="txtEmailResource1"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                                    Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                                    meta:resourcekey="rfvEmailResource1"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                                        Display="Dynamic" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                        meta:resourcekey="revEmail"></asp:RegularExpressionValidator></span>
                </div>
                <div class="form-floating">
                    
                    <span class="entry">
                        <asp:DropDownList ID="comboLanguage" runat="server" AutoPostBack="True" CssClass="form-select"
                                           meta:resourcekey="comboLanguageResource1"
                                          OnSelectedIndexChanged="ComboLanguageSelectedIndexChanged">
                        </asp:DropDownList></span>
                </div>
                <div class="form-floating">
                    <span class="entry">
                        <asp:DropDownList ID="comboCountry" runat="server" CssClass="form-select"
                                          meta:resourcekey="comboCountryResource1">
                        </asp:DropDownList></span>
                </div>
               
                <div class="button">
                    <asp:Button ID="btnRegister" runat="server" CssClass="w-100 btn btn-lg btn-primary" OnClick="BtnRegisterClick" meta:resourcekey="btnRegister" />
                </div>
                <p class="mt-5 mb-3 text-muted">&copy; 2023–2024</p>
            </form>
        </main>


    
    </body>

</html>

