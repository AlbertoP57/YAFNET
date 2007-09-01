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
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using YAF.Classes.Utils;
using YAF.Classes.Data;

namespace YAF.Pages.Admin
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public partial class editboard : YAF.Classes.Base.AdminPage {
	
		protected void Page_Load(object sender, System.EventArgs e) 
		{
			if(!IsPostBack) {
				PageLinks.AddLink(PageContext.BoardSettings.Name,YAF.Classes.Utils.yaf_BuildLink.GetLink( YAF.Classes.Utils.ForumPages.forum));
				PageLinks.AddLink("Administration",YAF.Classes.Utils.yaf_BuildLink.GetLink( YAF.Classes.Utils.ForumPages.admin_admin));
				PageLinks.AddLink("Boards","");

				BindData();
				if(Request.QueryString["b"] != null) 
				{
					AdminInfo.Visible = false;
					using(DataTable dt = YAF.Classes.Data.DB.board_list(Request.QueryString["b"])) 
					{
						DataRow row = dt.Rows[0];
						Name.Text = (string)row["Name"];
						AllowThreaded.Checked = (bool)row["AllowThreaded"];
					}
				}
				else
				{
					UserName.Text = User.UserName;
					UserEmail.Text = User.Email;
				}
			}
		}

		private void BindData() {
			DataBind();
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void Save_Click(object sender, System.EventArgs e)
		{
			if(Name.Text.Trim().Length==0) 
			{
				PageContext.AddLoadMessage("You must enter a name for the board.");
				return;
			}
			if(Request.QueryString["b"] == null) 
			{
				if(UserName.Text.Trim().Length==0)
				{
					PageContext.AddLoadMessage("You must enter the name of a administrator user.");
					return;
				}
				if(UserEmail.Text.Trim().Length==0) 
				{
					PageContext.AddLoadMessage("You must enter the email address of the administrator user.");
					return;
				}
				if(UserPass1.Text.Trim().Length==0)
				{
					PageContext.AddLoadMessage("You must enter a password for the administrator user.");
					return;
				}
				if(UserPass1.Text!=UserPass2.Text)
				{
					PageContext.AddLoadMessage("The passwords don't match.");
					return;
				}
			}

			if(Request.QueryString["b"] != null) 
			{
				YAF.Classes.Data.DB.board_save(Request.QueryString["b"],Name.Text,AllowThreaded.Checked);
			} 
			else
			{
				YAF.Classes.Data.DB.board_create(Name.Text,AllowThreaded.Checked,UserName.Text,UserEmail.Text,System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(UserPass1.Text,"md5"));
			}

			// Done
			PageContext.BoardSettings = null;
			YAF.Classes.Utils.yaf_BuildLink.Redirect( YAF.Classes.Utils.ForumPages.admin_boards);
		}

		protected void Cancel_Click(object sender, System.EventArgs e)
		{
			YAF.Classes.Utils.yaf_BuildLink.Redirect( YAF.Classes.Utils.ForumPages.admin_boards);
		}

		protected void BindData_AccessMaskID(object sender, System.EventArgs e) 
		{
			((DropDownList)sender).DataSource = YAF.Classes.Data.DB.accessmask_list(PageContext.PageBoardID,null);
			((DropDownList)sender).DataValueField = "AccessMaskID";
			((DropDownList)sender).DataTextField = "Name";
		}

		protected void SetDropDownIndex(object sender, System.EventArgs e) 
		{
			try
			{
				DropDownList list = (DropDownList)sender;
				list.Items.FindByValue(list.Attributes["value"]).Selected = true;
			}
			catch(Exception)
			{
			}
		}
	}
}
