﻿namespace YAF.DotNetNuke
{
  #region Using

  using System;
  using System.Data;
  using System.IO;
  using System.Web;
  using System.Web.Security;

  using global::DotNetNuke.Common.Utilities;
  using global::DotNetNuke.Entities.Users;
  using global::DotNetNuke.Services.Exceptions;
  using global::DotNetNuke.Services.Scheduling;

  using YAF.Classes.Core;
  using YAF.Classes.Data;
  using YAF.Classes.Utils;

  #endregion

  /// <summary>
  /// The yaf dnn import scheduler.
  /// </summary>
  public class YafDnnImportScheduler : SchedulerClient
  {
    #region Constants and Fields

    /// <summary>
    /// The s info.
    /// </summary>
    private string sInfo = string.Empty;

    #endregion

    // private readonly PortalSettings _portalSettings;
    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="YafDnnImportScheduler"/> class.
    /// </summary>
    /// <param name="objScheduleHistoryItem">
    /// The obj schedule history item.
    /// </param>
    public YafDnnImportScheduler(ScheduleHistoryItem objScheduleHistoryItem)
    {
      this.ScheduleHistoryItem = objScheduleHistoryItem;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// The do work.
    /// </summary>
    public override void DoWork()
    {
      try
      {
        this.GetSettings();

        // report success to the scheduler framework
        this.ScheduleHistoryItem.Succeeded = true;

        this.ScheduleHistoryItem.AddLogNote(this.sInfo);
      }
      catch (Exception exc)
      {
        this.ScheduleHistoryItem.Succeeded = false;
        this.ScheduleHistoryItem.AddLogNote("EXCEPTION: " + exc);
        this.Errored(ref exc);
        Exceptions.LogException(exc);
      }
    }

    #endregion

    #region Methods

    /// <summary>
    /// The create yaf host user.
    /// </summary>
    /// <param name="userId">
    /// The yaf user id.
    /// </param>
    /// <param name="boardId">
    /// The iboard id.
    /// </param>
    private static void CreateYafHostUser(int userId, int boardId)
    {
      // get this user information...
      DataTable userInfo = DB.user_list(boardId, userId, null, null, null);

      if (userInfo.Rows.Count <= 0)
      {
        return;
      }

      DataRow row = userInfo.Rows[0];

      if (Convert.ToBoolean(row["IsHostAdmin"]))
      {
        return;
      }

      // fix the ishostadmin flag...
      var userFlags = new UserFlags(row["Flags"]) { IsHostAdmin = true };

      // update...
      DB.user_adminsave(
        boardId, userId, row["Name"], row["DisplayName"], row["Email"], userFlags.BitValue, row["RankID"]);
    }

    /// <summary>
    /// The create yaf user.
    /// </summary>
    /// <param name="dnnUserInfo">
    /// The dnn user info.
    /// </param>
    /// <param name="dnnUser">
    /// The dnn user.
    /// </param>
    /// <param name="boardId">
    /// The i board id.
    /// </param>
    /// <returns>
    /// The create yaf user.
    /// </returns>
    private static int CreateYafUser(UserInfo dnnUserInfo, MembershipUser dnnUser, int boardId)
    {
      YafContext.Current.Cache.Clear();

      // setup roles
      RoleMembershipHelper.SetupUserRoles(boardId, dnnUser.UserName);

      // create the user in the YAF DB so profile can ge created...
      int? userId = RoleMembershipHelper.CreateForumUser(dnnUser, boardId);

      if (userId == null)
      {
        return 0;
      }

      // create profile
      YafUserProfile userProfile = YafUserProfile.GetProfile(dnnUser.UserName);

      // setup their inital profile information
      userProfile.Initialize(dnnUser.UserName, true);
      userProfile.RealName = dnnUserInfo.Profile.FullName;
      userProfile.Location = dnnUserInfo.Profile.Country;
      userProfile.Homepage = dnnUserInfo.Profile.Website;
      userProfile.Save();

      int yafUserId = UserMembershipHelper.GetUserIDFromProviderUserKey(dnnUser.ProviderUserKey);

      // Save User
      DB.user_save(
        yafUserId, 
        boardId, 
        null, 
        null, 
        null, 
        GetUserTimeZoneOffset(dnnUserInfo), 
        null, 
        null, 
        null, 
        null, 
        null, 
        null, 
        null, 
        null, 
        null, 
        null);

      return yafUserId;
    }

    /// <summary>
    /// The get user time zone offset.
    /// </summary>
    /// <param name="userInfo">
    /// The user info.
    /// </param>
    /// <returns>
    /// The get user time zone offset.
    /// </returns>
    private static int GetUserTimeZoneOffset(UserInfo userInfo)
    {
      int timeZone;
      if ((userInfo != null) && (userInfo.UserID != Null.NullInteger))
      {
        timeZone = userInfo.Profile.TimeZone;
      }
      else
      {
        timeZone = -480;
      }

      return timeZone;
    }

    /// <summary>
    /// The mark roles changed.
    /// </summary>
    private static void MarkRolesChanged()
    {
      RolePrincipal rolePrincipal;
      if (Roles.CacheRolesInCookie)
      {
        string roleCookie = string.Empty;

        HttpCookie cookie = HttpContext.Current.Request.Cookies[Roles.CookieName];
        if (cookie != null)
        {
          roleCookie = cookie.Value;
        }

        rolePrincipal = new RolePrincipal(HttpContext.Current.User.Identity, roleCookie);
      }
      else
      {
        rolePrincipal = new RolePrincipal(HttpContext.Current.User.Identity);
      }

      rolePrincipal.SetDirty();
    }

    /// <summary>
    /// The get settings.
    /// </summary>
    private void GetSettings()
    {
      var dsSettings = new DataSet();

      string sFile = string.Format("{0}App_Data/YafImports.xml", HttpRuntime.AppDomainAppPath);

      try
      {
        dsSettings.ReadXml(sFile);
      }
      catch (Exception)
      {
        var file = new FileStream(sFile, FileMode.Create);
        var sw = new StreamWriter(file);

        sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
        sw.WriteLine("<YafImports>");
        sw.WriteLine("<Import PortalId=\"0\" BoardId=\"1\"/>");
        sw.WriteLine("</YafImports>");

        sw.Close();
        file.Close();

        dsSettings.ReadXml(sFile);
      }

      foreach (DataRow dataRow in dsSettings.Tables[0].Rows)
      {
        int iPortalId = Convert.ToInt32(dataRow["PortalId"]);
        int iBoardId = Convert.ToInt32(dataRow["BoardId"]);

        this.ImportUsers(iBoardId, iPortalId);
      }
    }

    /// <summary>
    /// The import users.
    /// </summary>
    /// <param name="board">
    /// The i board.
    /// </param>
    /// <param name="portal">
    /// The i portal.
    /// </param>
    private void ImportUsers(int board, int portal)
    {
      int iNewUsers = 0;
      bool bRolesChanged = false;
      try
      {
        foreach (UserInfo dnnUserInfo in UserController.GetUsers(portal))
        {
          MembershipUser dnnUser = Membership.GetUser(dnnUserInfo.Username, true);

          if (dnnUser == null)
          {
            return;
          }
          
          bool roleChanged = false;
          foreach (string role in dnnUserInfo.Roles)
          {
            if (!Roles.RoleExists(role))
            {
              Roles.CreateRole(role);
              roleChanged = true;
            }

            if (!Roles.IsUserInRole(dnnUserInfo.Username, role))
            {
              Roles.AddUserToRole(dnnUserInfo.Username, role);
            }
          }

          // Sync Roles
          if (roleChanged)
          {
            bRolesChanged = true;

            MarkRolesChanged();
          }

          int yafUserId;

          try
          {
            yafUserId = DB.user_get(board, dnnUser.ProviderUserKey);
          }
          catch (Exception)
          {
            yafUserId = CreateYafUser(dnnUserInfo, dnnUser, board);
            iNewUsers++;
          }

          // Create user if Not Exist

          // super admin check...
          if (dnnUserInfo.IsSuperUser)
          {
            CreateYafHostUser(yafUserId, board);
          }

          if (YafContext.Current.Settings != null)
          {
            RoleMembershipHelper.UpdateForumUser(dnnUser, YafContext.Current.Settings.BoardID);
          }
        }

        this.sInfo = string.Format("{0} User(s) Imported", iNewUsers);

        if (bRolesChanged)
        {
          this.sInfo += ", but all Roles are syncronized!";
        }
        else
        {
          this.sInfo += ", Roles already syncronized!";
        }

        YafContext.Current.Cache.Clear();

        DataCache.ClearCache();

        // Session.Clear();
      }
      catch (Exception ex)
      {
        Exceptions.LogException(ex);
      }
    }

    #endregion
  }
}