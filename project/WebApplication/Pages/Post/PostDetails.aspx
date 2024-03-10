<%@ Page Title="" Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="PostDetails.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Post.PostDetails" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="lblPostDeatils" runat="server" Text="POST DETAILS" meta:resourcekey="lblPostDetails"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

    <asp:Panel runat="server" CssClass="card" ID="PostDetailsCard">
        <div class="card">
            <asp:Image ID="imageCard" ImageAlign="Top" CssClass="card-img-top" runat="server"/>
            <asp:Label ID="titleCard" CssClass="card-title" runat="server"></asp:Label>
                <asp:Label ID="descrCard" CssClass="card-text" runat="server"></asp:Label>
            <div class="card-body">
                <asp:Label ID="categCard" runat="server"></asp:Label>
                <asp:Label ID="dateCard" runat="server"></asp:Label>
            </div>
            <asp:Label ID="doCard" runat="server"></asp:Label>
            <asp:Label ID="teCard" runat="server"></asp:Label>
            <asp:Label ID="wbCard" runat="server"></asp:Label>
            <asp:Label ID="isoCard" runat="server"></asp:Label>
            <asp:HyperLink ID="lnkComment" CssClass="card-link" Text="<%$Resources:, seeComments %>" runat="server"/>
            
             <form runat="server">
                <div style="display:flex;" class="card-footer flex-row align-items-center justify-content-center">
                    <asp:ImageButton
                        Height="25px"
                        ID="ImageButtonDelete"
                        ImageUrl='~/icons/basura.png'
                        CommandName="postId"
                        CommandArgument='<%# Eval("PostId") %>'
                        OnCommand="deletePost"
                        OnClientClick="return confirm('¿Seguro este post?');"
                        runat="server" Visible="False" />
                </div>
            </form>

        
        </div>
    </asp:Panel>
</asp:Content>