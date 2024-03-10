<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/PracticaMaD.Master" CodeBehind="PostSearch.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Post.PostSearch" meta:resourcekey="PageResource2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_Header" runat="server">
    <asp:Label ID="lblSearchPost" runat="server" Text="BUSQUEDA USUARIOS" meta:resourcekey="lblSearchPost"></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">

    <form id="form1" runat="server">
        <section id="searchFeed">
            <center>
                <div class="searchFields">
                    <asp:Label ID="lblSearch" runat="server" Text="<%$Resources: Common, searchLabel %>"></asp:Label><br />
                    <asp:TextBox ID="keywordsTextBox" runat="server" meta:resourcekey="TextBox1Resource1"></asp:TextBox><br />
                    <asp:DropDownList ID="catDropDown" runat="server" meta:resourcekey="catDropDownResource1" AppendDataBoundItems="true"></asp:DropDownList><br />
                    <asp:Button ID="searchButton" runat="server" Text="<%$Resources: Common, searchButton %>" OnClick="searchButton_Click" />
                </div>
            </center>
        </section>
    </form>
</asp:Content>