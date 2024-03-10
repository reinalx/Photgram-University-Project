
<%@ Page Language="C#" AutoEventWireup="true"  CodeBehind="Authentication.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.User.Authentication"  meta:resourcekey="Page" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    
    <title>PracticaMaD</title>

    <link rel="stylesheet" href="/Content/bootstrap.min.css"/>
    
    <style type="text/css">
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
    <link href="/Css/signin.css" rel="stylesheet"/>
    
    <script type="text/javascript" src="/Scripts/jquery-3.7.1.min.js"></script>


</head>
        <body class="text-center" style="background-color: #f5f5f5">
    
        <main class="form-signin" >
            <form id="AuthenticationForm" method="POST" runat="server">
                <img class="mb-4" src="/icons/camara.png" alt="" width="72" height="72" />
                <h1 class="h3 mb-3 fw-normal">Please sign in</h1>

                <div class="form-floating">
                    <span class="entryLogin">
                        <asp:TextBox ID="txtLogin" CssClass="form-control" runat="server" placeholder="<%$ Resources: lclLogin.Text %>"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLogin" runat="server"
                                                    ControlToValidate="txtLogin" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:Label ID="lblLoginError" runat="server" ForeColor="Red"  Style="position: relative"
                                   Visible="False" meta:resourcekey="lblLoginError">                        
                        </asp:Label>
                    </span>
                </div>
                <div class="form-floating">

                    <span class="entryPassword">
                        <asp:TextBox TextMode="Password" CssClass="form-control" ID="txtPassword"  runat="server" placeholder="<%$ Resources: lclPassword.Text %>" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                                                    ControlToValidate="txtPassword" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"/>
                        <asp:Label ID="lblPasswordError" runat="server" ForeColor="Red" Style="position: relative"
                                   Visible="False" meta:resourcekey="lblPasswordError">       
                        </asp:Label>
                    </span>
                </div>
                <div class="checkbox mb-3">
                    <asp:CheckBox ID="checkRememberPassword" runat="server" TextAlign="Left" meta:resourcekey="checkRememberPassword" />
                </div>
                <div class="button">
                    <asp:Button ID="btnLogin" runat="server" CssClass="w-100 btn btn-lg btn-primary" OnClick="BtnLoginClick" meta:resourcekey="btnLogin" />
                </div>
                <div class="button" style="margin-top: 10px">
                    <asp:HyperLink ID="lnkLoginGoogle" runat="server" CssClass="w-100 btn btn-lg btn-danger" Text="Sign in with Google" NavigateUrl="~/Pages/User/SigninGoogle.aspx" ></asp:HyperLink>
                </div>
                <p><asp:HyperLink ID="lnkRegister" runat="server" NavigateUrl="~/Pages/User/Register.aspx" meta:resourcekey="lnkRegister" /></p>
                <p class="mt-5 mb-3 text-muted">&copy; 2023–2024</p>

            </form>
        </main>


    
    </body>

</html>





