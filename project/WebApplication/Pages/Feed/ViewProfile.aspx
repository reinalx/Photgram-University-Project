<%@ Page Title="" Language="C#" MasterPageFile="~/PracticaMaD.Master" AutoEventWireup="true" CodeBehind="ViewProfile.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMaD.Web.Pages.Feed.ViewProfile" meta:resourcekey="PageResource1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <asp:Label ID="lvlViewProfile" runat="server" Text="PROFILE" meta:resourcekey="lblViewProfile" ></asp:Label>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_MenuWelcome" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_MenuExplanation" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder_BodyContent" runat="server">
    <form id="form1" runat="server">
    <section>
        <div class="headAccount" align="left">
            <div class="profileInfo" >
                <asp:Label ID="lblUserName" runat="server" Text="Label" meta:resourcekey="lblUserNameResource1" Font-Size="XX-Large" Font-Bold="True"></asp:Label>
                <br/>
                <span id="profileLinks">
                    <asp:HyperLink ID="lnkViewFollows" runat="server" NavigateUrl="~/Pages/Feed/ViewFollows.aspx" meta:resourcekey="lnkViewFollowsResource1">Follows</asp:HyperLink>:
                    <asp:Label ID="lblNumFollows" runat="server" meta:resourcekey="lblNumFollowsResource1"></asp:Label> -    
                    <asp:HyperLink ID="lnkViewFollowers" runat="server" NavigateUrl="~/Pages/Feed/ViewFollowers.aspx" meta:resourcekey="lnkViewFollowersResource1">Followers</asp:HyperLink>
                    <asp:Label ID="lblNumFollowers" runat="server" meta:resourcekey="lblNumFollowersResource1"></asp:Label>
                </span> 
            </div>
            <div class="btFollow" >
                <asp:Button ID="btFollow" runat="server" CssClass="btn btn-primary"  Text="Follow"  OnClick="btFollow_Click" />
            </div>
        </div>
        
    </section>
        <br/>
        <div align="left">
            <asp:Label ID="lblPosts" runat="server" Text="Posts" meta:resourcekey="lblPosts" ></asp:Label>
        </div>
    <section class="posts" > 
        <div class="dashboardPost" align="center">
            <asp:ListView ID="lsvPostUser" runat="server"  >
                <ItemTemplate>
                    <span class="spCard">
                        <div class="card"  >
                            <asp:Image ID="imagenControl" CssClass="card-img-top" runat="server" ImageUrl='<%# Eval("img") %>' AlternateText="Not Found" ImageAlign="Top" meta:resourcekey="imagenControlResource1" />
                            <div class="card-body" align="left" >
                                <h5 class="card-title"><%# Eval("title") %></h5>
                                <p class="card-text"><%# Eval("description") %></p>
                                <asp:HyperLink ID="lnkPost" CssClass="btn btn-primary" runat="server" NavigateUrl='<%# "~/Pages/Post/PostDetails.aspx?postId=" + Eval("PostId") %>'  Text='<%$ Resources:Common, btSeePosts %>' ></asp:HyperLink>
                            </div>   
                            
                        </div>
                    </span>
                </ItemTemplate>
                
            </asp:ListView>
            <asp:Label ID="lblFeedPosts" runat="server" Text="Label" meta:resourcekey="lblFeedPosts" Visible="False"></asp:Label>

        </div>
        <div class="pager" >
            <ul class="pagination">
                <li class="page-item">
                    <asp:Button ID="btFirstPosts" runat="server" CssClass="page-link" Text="<%$ Resources:Common, btFirstPosts %>" OnClick="btFirstPosts_Click" meta:resourcekey="btFirstPostsResource1" />      
                </li>
                <li class="page-item">
                    <asp:Button ID="btMorePosts" CssClass="page-link" runat="server" Text= "<%$ Resources:Common, btMorePosts %>" OnClick="btMorePosts_Click" meta:resourcekey="btMorePostsResource1" />
                </li>
                <li class="page-item">
                    <asp:Button ID="btBack" runat="server" CssClass="page-link" Text="<%$ Resources:Common, btBack %>" OnClick="btBack_Click" />
                </li>
            </ul>
            <asp:Label ID="lblNotMorePost" runat="server" Text="<%$ Resources:Common, lblNotMorePost %>" Visible="False" ForeColor="#CC0000" meta:resourcekey="lblNotMorePostResource1"></asp:Label>

        </div>
        
    </section>
    </form>
    </asp:Content>