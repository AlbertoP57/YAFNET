﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotNetNukeModuleEdit.ascx.cs" company="">
//   
// </copyright>
// <summary>
//   Summary description for DotNetNukeModule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace YAF
{
  #region Using

  using System;
  using System.Data;
  using System.Web.UI;
  using System.Web.UI.WebControls;

  using DotNetNuke.Entities.Modules;

  using YAF.Classes;
  using YAF.Classes.Data;
  using YAF.Classes.Utils;

  #endregion

  /// <summary>
  /// Summary description for DotNetNukeModule.
  /// </summary>
  public partial class DotNetNukeModuleEdit : PortalModuleBase
  {
    // protected DropDownList    BoardID, CategoryID;
    // protected LinkButton    update, cancel, create;
    #region Methods

    /// <summary>
    /// The on init.
    /// </summary>
    /// <param name="e">
    /// The e.
    /// </param>
    protected override void OnInit(EventArgs e)
    {
      this.Load += this.DotNetNukeModuleEdit_Load;
      this.update.Click += this.UpdateClick;
      this.cancel.Click += this.CancelClick;
      this.create.Click += this.CreateClick;
      this.BoardID.SelectedIndexChanged += this.BoardID_SelectedIndexChanged;
      base.OnInit(e);
    }

    /// <summary>
    /// The render.
    /// </summary>
    /// <param name="writer">
    /// The writer.
    /// </param>
    protected override void Render(HtmlTextWriter writer)
    {
      writer.WriteLine("<link rel='stylesheet' type='text/css' href='{0}themes/standard/theme.css'/>", Config.AppRoot);
      base.Render(writer);
    }

    /// <summary>
    /// The bind categories.
    /// </summary>
    private void BindCategories()
    {
      using (DataTable dt = DB.category_list(this.BoardID.SelectedValue, DBNull.Value))
      {
        DataRow row = dt.NewRow();
        row["Name"] = "[All Categories]";
        row["CategoryID"] = DBNull.Value;
        dt.Rows.InsertAt(row, 0);

        this.CategoryID.DataSource = dt;
        this.CategoryID.DataTextField = "Name";
        this.CategoryID.DataValueField = "CategoryID";
        this.CategoryID.DataBind();

        if (this.Settings["forumcategoryid"] != null)
        {
          ListItem item = this.CategoryID.Items.FindByValue(this.Settings["forumcategoryid"].ToString());
          if (item != null)
          {
            item.Selected = true;
          }
        }
      }
    }

    /// <summary>
    /// The board i d_ selected index changed.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void BoardID_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.BindCategories();
    }

    /// <summary>
    /// The cancel click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void CancelClick(object sender, EventArgs e)
    {
      YafBuildLink.Redirect(ForumPages.forum);
    }

    /// <summary>
    /// The create click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void CreateClick(object sender, EventArgs e)
    {
      YafBuildLink.Redirect(ForumPages.admin_editboard);
    }

    /// <summary>
    /// The dot net nuke module edit_ load.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void DotNetNukeModuleEdit_Load(object sender, EventArgs e)
    {
      this.update.Text = "Update";
      this.cancel.Text = "Cancel";
      this.create.Text = "Create New Board";

      this.update.Visible = this.IsEditable;
      this.create.Visible = this.IsEditable;

      if (!this.IsPostBack)
      {
        using (DataTable dt = DB.board_list(DBNull.Value))
        {
          this.BoardID.DataSource = dt;
          this.BoardID.DataTextField = "Name";
          this.BoardID.DataValueField = "BoardID";
          this.BoardID.DataBind();
          if (this.Settings["forumboardid"] != null)
          {
            ListItem item = this.BoardID.Items.FindByValue(this.Settings["forumboardid"].ToString());
            if (item != null)
            {
              item.Selected = true;
            }
          }
        }

        this.BindCategories();
      }
    }

    /// <summary>
    /// The update click.
    /// </summary>
    /// <param name="sender">
    /// The sender.
    /// </param>
    /// <param name="e">
    /// The e.
    /// </param>
    private void UpdateClick(object sender, EventArgs e)
    {
      var objModules = new ModuleController();

      objModules.UpdateModuleSetting(this.ModuleId, "forumboardid", this.BoardID.SelectedValue);
      objModules.UpdateModuleSetting(this.ModuleId, "forumcategoryid", this.CategoryID.SelectedValue);

      YafBuildLink.Redirect(ForumPages.forum);
    }

    #endregion
  }
}