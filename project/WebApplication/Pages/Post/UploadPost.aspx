<%@ Page Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="UploadPost.aspx.cs" meta:resourcekey="Page" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Post.UploadPost" %>
<%@ Import Namespace="Resources" %>




<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="NewPostLb" runat="server" Text="Label" meta:resourcekey="NewPostTx"></asp:Label><br />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <div id="form">
        <form id="form1" method="post" runat="server">

            <br />
            <span class="entry">
                <asp:Label ID="titlelb" runat="server" Text="Label" meta:resourcekey="title"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="RequiredFieldValidator"
                    ControlToValidate="titleInput" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                    meta:resourcekey="rfvtitle" CssClass="error-message"></asp:RequiredFieldValidator><br />
                <asp:TextBox ID="titleInput" runat="server"></asp:TextBox><br /><br />
            </span>

            <span class="entry">
                <asp:Label ID="DescripcionLb" runat="server" Text="Label" meta:resourcekey="Descripcion"></asp:Label>
                <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ErrorMessage="RequiredFieldValidator"
                    ControlToValidate="DescriptionInput" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                    meta:resourcekey="rfvDescription" CssClass="error-message"></asp:RequiredFieldValidator><br />
                <asp:TextBox ID="DescriptionInput" runat="server"></asp:TextBox><br /><br />
            </span>
            
            <span class="entry">
                <asp:Label ID="CategoriaLb" runat="server" Text="Label" meta:resourcekey="Categoria"></asp:Label><br />
                <asp:DropDownList ID="ddlCategory" runat="server" meta:resourcekey="DropDownCat" AppendDataBoundItems="True"></asp:DropDownList><br /><br />
            </span>

            <span class="entry">
                <asp:Label ID="ImagenLb" runat="server" Text="Label" meta:resourcekey="Imagen"></asp:Label><br />
                <asp:RequiredFieldValidator ID="rfvImage" runat="server" ErrorMessage="RequiredFieldValidator"
                                            ControlToValidate="FileUploadImage" Display="Dynamic" Text="<%$ Resources:Common, mandatoryField %>"
                                            meta:resourcekey="rfvImage" CssClass="error-message"></asp:RequiredFieldValidator><br />
                <asp:FileUpload ID="FileUploadImage" runat="server" /><br /><br />
            </span>
            
            <span class="entry">
                <p runat="server" id="SectionApertura" class="toggleable">
                    <asp:Label ID="SectionAperturaLb" runat="server" Text="Label" meta:resourcekey="SectionAperturaTx"></asp:Label><br />
                    <asp:TextBox ID="AperturaInput" runat="server"></asp:TextBox>
                </p>
            </span>
            
            <span class="entry">
                <p runat="server" id="SectionTExpo" class="toggleable">
                    <asp:Label ID="SectionTExpoLb" runat="server" Text="Label" meta:resourcekey="SectionTExpoLbTx"></asp:Label><br />
                    <asp:TextBox ID="TExpoInput" runat="server"></asp:TextBox>
                </p>
            </span>
            
            <span class="entry">
                <p runat="server" id="SectionIso" class="toggleable">
                    <asp:Label ID="IsoLb" runat="server" Text="Label" meta:resourcekey="Iso"></asp:Label><br />
                    <asp:TextBox ID="ISOIput" runat="server"></asp:TextBox>
                </p>
            </span>
            
            <span class="entry">
                <p runat="server" id="SectionBalBlncos" class="toggleable">
                    <asp:Label ID="SectionBalBlncosLb" runat="server" Text="Label" meta:resourcekey="SectionBalBlncosTx"></asp:Label><br />
                    <asp:TextBox ID="SectionBalBlncosInput" runat="server"></asp:TextBox>
                </p>
            </span>
            
    
            <div>
                <div id="listTags" align="center">
                    <asp:GridView ID="gvTagList" runat="server" GridLines="None" 
                                  AutoGenerateColumns="False"
                                  ShowHeaderWhenEmpty="False">
                        <Columns>
                            <asp:BoundField DataField="TagName" HeaderText="<%$ Resources: TagName %>" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="addTag" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div >
                    <asp:Label ID="lblTag" runat="server" Text="Introduzca una nueva etiqueta" Font-Bold="True" meta:resourcekey="lblTag"></asp:Label>
                </div>
                <span class="entry">
                    <asp:RegularExpressionValidator Display="Dynamic" ControlToValidate="txtTag" ID="RegularExpressionValidator3" 
                                                    ValidationExpression="^[a-zA-Z]{3,}$" runat="server"
                                                    ValidationGroup="vdgAddTag"
                                                    ErrorMessage="<%$ resources: invalidTag.Text %>"
                                                    ></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="rfvTag" runat="server" ControlToValidate="txtTag" 
                                                ValidationGroup="vdgAddTag" Display="Dynamic" 
                                                Text="<%$ Resources:Common, mandatoryField %>" meta:resourcekey="rfvTag"
                    ></asp:RequiredFieldValidator>
                    <asp:TextBox ID="txtTag" runat="server" Width="300px" Columns="18" />
                   

                    <asp:Label ID="lblTagError" runat="server" ForeColor="Red" Style="position: relative"
                         Visible="False" meta:resourcekey="lblTagError"> </asp:Label>
                </span>
    
                <div class="button">
                    <asp:Button ID="btnAddTag" runat="server" OnClick="BtnAddTag" ValidationGroup="vdgAddTag" meta:resourcekey="btnAddTag" />
                </div>
            </div>
            <div class="button">
                <asp:Button ID="Crear" runat="server" Text="Subir" meta:resourcekey="UploadPost" OnClick="Crear_Click" />
            </div>

        </form>
    </div>
</asp:Content>
