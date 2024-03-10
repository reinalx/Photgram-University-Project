<%@ Page Title="" Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="ViewFollowers.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Feed.ViewFollowers" meta:resourcekey="PageResource1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="lblViewFollowers" runat="server" Text="FOLLOWERS" meta:resourcekey="lblViewFollowersResource1"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <form id="form1" runat="server">
    <div align="center">
        <asp:Panel ID="pnViewFollowers" runat="server" BackColor="#CCCCCC" BorderColor="Black" BorderStyle="Solid" Width="349px" meta:resourcekey="pnViewFollowersResource1">
            <asp:DataList ID="dtlFollowers" runat="server" meta:resourceKey="dtlFollowersResource1" >
                <ItemTemplate>
                    <div>
                        <asp:Label ID="lblLoginNameUser" runat="server" Text='<%# Eval("LoginNameUser") %>' ></asp:Label> - 
                        <asp:HyperLink ID="lnkViewProfile" runat="server" NavigateUrl='<%# "~/Pages/Feed/ViewProfile.aspx?usrId=" + Eval("UsrId") %>' meta:resourceKey="lnkViewProfileResource1">Ver perfil</asp:HyperLink>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </div>
        <div class="container-fluid"  >
            <span class="spPagination">
                <ul class="pagination">
                    <li class="page-item">
                        <asp:Button ID="btPrevious" runat="server" CssClass="page-link" Text="<%$ Resources:Common, btPrevious %>" OnClick="btPrevious_Click" meta:resourcekey="btPreviousResource1" />
                    </li>
                    <li class="page-item">
                        <asp:Button ID="btNext" runat="server" CssClass="page-link" Text="<%$ Resources:Common, btNext %>" OnClick="btNext_Click" meta:resourcekey="btNextResource1" />
                    </li>
                </ul>
                <br/>
                <asp:Label ID="lblNotMoreFollowers" runat="server" Text="Not more Follows" meta:resourcekey="lblNotMoreFollowersResource1" Visible="False" ForeColor="#CC0000" ></asp:Label>

            </span>
        </div>
    </form>

</asp:Content>
