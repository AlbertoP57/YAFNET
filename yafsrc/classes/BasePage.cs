/* Yet Another Forum.net
 * Copyright (C) 2003 Bj�rnar Henden
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Globalization;
using System.Threading;
using yaf.classes;

// Gr�nn: #25C110
// Brown: #D0BF8C

namespace yaf
{
	/// <summary>
	/// Summary description for BasePage.
	/// </summary>
	public class BasePage : System.Web.UI.Page
	{
		private HiPerfTimer	hiTimer				= new HiPerfTimer(true);
		private DataRow		m_pageinfo;
		private string		m_strForumName		= "Yet Another Forum.net";
		private string		m_strLoadMessage	= "";
		private bool		m_bNoDataBase		= false;
		private bool		m_bShowToolBar		= true;
		private string		m_strThemeDir		= System.Configuration.ConfigurationSettings.AppSettings["themedir"];
		private string		m_strSmtpServer		= System.Configuration.ConfigurationSettings.AppSettings["smtpserver"];
		private string		m_strForumEmail		= System.Configuration.ConfigurationSettings.AppSettings["forumemail"];

		/// <summary>
		/// Constructor
		/// </summary>
		public BasePage()
		{
			this.Load += new System.EventHandler(this.Page_Load);
			this.Error += new System.EventHandler(this.Page_Error);
		}

		private void Page_Error(object sender, System.EventArgs e) 
		{
			if(!IsLocal) 
				Utils.ReportError(Server.GetLastError());
		}

		/// <summary>
		/// Called when page is loaded
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Page_Load(object sender, System.EventArgs e) 
		{
			// Set the culture and UI culture to the browser's accept language
			try 
			{
				string sCulture = Request.UserLanguages[0];
				if(sCulture.IndexOf(';')>=0) 
					sCulture = sCulture.Substring(0,sCulture.IndexOf(';'));

				Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(sCulture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(sCulture);
			}
			catch(Exception)
			{
			}

			//Response.Expires = -1000;
			Response.AddHeader("Cache-control","private, no-cache, must-revalidate");
			Response.AddHeader("Expires","Mon, 26 Jul 1997 05:00:00 GMT"); // Past date
			Response.AddHeader("Pragma","no-cache");

			if(m_bNoDataBase) return;

			DataTable banip = (DataTable)Cache["bannedip"];
			if(banip == null) {
				banip = DB.bannedip_list(null);
				Cache["bannedip"] = banip;
			}
			foreach(DataRow row in banip.Rows)
				if(Utils.IsBanned((string)row["Mask"],Request.ServerVariables["REMOTE_ADDR"]))
					Response.End();

			// Find user name
			AuthType authType = Data.GetAuthType;
			string	sUserIdentityName = User.Identity.Name;
			string	sUserEmail = null;
			if(authType==AuthType.RainBow) 
			{
				try 
				{
					string[] split = sUserIdentityName.Split('|');
					sUserIdentityName = split[0];
					sUserEmail = split[1];
				}
				catch(Exception) 
				{
					sUserIdentityName = User.Identity.Name;
				}
			}

			m_pageinfo = DB.pageload(
				Session.SessionID,
				sUserIdentityName,
				Request.UserHostAddress,
				Request.FilePath,
				Request.Browser.Browser,
				Request.Browser.Platform,
				Request.QueryString["c"],
				Request.QueryString["f"],
				Request.QueryString["t"],
				Request.QueryString["m"]);

			// If user wasn't found and we have foreign 
			// authorization, try to register the user.
			if(m_pageinfo==null && authType!=AuthType.YetAnotherForum) 
			{
				DB.user_register(this,sUserIdentityName,"ext",sUserEmail,"-","-",0,false);

				m_pageinfo = DB.pageload(
					Session.SessionID,
					sUserIdentityName,
					Request.UserHostAddress,
					Request.FilePath,
					Request.Browser.Browser,
					Request.Browser.Platform,
					Request.QueryString["c"],
					Request.QueryString["f"],
					Request.QueryString["t"],
					Request.QueryString["m"]);
			}

			if(m_pageinfo==null) 
			{
				if(User.Identity.IsAuthenticated) 
				{
					System.Web.Security.FormsAuthentication.SignOut();
					Response.Redirect(BaseDir);
				} 
				else
					throw new Exception("Couldn't find user.");
			}

			m_strForumName = (string)m_pageinfo["BBName"];
			m_strSmtpServer = (string)m_pageinfo["SmtpServer"];
			m_strForumEmail = (string)m_pageinfo["ForumEmail"];

			if(Session["lastvisit"] == null && Request.Cookies["yaf"] != null && Request.Cookies["yaf"]["lastvisit"] != null) 
			{
				try 
				{
					Session["lastvisit"] = DateTime.Parse(Request.Cookies["yaf"]["lastvisit"]);
				}
				catch(Exception) 
				{
					Session["lastvisit"] = DateTime.Now;
				}
				Response.Cookies["yaf"]["lastvisit"] = DateTime.Now.ToString();
				Response.Cookies["yaf"].Expires = DateTime.Now + TimeSpan.FromDays(365);
			}
			else if(Session["lastvisit"] == null) 
			{
				Session["lastvisit"] = DateTime.Now;
			}

			if(Request.Cookies["yaf"] != null && Request.Cookies["yaf"]["lastvisit"] != null) 
			{
				try 
				{
					if(DateTime.Parse(Request.Cookies["yaf"]["lastvisit"]) < DateTime.Now - TimeSpan.FromMinutes(5)) 
					{
						Response.Cookies["yaf"]["lastvisit"] = DateTime.Now.ToString();
						Response.Cookies["yaf"].Expires = DateTime.Now + TimeSpan.FromDays(365);
					}
				}
				catch(Exception) 
				{
					Response.Cookies["yaf"]["lastvisit"] = DateTime.Now.ToString();
					Response.Cookies["yaf"].Expires = DateTime.Now + TimeSpan.FromDays(365);
				}
			}
			else 
			{
				Response.Cookies["yaf"]["lastvisit"] = DateTime.Now.ToString();
				Response.Cookies["yaf"].Expires = DateTime.Now + TimeSpan.FromDays(365);
			}

			// Check if pending mails, and send 10 of them if possible
			if((int)m_pageinfo["MailsPending"]>0) {
				try {
					using(DataTable dt = DB.mail_list()) 
					{
						for(int i=0;i<dt.Rows.Count;i++) 
						{
							// Build a MailMessage
							SendMail(ForumEmail,(string)dt.Rows[i]["ToUser"],(string)dt.Rows[i]["Subject"],(string)dt.Rows[i]["Body"]);
							DB.mail_delete(dt.Rows[i]["MailID"]);
						}
						if(IsAdmin) AddLoadMessage(String.Format("Sent {0} mails.",dt.Rows.Count));
					}
				}
				catch(Exception x) {
					if(IsAdmin) {
						AddLoadMessage(x.Message);
					}
				}
			}
		}

		/// <summary>
		/// Writes the document
		/// </summary>
		/// <param name="writer"></param>
		protected override void Render(System.Web.UI.HtmlTextWriter writer) 
		{
#if true
			string html = ReadTemplate("page.html");
#else
			string html;
			if(Cache["htmltemplate"] != null) {
				html = Cache["htmltemplate"].ToString();
			} else {
				html = ReadTemplate("page.html");
				Cache["htmltemplate"] = html;
			}
#endif
			
			if(!m_bShowToolBar) {
				writer.WriteLine("<html>");
				writer.WriteLine("<!-- Copyright 2003 Bj�rnar Henden -->");
				writer.WriteLine("<head>");
				writer.WriteLine(String.Format("<link rel=stylesheet type=text/css href={0}forum.css>",BaseDir));
				writer.WriteLine(String.Format("<link rel=stylesheet type=text/css href={0}>",ThemeFile("theme.css")));
				writer.WriteLine(String.Format("<title>{0}</title>",ForumName));
				writer.WriteLine("<script>");
				writer.WriteLine("function yaf_onload() {");
				if(m_strLoadMessage.Length>0)
					writer.WriteLine(String.Format("	alert(\"{0}\");",m_strLoadMessage));
				writer.WriteLine("}");
				writer.WriteLine("</script>");
				writer.WriteLine("</head>");
				writer.WriteLine("<body onload='yaf_onload()'>");
			} else {
				string title = String.Format("<title>{0}</title>",ForumName);
				string css = String.Format("<link type=text/css rel=stylesheet href='{0}forum.css' />",BaseDir);
				css += String.Format("\n<link type=text/css rel=stylesheet href='{0}' />",ThemeFile("theme.css"));
				string script = "<script>\nfunction yaf_onload() {}\n</script>";
				if(m_strLoadMessage.Length>0)
					script = String.Format("<script>\nfunction yaf_onload() {1}\nalert(\"{0}\")\n{2}\n</script>\n",m_strLoadMessage,'{','}');

				html = html.Replace("{title}",title);
				html = html.Replace("{css}",css);
				html = html.Replace("{script}",script);
			}

			if(m_bShowToolBar) 
			{
				int pos = html.IndexOf("{forum}");
				if(pos<0)
					throw new Exception("Invalid template -- {forum} constant is missing.");

				writer.Write(html.Substring(0,pos));	// Write html before forum


				writer.WriteLine("<table width=100% cellspacing=0 class=content cellpadding=0><tr>");

				if(User.Identity.IsAuthenticated) 
				{
					writer.WriteLine(String.Format("<td style=\"padding:5px\" class=post align=left><b>Logged in as: {0}</b></td>",PageUserName));

					writer.WriteLine("<td style=\"padding:5px\" align=right valign=middle class=post>");
					if(IsAdmin)
						writer.WriteLine(String.Format("	<a href=\"admin/\">Admin</a> |"));
					writer.WriteLine("	<a href=\"active.aspx\">Active Topics</a> |");
					if(!IsGuest)
						writer.WriteLine(String.Format("	<a href=\"cp_profile.aspx\">My Profile</a> |"));
					writer.WriteLine(String.Format("	<a href=\"members.aspx\">Members</a>"));
					if(Data.GetAuthType==AuthType.YetAnotherForum)
						writer.WriteLine(String.Format("| <a href=\"logout.aspx\">Logout</a>"));
				} 
				else 
				{
					writer.WriteLine("<td style=\"padding:5px\" class=post align=left><b>Welcome Guest</b></td>");

					writer.WriteLine("<td style=\"padding:5px\" align=right valign=middle class=post>");
					if(Data.GetAuthType==AuthType.YetAnotherForum) 
					{
						writer.WriteLine(String.Format("	<a href=\"login.aspx\">Log In</a> |"));
						writer.WriteLine(String.Format("	<a href=\"rules.aspx\">Register</a> |"));
					}
					writer.WriteLine(String.Format("	<a href=\"members.aspx\">Members</a>"));
				}
				writer.WriteLine("</td></tr></table>");
				writer.WriteLine("<br />");


				RenderBody(writer);
				writer.WriteLine("<p style=\"text-align:center;font-size:7pt\">");

				writer.WriteLine(String.Format("Powered by <a title=\"Yet Another Forum.net Home Page\" href=\"http://www.yetanotherforum.net/\">{0}</a> version {1} - {2}<br/>Copyright &copy; 2003 Yet Another Forum.net. All rights reserved.","Yet Another Forum.net",AppVersionName,FormatDateShort(AppVersionDate)));
				hiTimer.Stop();
				writer.WriteLine(String.Format(CustomCulture,"<br>This page was generated in {0:N3} seconds.",hiTimer.Duration));

				writer.WriteLine("</p>");
				writer.Write(html.Substring(pos+8));	// Write html after forum
			} else {
				RenderBody(writer);
				writer.WriteLine("</body>");
				writer.WriteLine("</html>");
			}
		}

		/// <summary>
		/// Renders the body
		/// </summary>
		/// <param name="writer"></param>
		protected virtual void RenderBody(System.Web.UI.HtmlTextWriter writer) 
		{
			RenderBase(writer);
		}

		/// <summary>
		/// Calls the base class to render components
		/// </summary>
		/// <param name="writer"></param>
		protected void RenderBase(System.Web.UI.HtmlTextWriter writer) 
		{
			base.Render(writer);
		}

		/// <summary>
		/// Set to false if you don't want the menus at top and bottom. Only admin pages will set this to false
		/// </summary>
		protected bool ShowToolBar 
		{
			set 
			{
				m_bShowToolBar = value;
			}
		}

		/// <summary>
		/// The name of the froum
		/// </summary>
		public string ForumName 
		{
			get 
			{
				return m_strForumName;
			}
		}
		/// <summary>
		/// Returns the forum url
		/// </summary>
		public string ForumURL {
			get {
				long port = long.Parse(Request.ServerVariables["SERVER_PORT"]);
				if(port!=80)
					return String.Format("http://{0}:{1}{2}",Request.ServerVariables["SERVER_NAME"],port,BaseDir);
				else
					return String.Format("http://{0}{1}",Request.ServerVariables["SERVER_NAME"],BaseDir);
			}
		}
		/// <summary>
		/// Find the path of a smiley icon
		/// </summary>
		/// <param name="icon">The file name of the icon you want</param>
		/// <returns>The path to the image file</returns>
		public string Smiley(string icon) {
			return String.Format("{0}images/emoticons/{1}",BaseDir,icon);
		}
		/// <summary>
		/// The directory of theme files
		/// </summary>
		protected string ThemeDir 
		{
			get 
			{
				return m_strThemeDir;
			}
		}

		/// <summary>
		/// Base directory for forum
		/// </summary>
		public string BaseDir 
		{
			get 
			{
				return Data.BaseDir;
			}
		}

		/// <summary>
		/// Returns the full path of a file in the themedir
		/// </summary>
		/// <param name="filename">The name of the file relative to the theme directory</param>
		/// <returns></returns>
		public string ThemeFile(string filename) 
		{
			return ThemeDir + filename;
		}

		/// <summary>
		/// Adds a message that is displayed to the user when the page is loaded.
		/// </summary>
		/// <param name="msg">The message to display</param>
		public void AddLoadMessage(string msg) 
		{
			msg = msg.Replace("\\","\\\\");
			msg = msg.Replace("'","\\'");
			msg = msg.Replace("\r\n","\\r\\n");
			msg = msg.Replace("\n","\\n");
			msg = msg.Replace("\"","\\\"");
			m_strLoadMessage += msg + "\\n\\n";
		}

		/// <summary>
		/// The CultureInfo for the current user
		/// </summary>
		protected CultureInfo CustomCulture {
			get {
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		/// <summary>
		/// Set to true if this is the start page. Should only be set by the page that initialized the database.
		/// </summary>
		protected bool NoDataBase {
			set {
				m_bNoDataBase = value;
			}
		}
		/// <summary>
		/// The smtp server to send mails from
		/// </summary>
		public string SmtpServer {
			get {
				return m_strSmtpServer.Length>0 ? m_strSmtpServer : null;
			}
		}
		public string SmtpUserName 
		{
			get 
			{
				string tmp = m_pageinfo["SmtpUserName"].ToString();
				return tmp.Length>0 ? tmp : null;
			}
		}
		public string SmtpUserPass
		{
			get 
			{
				string tmp = m_pageinfo["SmtpUserPass"].ToString();
				return tmp.Length>0 ? tmp : null;
			}
		}
		/// <summary>
		/// The official forum email address. 
		/// </summary>
		public string ForumEmail 
		{
			get {
				return m_strForumEmail;
			}
		}

		public bool UseBlankLinks 
		{
			get 
			{
				return (bool)m_pageinfo["BlankLinks"];
			}
		}

		public bool UseEmailVerification 
		{
			get 
			{
				return (bool)m_pageinfo["EmailVerification"];
			}
		}

		public bool ShowMovedTopics
		{
			get 
			{
				return (bool)m_pageinfo["ShowMoved"];
			}
		}

		public string ReadTemplate(string name) 
		{
			string file;
			if(Cache[name] != null && false) {
				file = Cache[name].ToString();
			} else {
				string templatefile = Server.MapPath(String.Format("{0}templates/{1}",BaseDir,name));
				StreamReader sr = new StreamReader(templatefile,Encoding.ASCII);
				file = sr.ReadToEnd();
				sr.Close();
				Cache[name] = file;
			}
			return file;
		}

		public bool IsLocal {
			get {
				string s = Request.ServerVariables["SERVER_NAME"];
				return s!=null && s.ToLower()=="localhost";
			}
		}

		#region User access functions
		/// <summary>
		/// The UserID of the current user.
		/// </summary>
		public int PageUserID {
			get {
				if(m_pageinfo!=null)
					return (int)m_pageinfo["UserID"];
				else
					return 0;
			}
		}
		public string PageUserName 
		{
			get 
			{
				if(m_pageinfo!=null)
					return (string)m_pageinfo["UserName"];
				else
					return "";
			}
		}
		/// <summary>
		/// ForumID for the current page, or 0 if not in any forum
		/// </summary>
		protected int PageForumID {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("ForumID"))
					return (int)m_pageinfo["ForumID"];
				else
					return 0;
			}
		}
		/// <summary>
		/// Name of forum for the current page, or an empty string if not in any forum
		/// </summary>
		protected string PageForumName {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("ForumName"))
					return (string)m_pageinfo["ForumName"];
				else
					return "";
			}
		}
		/// <summary>
		/// CategoryID for the current page, or 0 if not in any category
		/// </summary>
		protected int PageCategoryID {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("CategoryID"))
					return (int)m_pageinfo["CategoryID"];
				else
					return 0;
			}
		}
		/// <summary>
		/// Name of category for the current page, or an empty string if not in any category
		/// </summary>
		protected string PageCategoryName {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("CategoryName"))
					return (string)m_pageinfo["CategoryName"];
				else
					return "";
			}
		}
		/// <summary>
		/// The TopicID of the current page, or 0 if not in any topic
		/// </summary>
		protected int PageTopicID {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("TopicID"))
					return (int)m_pageinfo["TopicID"];
				else
					return 0;
			}
		}
		/// <summary>
		/// Name of topic for the current page, or an empty string if not in any topic
		/// </summary>
		protected string PageTopicName {
			get {
				if(m_pageinfo!=null && !m_pageinfo.IsNull("TopicName"))
					return (string)m_pageinfo["TopicName"];
				else
					return "";
			}
		}
		/// <summary>
		/// True if current user is an administrator
		/// </summary>
		protected bool IsAdmin {
			get {
				if(m_pageinfo!=null)
					return long.Parse(m_pageinfo["IsAdmin"].ToString())!=0;
				else
					return false;
			}
		}
		/// <summary>
		/// True if the current user is a guest
		/// </summary>
		protected bool IsGuest {
			get {
				if(m_pageinfo!=null)
					return long.Parse(m_pageinfo["IsGuest"].ToString())!=0;
				else
					return false;
			}
		}
		/// <summary>
		/// True if current user has post access in the current forum
		/// </summary>
		protected bool ForumPostAccess {
			get {
				if(m_pageinfo.IsNull("PostAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["PostAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user has reply access in the current forum
		/// </summary>
 		protected bool ForumReplyAccess {
			get {
				if(m_pageinfo.IsNull("ReplyAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["ReplyAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user has read access in the current forum
		/// </summary>
		protected bool ForumReadAccess {
			get {
				if(m_pageinfo.IsNull("ReadAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["ReadAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user has access to create priority topics in the current forum
		/// </summary>
		protected bool ForumPriorityAccess {
			get {
				if(m_pageinfo.IsNull("PriorityAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["PriorityAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user has access to create polls in the current forum.
		/// </summary>
		protected bool ForumPollAccess {
			get {
				if(m_pageinfo.IsNull("PollAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["PollAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user has access to vote on polls in the current forum
		/// </summary>
		protected bool ForumVoteAccess {
			get {
				if(m_pageinfo.IsNull("VoteAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["VoteAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user is a moderator of the current forum
		/// </summary>
		protected bool ForumModeratorAccess {
			get {
				if(m_pageinfo.IsNull("ModeratorAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["ModeratorAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user can delete own messages in the current forum
		/// </summary>
		protected bool ForumDeleteAccess {
			get {
				if(m_pageinfo.IsNull("DeleteAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["DeleteAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user can edit own messages in the current forum
		/// </summary>
		protected bool ForumEditAccess {
			get {
				if(m_pageinfo.IsNull("EditAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["EditAccess"].ToString())>0;
			}
		}
		/// <summary>
		/// True if the current user can upload attachments
		/// </summary>
		protected bool ForumUploadAccess 
		{
			get 
			{
				if(m_pageinfo.IsNull("UploadAccess"))
					return false;
				else
					return long.Parse(m_pageinfo["UploadAccess"].ToString())>0;
			}
		}
		#endregion
		#region Date and time functions
		/// <summary>
		/// Returns the forum timezone offset from GMT
		/// </summary>
		public TimeSpan TimeZoneOffsetForum 
		{
			get {
				return new TimeSpan(1,0,0);
			}
		}
		/// <summary>
		/// Returns the user timezone offset from GMT
		/// </summary>
		public TimeSpan TimeZoneOffsetUser {
			get {
				if(m_pageinfo!=null) {
					int min = (int)m_pageinfo["TimeZone"];
					return new TimeSpan(min/60,min%60,0);
				} else
					return new TimeSpan(0);
			}
		}
		/// <summary>
		/// Returns the time zone offset for the current user compared to the forum time zone.
		/// </summary>
		public TimeSpan TimeOffset {
			get {
				return TimeZoneOffsetUser - TimeZoneOffsetForum;
			}
		}
		/// <summary>
		/// Formats a datetime value into 07.03.2003 22:32:34
		/// </summary>
		/// <param name="dt">The date to be formatted</param>
		/// <returns></returns>
		public string FormatDateTime(object o) {
			DateTime dt = (DateTime)o;
			return String.Format(CustomCulture,"{0:F}",dt + TimeOffset);
		}
		/// <summary>
		/// Formats a datetime value into 7. februar 2003
		/// </summary>
		/// <param name="dt">The date to be formatted</param>
		/// <returns></returns>
		public string FormatDateLong(DateTime dt) {
			return String.Format(CustomCulture,"{0:D}",dt + TimeOffset);
		}
		/// <summary>
		/// Formats a datetime value into 07.03.2003
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public string FormatDateShort(DateTime dt) {
			return String.Format(CustomCulture,"{0:d}",dt + TimeOffset);
		}
		/// <summary>
		/// Formats a datetime value into 22:32:34
		/// </summary>
		/// <param name="dt">The date to be formatted</param>
		/// <returns></returns>
		public string FormatTime(DateTime dt) {
			return String.Format(CustomCulture,"{0:T}",dt + TimeOffset);
		}
		#endregion

	
		public void SendMail(string from,string to,string subject,string body) 
		{
#if false
			// .NET
			System.Web.Mail.MailMessage mailMessage = new System.Web.Mail.MailMessage();
			mailMessage.From = from;
			mailMessage.To = to;
			mailMessage.Subject = subject;
			mailMessage.BodyFormat = System.Web.Mail.MailFormat.Text;
			mailMessage.Body = body;
			if(SmtpServer!=null)
				System.Web.Mail.SmtpMail.SmtpServer = SmtpServer;
			System.Web.Mail.SmtpMail.Send(mailMessage);
#else
			// http://sourceforge.net/projects/opensmtp-net/
			OpenSmtp.Mail.SmtpConfig.VerifyAddresses = false;

			OpenSmtp.Mail.Smtp smtp = new OpenSmtp.Mail.Smtp(SmtpServer,25);
			if(SmtpUserName!=null && SmtpUserPass!=null) 
			{
				smtp.Username = SmtpUserName;
				smtp.Password = SmtpUserPass;
			}
			smtp.SendMail(from,to,subject,body);
#endif
		}

		static public int AppVersion 
		{
			get 
			{
				return 6;
			}
		}
		static public string AppVersionName 
		{
			get 
			{
				return "0.9.0";
			}
		}
		static public DateTime AppVersionDate 
		{
			get 
			{
				return new DateTime(2003,10,6);
			}
		}
	}
}
