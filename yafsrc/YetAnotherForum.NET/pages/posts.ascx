﻿<%@ Control Language="c#" AutoEventWireup="True" Inherits="YAF.Pages.posts" Codebehind="posts.ascx.cs" %>
<%@ Import Namespace="YAF.Core" %>
<%@ Import Namespace="YAF.Types.Interfaces" %>
<%@ Import Namespace="YAF.Controls" %>
<%@ Import Namespace="YAF.Types.Extensions" %>
<%@ Register TagPrefix="YAF" TagName="DisplayPost" Src="../controls/DisplayPost.ascx" %>
<%@ Register TagPrefix="YAF" TagName="DisplayConnect" Src="../controls/DisplayConnect.ascx" %>
<%@ Register TagPrefix="YAF" TagName="DisplayAd" Src="../controls/DisplayAd.ascx" %>
<%@ Register TagPrefix="YAF" TagName="PollList" Src="../controls/PollList.ascx" %>
<%@ Register TagPrefix="YAF" TagName="SimilarTopics" Src="../controls/SimilarTopics.ascx" %>
<%@ Register TagPrefix="modal" TagName="QuickReply" Src="../Dialogs/QuickReply.ascx" %>
<YAF:PageLinks ID="PageLinks" runat="server" />
<YAF:PollList ID="PollList" TopicId='<%# PageContext.PageTopicID %>' ShowButtons='<%# ShowPollButtons() %>' Visible='<%# PollGroupId() > 0 %>' PollGroupId='<%# PollGroupId() %>' runat="server"/>
<a id="top"  name="top"></a>
<table class="command" width="100%">
    <tr>
        <td align="left">
            <YAF:Pager ID="Pager" runat="server" UsePostBack="False" />
        </td>
        <td>
            <span id="dvFavorite1">
                <YAF:ThemeButton ID="TagFavorite1" runat="server" CssClass="yafcssbigbutton rightItem button-favorite"
                    TextLocalizedTag="BUTTON_TAGFAVORITE" TitleLocalizedTag="BUTTON_TAGFAVORITE_TT" />
            </span>        
            <YAF:ThemeButton ID="MoveTopic1" runat="server" CssClass="yafcssbigbutton rightItem button-move"
                OnClick="MoveTopic_Click" TextLocalizedTag="BUTTON_MOVETOPIC" TitleLocalizedTag="BUTTON_MOVETOPIC_TT" />
            <YAF:ThemeButton ID="UnlockTopic1" runat="server" CssClass="yafcssbigbutton rightItem button-unlock"
                OnClick="UnlockTopic_Click" TextLocalizedTag="BUTTON_UNLOCKTOPIC" TitleLocalizedTag="BUTTON_UNLOCKTOPIC_TT" />
            <YAF:ThemeButton ID="LockTopic1" runat="server" CssClass="yafcssbigbutton rightItem button-lock"
                OnClick="LockTopic_Click" TextLocalizedTag="BUTTON_LOCKTOPIC" TitleLocalizedTag="BUTTON_LOCKTOPIC_TT" />
            <YAF:ThemeButton ID="DeleteTopic1" runat="server" CssClass="yafcssbigbutton rightItem button-delete"
                OnClick="DeleteTopic_Click" OnLoad="DeleteTopic_Load" TextLocalizedTag="BUTTON_DELETETOPIC"
                TitleLocalizedTag="BUTTON_DELETETOPIC_TT" />
            <YAF:ThemeButton ID="NewTopic1" runat="server" CssClass="yafcssbigbutton rightItem button-newtopic"
                OnClick="NewTopic_Click" TextLocalizedTag="BUTTON_NEWTOPIC" TitleLocalizedTag="BUTTON_NEWTOPIC_TT" />
            <YAF:ThemeButton ID="PostReplyLink1" runat="server" CssClass="yafcssbigbutton rightItem button-reply"
                OnClick="PostReplyLink_Click" TextLocalizedTag="BUTTON_POSTREPLY" TitleLocalizedTag="BUTTON_POSTREPLY_TT" />
            <YAF:ThemeButton ID="QuickReplyLink1" runat="server" CssClass="btn btn-primary rightItem button-reply"
                             TextLocalizedTag="QUICKREPLY" TitleLocalizedTag="BUTTON_POSTREPLY_TT"
                             Icon="reply" DataTarget="QuickReplyDialog"/>
        </td>
    </tr>
</table>
<table class="content postHeader" width="100%">
    <tr class="postTitle">
        <td class="header1">
            <div class="leftItem">
              <asp:HyperLink ID="TopicLink" runat="server" CssClass="HeaderTopicLink">
                <asp:Label ID="TopicTitle" runat="server" />
              </asp:HyperLink>
            </div>
            <div class="rightItem">
                <div id="fb-root"></div>
                <div style="display:inline">
               <YAF:ThemeButton runat="server" ID="ShareLink"
                                TextLocalizedTag="SHARE" TitleLocalizedTag="SHARE_TOOLTIP"
                                Icon="share" 
                                Type="Link" 
                                CssClass="dropdown-toggle"
                                DataToggle="dropdown">
               </YAF:ThemeButton>
                
                <YAF:PopMenu ID="ShareMenu" runat="server" Control="ShareLink" />
                </div>
                <div style="display:inline">
                <YAF:ThemeButton runat="server" ID="OptionsLink"
                                 TextLocalizedTag="TOOLS" TitleLocalizedTag="OPTIONS_TOOLTIP"
                                 Icon="cog"
                                 Type="Link"
                                 CssClass="dropdown-toggle"
                                 DataToggle="dropdown">
                </YAF:ThemeButton>
                <asp:UpdatePanel ID="PopupMenuUpdatePanel" runat="server" style="display:inline">
                  <ContentTemplate>
                    <span id="WatchTopicID" runat="server" visible="false"></span>
                  </ContentTemplate>
                </asp:UpdatePanel>
                <YAF:PopMenu runat="server" ID="OptionsMenu" Control="OptionsLink" />
                </div>
                
                <div style="display:inline">
                <asp:PlaceHolder ID="ViewOptions" runat="server">
                    <YAF:ThemeButton runat="server" ID="ViewLink"
                                     TextLocalizedTag="VIEW" TitleLocalizedTag="VIEW_TOOLTIP"
                                     Icon="book"
                                     Type="Link"
                                     CssClass="dropdown-toggle"
                                     DataToggle="dropdown">
                    </YAF:ThemeButton>
                    
                </asp:PlaceHolder>
                <YAF:PopMenu ID="ViewMenu" runat="server" Control="ViewLink" />
                </div>
                
                <asp:HyperLink ID="ImageMessageLink" runat="server" CssClass="GoToLink">
                     <YAF:ThemeImage ID="LastPostedImage" runat="server" Style="border: 0" />
                </asp:HyperLink>
                <asp:HyperLink ID="ImageLastUnreadMessageLink" runat="server" CssClass="GoToLink">
                    <YAF:ThemeImage ID="LastUnreadImage" runat="server"  Style="border: 0" />
                </asp:HyperLink>
            </div>
        </td>
    </tr>
    <tr class="header2">
        <td class="header2links">
            <asp:LinkButton ID="PrevTopic" runat="server" ToolTip='<%# this.GetText("prevtopic") %>' CssClass="PrevTopicLink" OnClick="PrevTopic_Click">
                <YAF:LocalizedLabel ID="LocalizedLabel7" runat="server" LocalizedTag="prevtopic" />
            </asp:LinkButton>
            <asp:LinkButton ID="NextTopic" ToolTip='<%# this.GetText("nexttopic") %>' runat="server" CssClass="NextTopicLink" OnClick="NextTopic_Click">
                <YAF:LocalizedLabel ID="LocalizedLabel8" runat="server" LocalizedTag="nexttopic" />
            </asp:LinkButton>
            <div runat="server" visible="false">
                <asp:LinkButton ID="TrackTopic" runat="server" CssClass="header2link" OnClick="TrackTopic_Click">
                    <YAF:LocalizedLabel ID="LocalizedLabel9" runat="server" LocalizedTag="watchtopic" />
                </asp:LinkButton>
                <asp:LinkButton ID="EmailTopic" runat="server" CssClass="header2link" OnClick="EmailTopic_Click">
                    <YAF:LocalizedLabel ID="LocalizedLabel10" runat="server" LocalizedTag="emailtopic" />
                </asp:LinkButton>
                <asp:LinkButton ID="PrintTopic" runat="server" CssClass="header2link" OnClick="PrintTopic_Click">
                    <YAF:LocalizedLabel ID="LocalizedLabel11" runat="server" LocalizedTag="printtopic" />
                </asp:LinkButton>
                <asp:HyperLink ID="RssTopic" runat="server" CssClass="header2link">
                    <YAF:LocalizedLabel ID="LocalizedLabel12" runat="server" LocalizedTag="rsstopic" />
                </asp:HyperLink>
            </div>
        </td>
    </tr>
</table>
<asp:Repeater ID="MessageList" runat="server" OnItemCreated="MessageList_OnItemCreated">
    <ItemTemplate>
        <table class="content postContainer" width="100%">
            <%# GetThreadedRow(Container.DataItem) %>
            <YAF:DisplayPost ID="DisplayPost1" runat="server" DataRow="<%# Container.DataItem %>"
                Visible="<%#IsCurrentMessage(Container.DataItem)%>" PostCount="<%# Container.ItemIndex %>" CurrentPage="<%# Pager.CurrentPageIndex %>" IsThreaded="<%#IsThreaded%>" />
        </table>
        <YAF:DisplayAd ID="DisplayAd" runat="server" Visible="False" />
        <YAF:DisplayConnect ID="DisplayConnect" runat="server" Visible="False" />
    </ItemTemplate>
    <AlternatingItemTemplate>        
        <table class="content postContainer_Alt" width="100%">
            <%# GetThreadedRow(Container.DataItem) %>
            <YAF:DisplayPost ID="DisplayPostAlt" runat="server" DataRow="<%# Container.DataItem %>"
                IsAlt="True" Visible="<%#IsCurrentMessage(Container.DataItem)%>" PostCount="<%# Container.ItemIndex %>" CurrentPage="<%# Pager.CurrentPageIndex %>" IsThreaded="<%#IsThreaded%>" />
        </table>
        <YAF:DisplayAd ID="DisplayAd" runat="server" Visible="False" />
        <YAF:DisplayConnect ID="DisplayConnect" runat="server" Visible="False" />
    </AlternatingItemTemplate>
</asp:Repeater>
<table class="header2 postNavigation" width="100%"  id="tbFeeds" runat="server" visible="<%# this.Get<IPermissions>().Check(this.Get<YafBoardSettings>().PostsFeedAccess) %>">
<tr>
<td class="post">
    <YAF:RssFeedLink ID="RssFeed" runat="server" FeedType="Posts"  AdditionalParameters='<%# "t={0}".FormatWith(PageContext.PageTopicID) %>' TitleLocalizedTag="RSSICONTOOLTIPACTIVE" Visible="<%# this.Get<YafBoardSettings>().ShowRSSLink && this.Get<IPermissions>().Check(this.Get<YafBoardSettings>().PostsFeedAccess) %>" />&nbsp; 
    <YAF:RssFeedLink ID="AtomFeed" runat="server" FeedType="Posts" AdditionalParameters='<%# "t={0}".FormatWith(PageContext.PageTopicID) %>' IsAtomFeed="true" Visible="<%# this.Get<YafBoardSettings>().ShowAtomLink && this.Get<IPermissions>().Check(this.Get<YafBoardSettings>().PostsFeedAccess) %>" ImageThemeTag="ATOMFEED" TextLocalizedTag="ATOMFEED" TitleLocalizedTag="ATOMICONTOOLTIPACTIVE" />
</td>
</tr>
</table>                           
<table class="content postForumUsers" width="100%">
    <YAF:ForumUsers ID="ForumUsers1" runat="server" />
</table>
<YAF:SimilarTopics ID="SimilarTopics"  runat="server" Topic='<%# this.PageContext.PageTopicName %>'>
</YAF:SimilarTopics>
<table cellpadding="0" cellspacing="0" class="command" width="100%">
    <tr>
        <td align="left">
            <YAF:Pager ID="PagerBottom" runat="server" LinkedPager="Pager" UsePostBack="false" />
        </td>
        <td>
            <span id="dvFavorite2">
                <YAF:ThemeButton ID="TagFavorite2" runat="server" CssClass="yafcssbigbutton rightItem button-favorite"
                    TextLocalizedTag="BUTTON_TAGFAVORITE" TitleLocalizedTag="BUTTON_TAGFAVORITE_TT" />
            </span>        
            <YAF:ThemeButton ID="MoveTopic2" runat="server" CssClass="yafcssbigbutton rightItem button-move"
                OnClick="MoveTopic_Click" TextLocalizedTag="BUTTON_MOVETOPIC" TitleLocalizedTag="BUTTON_MOVETOPIC_TT" />
            <YAF:ThemeButton ID="UnlockTopic2" runat="server" CssClass="yafcssbigbutton rightItem button-unlock"
                OnClick="UnlockTopic_Click" TextLocalizedTag="BUTTON_UNLOCKTOPIC" TitleLocalizedTag="BUTTON_UNLOCKTOPIC_TT" />
            <YAF:ThemeButton ID="LockTopic2" runat="server" CssClass="yafcssbigbutton rightItem button-lock"
                OnClick="LockTopic_Click" TextLocalizedTag="BUTTON_LOCKTOPIC" TitleLocalizedTag="BUTTON_LOCKTOPIC_TT" />
            <YAF:ThemeButton ID="DeleteTopic2" runat="server" CssClass="yafcssbigbutton rightItem button-delete"
                OnClick="DeleteTopic_Click" OnLoad="DeleteTopic_Load" TextLocalizedTag="BUTTON_DELETETOPIC"
                TitleLocalizedTag="BUTTON_DELETETOPIC_TT" />
            <YAF:ThemeButton ID="NewTopic2" runat="server" CssClass="yafcssbigbutton rightItem button-newtopic"
                OnClick="NewTopic_Click" TextLocalizedTag="BUTTON_NEWTOPIC" TitleLocalizedTag="BUTTON_NEWTOPIC_TT" />
            <YAF:ThemeButton ID="PostReplyLink2" runat="server" CssClass="yafcssbigbutton rightItem button-reply"
                OnClick="PostReplyLink_Click" TextLocalizedTag="BUTTON_POSTREPLY" TitleLocalizedTag="BUTTON_POSTREPLY_TT" />
            <YAF:ThemeButton ID="QuickReplyLink2" runat="server" CssClass="btn btn-primary rightItem button-reply"
                             TextLocalizedTag="QUICKREPLY" TitleLocalizedTag="BUTTON_POSTREPLY_TT"
                             Icon="reply" DataTarget="QuickReplyDialog"/>
        </td>
    </tr>
</table>
<YAF:PageLinks ID="PageLinksBottom" runat="server" LinkedPageLinkID="PageLinks" />
<asp:PlaceHolder ID="ForumJumpHolder" runat="server">
    <div class="float-right">
        <YAF:LocalizedLabel ID="ForumJumpLabel" runat="server" LocalizedTag="FORUM_JUMP" />
        &nbsp;<YAF:ForumJump ID="ForumJump1" runat="server" />
    </div>
</asp:PlaceHolder>
<div class="float-right">
    <YAF:PageAccess ID="PageAccess1" runat="server" />
</div>
<div id="DivSmartScroller">
    <YAF:SmartScroller ID="SmartScroller1" runat="server" />
</div>
<modal:QuickReply ID="QuickReplyDialog" runat="server" />