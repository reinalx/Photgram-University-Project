<%@ Page Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="Comments.aspx.cs" meta:resourcekey="Page" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Post.Comments" EnableEventValidation="false" %>
<%@ Import Namespace="Resources" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="lblPostsFound" runat="server" Text="POSTS FOUNDS" meta:resourcekey="lblPostsFound"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <div align="center">
        <form runat="server">

            <asp:Label ID="lblNoComments" Text="<%$ Resources:, lblNoComments %>" runat="server"></asp:Label>
            <div class="card-deck">
                <asp:ListView ID="posts" runat="server">
                    <ItemTemplate>
                        <div class="comment-container">
                            <asp:HyperLink ID="lnkAuth" CssClass="card-link" NavigateUrl='<%# "~/Pages/Feed/ViewProfile.aspx?usrId=" + Eval("usrId") %>' Text='<%# getUsername(Eval("usrId")) %>' runat="server"/>
                            <br />
                            <asp:Label runat="server" Text='<%# Eval("date") %>'></asp:Label>        
                            <br />
                            <asp:Label runat="server" Text='<%# Eval("text") %>'></asp:Label>
                            <br />
                            <asp:Button ID="buttonDeleteComment" runat="server" Text="<%$ Resources:, btnDeleteComment %>" OnCommand="buttonTextDeleteCommentId_Click" CommandArgument='<%# Eval("commentId") %>' Visible='<%#canDelete( Eval("commentId").ToString()) %>'/>

                            <br />
                            <hr />
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>

             <br/><br/>
            <div>      
                <asp:TextBox ID="textAddCommentId" runat="server"></asp:TextBox>

                <asp:Button ID="buttonTextAddCommentId" runat="server" Text="Añadir comment INTER" OnClick="buttonTextAddCommentId_Click" meta:resourcekey="btnAddComment"/>
            </div>

                <asp:HyperLink ID="postDetails" CssClass="card-link" Text="<%$Resources:, seeDetails %>" runat="server"/>


        </form>
    </div>
    <br />
    <!-- "Previous" and "Next" links. -->
    <div class="previousNextLinks" align="center">
        <span class="previousLink">
            <asp:HyperLink ID="lnkPrevious" Text=" < " runat="server" Visible="False"></asp:HyperLink>
            &nbsp&nbsp
        </span><span class="nextLink">
                <asp:HyperLink ID="lnkNext" Text=" > " runat="server" Visible="False"></asp:HyperLink>
            </span>
    </div>
    <br />
    <br />
</asp:Content>