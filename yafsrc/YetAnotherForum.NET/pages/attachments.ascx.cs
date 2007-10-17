using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using YAF.Classes.Utils;
using YAF.Classes.Data;

namespace YAF.Pages // YAF.Pages
{
	/// <summary>
	/// Summary description for attachments.
	/// </summary>
	public partial class attachments : YAF.Classes.Base.ForumPage
	{
		private DataRow _forum, _topic;

		public attachments()
			: base( "ATTACHMENTS" )
		{
		}

		protected void Page_Load( object sender, System.EventArgs e )
		{
			using ( DataTable dt = YAF.Classes.Data.DB.forum_list( PageContext.PageBoardID, PageContext.PageForumID ) )
				_forum = dt.Rows [0];
			_topic = YAF.Classes.Data.DB.topic_info( PageContext.PageTopicID );

			if ( !IsPostBack )
			{
				if ( !PageContext.ForumModeratorAccess && !PageContext.ForumUploadAccess )
					YafBuildLink.AccessDenied();

				if ( !PageContext.ForumReadAccess )
					YafBuildLink.AccessDenied();

				// Ederon : 9/9/2007 - moderaotrs can attach in locked posts
				if (General.BinaryAnd(_topic["Flags"], TopicFlags.Locked) && !PageContext.ForumModeratorAccess)
					YafBuildLink.AccessDenied(/*"The topic is closed."*/);

				if (General.BinaryAnd(_forum["Flags"], ForumFlags.Locked))
					YafBuildLink.AccessDenied(/*"The forum is closed."*/);

				// Check that non-moderators only edit messages they have written
				if ( !PageContext.ForumModeratorAccess )
					using ( DataTable dt = YAF.Classes.Data.DB.message_list( Request.QueryString ["m"] ) )
						if ( ( int ) dt.Rows [0] ["UserID"] != PageContext.PageUserID )
							YafBuildLink.AccessDenied(/*"You didn't post this message."*/);

				if ( PageContext.Settings.LockedForum == 0 )
				{
					PageLinks.AddLink( PageContext.BoardSettings.Name, YAF.Classes.Utils.YafBuildLink.GetLink( YAF.Classes.Utils.ForumPages.forum ) );
					PageLinks.AddLink( PageContext.PageCategoryName, YAF.Classes.Utils.YafBuildLink.GetLink( YAF.Classes.Utils.ForumPages.forum, "c={0}", PageContext.PageCategoryID ) );
				}
				PageLinks.AddForumLinks( PageContext.PageForumID );
				PageLinks.AddLink( PageContext.PageTopicName, YAF.Classes.Utils.YafBuildLink.GetLink( YAF.Classes.Utils.ForumPages.posts, "t={0}", PageContext.PageTopicID ) );
				PageLinks.AddLink( GetText( "TITLE" ), "" );

				Back.Text = GetText( "BACK" );
				Upload.Text = GetText( "UPLOAD" );

        // MJ : 10/14/2007 - list of allowed file extensions
        DataTable extensionTable = YAF.Classes.Data.DB.extension_list(PageContext.PageBoardID);

				string types = "";
				bool bFirst = true;

				foreach ( DataRow row in extensionTable.Rows )
				{
					types += String.Format( "{1}*.{0}", row ["Extension"].ToString(), ( bFirst ? "" : ", " ) );
					if ( bFirst ) bFirst = false;
				}

				if ( !String.IsNullOrEmpty( types ) )
				{
					ExtensionsList.Text = types;
				}
                
				BindData();
			}
		}

		private void BindData()
		{
			DataTable dt = YAF.Classes.Data.DB.attachment_list( Request.QueryString ["m"], null, null );
			List.DataSource = dt;

			List.Visible = ( dt.Rows.Count > 0 ) ? true : false;

			// show disallowed or allowed localized text depending on the Board Setting
			ExtensionTitle.LocalizedTag = ( PageContext.BoardSettings.FileExtensionAreAllowed ? "ALLOWED_EXTENSIONS" : "DISALLOWED_EXTENSIONS" );

			DataBind();
		}

		protected void Delete_Load( object sender, System.EventArgs e )
		{
			( ( LinkButton ) sender ).Attributes ["onclick"] = String.Format( "return confirm('{0}')", GetText( "ASK_DELETE" ) );
		}

		private void Back_Click( object sender, System.EventArgs e )
		{
			YAF.Classes.Utils.YafBuildLink.Redirect( YAF.Classes.Utils.ForumPages.posts, "m={0}#{0}", Request.QueryString ["m"] );
		}

		private void List_ItemCommand( object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e )
		{
			switch ( e.CommandName )
			{
				case "delete":
					YAF.Classes.Data.DB.attachment_delete( e.CommandArgument );
					BindData();
					break;
			}
		}

		private void Upload_Click( object sender, System.EventArgs e )
		{
			try
			{
				if ( CheckValidFile( File ) )
				{
					SaveAttachment( Request.QueryString ["m"], File );
				}
				BindData();
			}
			catch ( Exception x )
			{
				YAF.Classes.Data.DB.eventlog_create( PageContext.PageUserID, this, x );
				PageContext.AddLoadMessage( x.Message );
				return;
			}
		}

        // Modified by MJ Hufford - 10/08/2007
		private bool CheckValidFile( HtmlInputFile uploadedFile )
		{
			string filePath = uploadedFile.PostedFile.FileName.Trim();

			if ( String.IsNullOrEmpty( filePath ) || uploadedFile.PostedFile.ContentLength == 0 )
			{
				return false;
			}

			string extension = System.IO.Path.GetExtension( filePath ).ToLower();
			// remove the "period"
			extension = extension.Replace( ".", "" );

			// If we don't get a match from the db, then the extension is not allowed
      DataTable dt = YAF.Classes.Data.DB.extension_list(PageContext.PageBoardID, extension);

			bool bInList = ( dt.Rows.Count > 0 );
			bool bError = false;

			if ( PageContext.BoardSettings.FileExtensionAreAllowed && !bInList )
			{
				// since it's not in the list -- it's invalid
				bError = true;
			}
			else if ( !PageContext.BoardSettings.FileExtensionAreAllowed && bInList )
			{
				// since it's on the list -- it's invalid
				bError = true;
			}

			if ( bError )
			{
				// just throw an error that this file is invalid...
				PageContext.AddLoadMessage( String.Format( GetText( "FILEERROR" ), extension ) );
				return false;
			}

			return true;
		}

		private void SaveAttachment( object messageID, HtmlInputFile file )
		{
			if ( file.PostedFile == null || file.PostedFile.FileName.Trim().Length == 0 || file.PostedFile.ContentLength == 0 )
				return;

			string sUpDir = Request.MapPath( YAF.Classes.Config.UploadDir );
			string filename = file.PostedFile.FileName;

			int pos = filename.LastIndexOfAny( new char [] { '/', '\\' } );
			if ( pos >= 0 ) filename = filename.Substring( pos + 1 );

			// filename can be only 255 characters long (due to table column)
			if (filename.Length > 255) filename = filename.Substring(filename.Length - 255);

			// verify the size of the attachment
			if ( PageContext.BoardSettings.MaxFileSize > 0 && file.PostedFile.ContentLength > PageContext.BoardSettings.MaxFileSize )
				throw new Exception( GetText( "ERROR_TOOBIG" ) );

			if ( PageContext.BoardSettings.UseFileTable )
			{
				YAF.Classes.Data.DB.attachment_save( messageID, filename, file.PostedFile.ContentLength, file.PostedFile.ContentType, file.PostedFile.InputStream );
			}
			else
			{
				file.PostedFile.SaveAs( String.Format( "{0}{1}.{2}", sUpDir, messageID, filename ) );
				YAF.Classes.Data.DB.attachment_save( messageID, filename, file.PostedFile.ContentLength, file.PostedFile.ContentType, null );
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit( EventArgs e )
		{
			Back.Click += new EventHandler( Back_Click );
			Upload.Click += new EventHandler( Upload_Click );
			List.ItemCommand += new RepeaterCommandEventHandler( List_ItemCommand );
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit( e );
		}

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
}
}
