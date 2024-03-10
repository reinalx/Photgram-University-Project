<%@ Page Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="PostsByKeywordAndCat.aspx.cs" meta:resourcekey="Page" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Post.PostsByKeywordAndCat" EnableEventValidation="false" %>
<%@ Import Namespace="Resources" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="lblPostsFound" runat="server" meta:resourcekey="lblPostsFound"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <div align="center">
        <form runat="server">

            <asp:Label ID="lblNoPosts" Text="<%$ Resources:, lblNoPosts %>" runat="server"></asp:Label>
            <div class="card-deck">
                <asp:ListView ID="posts" runat="server">
                <ItemTemplate>
                     <div class="card" style="width: 23rem; display: inline-block; margin: 5px auto; max-width: 340px; width: 100%;  ">
                        <asp:Image ID="imagenControl" CssClass="card-img-top img-thumbnail" runat="server" ImageUrl='<%# Eval("image") %>' AlternateText="Not Found" ImageAlign="Top" meta:resourcekey="imagenControlResource1" />
                        <div class="card-body" style="padding: 15px">
                            <h5 class="card-title"><%# Eval("title") %></h5>
                            <asp:HyperLink ID="lnkAuth" CssClass="card-link" NavigateUrl='<%# "~/Pages/Feed/ViewProfile.aspx?usrId=" + Eval("userId") %>' Text='<%# Eval("loginName") %>' runat="server"/>
                            <asp:HyperLink ID="lnkPost" CssClass="card-link" runat="server" NavigateUrl='<%# "~/Pages/Post/PostDetails.aspx?postId=" + Eval("PostId") %>' Text="<%$Resources:, detailsLbl %>"></asp:HyperLink>
                            <asp:HyperLink ID="lnkComm" CssClass="card-link" runat="server" NavigateUrl='<%# "~/Pages/Post/Comments.aspx?postId=" + Eval("PostId") %>' Text="<%$Resources:, seeCommentsLbl %>"></asp:HyperLink>
                        </div>   
                         <div style="display:flex;" class="card-footer flex-row align-items-center justify-content-center">
                                <h6 class="post-likes" style="font-size:25px"><%# Eval("likes") %></h6>
                                <asp:ImageButton
                                    Height="25px"
                                    ID="ImageButton1"
                                    ImageUrl='<%#isPostLiked(Eval("postId").ToString()) ? "~/icons/heart-fill.svg" : "~/icons/heart.svg" %>'
                                    CommandName="postId"
                                    CommandArgument='<%#  Eval("PostId") %>'
                                    OnCommand="likePost"
                                    runat="server"  />
                         </div>
                    </div>
                </ItemTemplate>
                </asp:ListView>
            </div>
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