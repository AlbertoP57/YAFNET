﻿/* Yet Another Forum.NET
 * Copyright (C) 2006-2012 Jaben Cargman
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

namespace YAF.DotNetNuke
{
    #region Using

    using System;
    using System.Data;
    using System.IO;
    using System.Web;
    using System.Web.Security;

    using global::DotNetNuke.Common.Utilities;

    using global::DotNetNuke.Entities.Portals;

    using global::DotNetNuke.Entities.Users;

    using global::DotNetNuke.Services.Exceptions;

    using global::DotNetNuke.Services.Scheduling;

    using YAF.Classes;
    using YAF.Classes.Data;
    using YAF.Core;
    using YAF.DotNetNuke.Utils;
    using YAF.Types.Extensions;
    using YAF.Types.Flags;
    using YAF.Types.Interfaces;

    #endregion

    /// <summary>
    /// The YAF DNN import scheduler.
    /// </summary>
    public class YafDnnImportScheduler : SchedulerClient
    {
        #region Constants and Fields

        /// <summary>
        /// The info.
        /// </summary>
        private string info = string.Empty;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="YafDnnImportScheduler"/> class.
        /// </summary>
        /// <param name="scheduleHistoryItem">
        /// The schedule history item.
        /// </param>
        public YafDnnImportScheduler(ScheduleHistoryItem scheduleHistoryItem)
        {
            this.ScheduleHistoryItem = scheduleHistoryItem;
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

                this.ScheduleHistoryItem.AddLogNote(this.info);
            }
            catch (Exception exc)
            {
                this.ScheduleHistoryItem.Succeeded = false;
                this.ScheduleHistoryItem.AddLogNote("EXCEPTION: {0}".FormatWith(exc));
                this.Errored(ref exc);

                Exceptions.LogException(exc);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create YAF host user.
        /// </summary>
        /// <param name="userId">
        /// The YAF user id.
        /// </param>
        /// <param name="boardId">
        /// The board id.
        /// </param>
        private static void CreateYafHostUser(int userId, int boardId)
        {
            // get this user information...
            DataTable userInfo = LegacyDb.user_list(boardId, userId, null, null, null);

            if (userInfo.Rows.Count <= 0)
            {
                return;
            }

            DataRow row = userInfo.Rows[0];

            if (Convert.ToBoolean(row["IsHostAdmin"]))
            {
                return;
            }

            // fix the IsHostAdmin flag...
            var userFlags = new UserFlags(row["Flags"]) { IsHostAdmin = true };

            // update...
            LegacyDb.user_adminsave(
                boardId, userId, row["Name"], row["DisplayName"], row["Email"], userFlags.BitValue, row["RankID"]);
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        private void GetSettings()
        {
            var dsSettings = new DataSet();

            string filePath = "{0}App_Data/YafImports.xml".FormatWith(HttpRuntime.AppDomainAppPath);

            try
            {
                dsSettings.ReadXml(filePath);
            }
            catch (Exception)
            {
                var file = new FileStream(filePath, FileMode.Create);
                var sw = new StreamWriter(file);

                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                sw.WriteLine("<YafImports>");
                sw.WriteLine("<Import PortalId=\"0\" BoardId=\"1\"/>");
                sw.WriteLine("</YafImports>");

                sw.Close();
                file.Close();

                dsSettings.ReadXml(filePath);
            }

            foreach (DataRow dataRow in dsSettings.Tables[0].Rows)
            {
                this.ImportUsers(dataRow["BoardId"].ToType<int>(), dataRow["PortalId"].ToType<int>());
            }
        }

        /// <summary>
        /// Imports the users.
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="portalId">The portal id.</param>
        private void ImportUsers(int boardId, int portalId)
        {
            int iNewUsers = 0;

            var users = UserController.GetUsers(portalId);

            users.Sort(new UserComparer());

            // Load PortalSettings
            var portalGUID = new PortalController().GetPortal(portalId).GUID;

            // Load Yaf Board Settings if needed
            var boardSettings = YafContext.Current == null
                                    ? new YafLoadBoardSettings(boardId)
                                    : YafContext.Current.Get<YafBoardSettings>();

            var rolesChanged = false;

            try
            {
                foreach (UserInfo dnnUserInfo in users)
                {
                    var dnnUser = Membership.GetUser(dnnUserInfo.Username, true);

                    if (dnnUser == null)
                    {
                        continue;
                    }

                    if (dnnUserInfo.IsDeleted)
                    {
                        continue;
                    }

                    var yafUserId = LegacyDb.user_get(boardId, dnnUser.ProviderUserKey);

                    if (yafUserId.Equals(0))
                    {
                        // Create user if Not Exist
                        yafUserId = UserImporter.CreateYafUser(dnnUserInfo, dnnUser, boardId, null, boardSettings);
                        iNewUsers++;
                    }
                    else
                    {
                        ProfileSyncronizer.UpdateUserProfile(
                            yafUserId, dnnUserInfo, dnnUser, portalId, portalGUID, boardId);
                    }

                    rolesChanged = RoleSyncronizer.SynchronizeUserRoles(boardId, portalId, yafUserId, dnnUserInfo);

                    // super admin check...
                    if (dnnUserInfo.IsSuperUser)
                    {
                        CreateYafHostUser(yafUserId, boardId);
                    }
                }

                this.info = "{0} User(s) Imported".FormatWith(iNewUsers);

                this.info += rolesChanged
                                  ? ", but all User Roles are synchronized!"
                                  : ", User Roles already synchronized!";

                YafContext.Current.Get<IDataCache>().Clear();

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