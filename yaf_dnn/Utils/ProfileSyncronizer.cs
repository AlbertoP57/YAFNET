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

namespace YAF.DotNetNuke.Utils
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Web.Security;
    using global::DotNetNuke.Common;
    using global::DotNetNuke.Common.Utilities;
    using global::DotNetNuke.Entities.Modules;
    using global::DotNetNuke.Entities.Portals;
    using global::DotNetNuke.Entities.Profile;
    using global::DotNetNuke.Entities.Users;
    using YAF.Classes;
    using YAF.Classes.Data;
    using YAF.Core;
    using YAF.DotNetNuke.Controller;
    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.EventProxies;
    using YAF.Types.Interfaces;
    using YAF.Utils;

    /// <summary>
    /// YAF DNN Profile Syncronization 
    /// </summary>
    public class ProfileSyncronizer : PortalModuleBase
    {
        /// <summary>
        /// Syncronizes The Yaf Profile with Dnn Profile or reverse if
        /// one profile is newer then the other
        /// </summary>
        /// <param name="yafUserId">The Yaf UserId</param>
        /// <param name="dnnUserInfo">Dnn UserInfo of current User</param>
        /// <param name="membershipUser">MemberShip of current User</param>
        /// <param name="portalID">The portal ID.</param>
        /// <param name="portalGuid">The portal GUID.</param>
        /// <param name="boardId">The board Id.</param>
        public static void UpdateUserProfile(int yafUserId, UserInfo dnnUserInfo, MembershipUser membershipUser, int portalID, Guid portalGuid, int boardId)
        {
            try
            {
                var yafUserProfile = YafUserProfile.GetProfile(membershipUser.UserName);

                ////yafUserProfile.IsDirty

                ////dnnUserInfo.Profile.IsDirty

                var yafTime = yafUserProfile.LastUpdatedDate;
                var dnnTime = Profile.YafDnnGetLastUpdatedProfile(dnnUserInfo.UserID);

                TimeSpan timeCompare = dnnTime - yafTime;

                if (timeCompare.TotalSeconds > 0)
                {
                    SyncYaf(dnnTime, yafUserId, yafUserProfile, dnnUserInfo, portalID, portalGuid, boardId);
                }
                else
                {
                    SyncDnn(yafTime, yafUserId, yafUserProfile, dnnUserInfo, membershipUser, portalID);
                }
            }
            catch (Exception)
            {
                /*EventLogController objEventLog = new EventLogController();
                objEventLog.AddLog();*/
            }
        }

        /// <summary>
        /// The get user time zone offset.
        /// </summary>
        /// <param name="userInfo">
        /// The user info.
        /// </param>
        /// <param name="portalSettings">
        /// Current Portal Settings
        /// </param>
        /// <returns>
        /// Returns the User Time Zone Offset Value
        /// </returns>
        public static int GetUserTimeZoneOffset(UserInfo userInfo, PortalSettings portalSettings)
        {
            int timeZone;

            if ((userInfo != null) && (userInfo.UserID != Null.NullInteger))
            {
                timeZone = userInfo.Profile.TimeZone;
            }
            else
            {
                if (portalSettings != null)
                {
                    timeZone = portalSettings.TimeZoneOffset;
                }
                else
                {
                    timeZone = -480;
                }
            }

            return timeZone;
        }

        /// <summary>
        /// yaf profile is newer sync dnn now
        /// </summary>
        /// <param name="dnnTime">
        /// The dnn Time.
        /// </param>
        /// <param name="yafUserId">
        /// The yaf user id.
        /// </param>
        /// <param name="yafUserProfile">
        /// The yaf user profile.
        /// </param>
        /// <param name="dnnUserInfo">
        /// The dnn user info.
        /// </param>
        /// <param name="membershipUser">
        /// The membership user.
        /// </param>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        private static void SyncDnn(DateTime dnnTime, int yafUserId, YafUserProfile yafUserProfile, UserInfo dnnUserInfo, MembershipUser membershipUser, int portalId)
        {
            var cacheKeyDnnName = "dnnsync_userid{0}_portalid{1}".FormatWith(dnnUserInfo.UserID, portalId);

            DateTime cacheTime = dnnTime;

            // Make sure its syncs only when needed
            try
            {
                if (DataCache.GetCache(cacheKeyDnnName) != null)
                {
                    cacheTime = (DateTime)DataCache.GetCache(cacheKeyDnnName);
                }
            }
            catch (Exception)
            {
                cacheTime = dnnTime;
            }
            
            if (dnnTime < cacheTime)
            {
                return;
            }

            var yafUserData = new CombinedUserDataHelper(yafUserId);

            if (!string.IsNullOrEmpty(yafUserProfile.RealName.Trim()))
            {
                if (yafUserProfile.RealName.Contains(" "))
                {
                    // Split Fullname into First and Lastname
                    var firstName = yafUserProfile.RealName.Remove(yafUserProfile.RealName.IndexOf(" "));
                    var lastName = yafUserProfile.RealName.Substring(yafUserProfile.RealName.IndexOf(" ") + 1);

                    dnnUserInfo.Profile.FirstName = firstName;
                    dnnUserInfo.Profile.LastName = lastName;
                }
                else
                {
                    dnnUserInfo.Profile.FirstName = yafUserProfile.RealName;
                }
            }

            dnnUserInfo.DisplayName = yafUserData.DisplayName;

            dnnUserInfo.Profile.Country = new RegionInfo(new CultureInfo(yafUserProfile.Country).LCID).EnglishName;

            dnnUserInfo.Profile.City = yafUserProfile.City;
            ////dnnUserInfo.Profile.Region = yafUserProfile.Region;

            dnnUserInfo.Profile.Website = yafUserProfile.Homepage;
            dnnUserInfo.Email = membershipUser.Email;

            if (!string.IsNullOrEmpty(yafUserData.CultureUser))
            {
                dnnUserInfo.Profile.PreferredLocale = yafUserData.CultureUser;
            }

            /*var userAvatarUrl = YafContext.Current.Get<IAvatars>().GetAvatarUrlForUser(yafUserId);

            if (!string.IsNullOrEmpty(userAvatarUrl))
            {
                dnnUserInfo.Profile.Photo = SaveYafAvatar(userAvatarUrl, dnnUserInfo, portalId);
            }*/

            // Save other Yaf Profile Properties as Custom DNN Profile Properties
            ////dnnUserInfo.Profile.SetProfileProperty("Biography", yafUserProfile.Interests);

            dnnUserInfo.Profile.SetProfileProperty("Birthday", yafUserProfile.Birthday.ToString());
            dnnUserInfo.Profile.SetProfileProperty("Occupation", yafUserProfile.Occupation);
            dnnUserInfo.Profile.SetProfileProperty("Gender", yafUserProfile.Gender.ToString());
            dnnUserInfo.Profile.SetProfileProperty("Blog", yafUserProfile.Blog);
            dnnUserInfo.Profile.SetProfileProperty("MSN", yafUserProfile.MSN);
            dnnUserInfo.Profile.SetProfileProperty("YIM", yafUserProfile.YIM);
            dnnUserInfo.Profile.SetProfileProperty("AIM", yafUserProfile.AIM);
            dnnUserInfo.Profile.SetProfileProperty("ICQ", yafUserProfile.ICQ);
            dnnUserInfo.Profile.SetProfileProperty("Facebook", yafUserProfile.Facebook);
            dnnUserInfo.Profile.SetProfileProperty("Twitter", yafUserProfile.Twitter);
            dnnUserInfo.Profile.SetProfileProperty("Twitter1", yafUserProfile.TwitterId);
            dnnUserInfo.Profile.SetProfileProperty("XMPP", yafUserProfile.XMPP);
            dnnUserInfo.Profile.SetProfileProperty("Skype", yafUserProfile.Skype);

            ProfileController.UpdateUserProfile(dnnUserInfo, dnnUserInfo.Profile.ProfileProperties);

            UserController.UpdateUser(portalId, dnnUserInfo);

            var currentTime = DateTime.Now;

            DataCache.SetCache(cacheKeyDnnName, currentTime);
        }

        /// <summary>
        /// Dnn Profile is newer, sync yaf now
        /// NOTE : no neeed to manually sync Email Adress
        /// </summary>
        /// <param name="yafTime">The yaf Time.</param>
        /// <param name="yafUserId">The yaf user id.</param>
        /// <param name="yafUserProfile">The yaf user profile.</param>
        /// <param name="dnnUserInfo">The dnn user info.</param>
        /// <param name="portalID">The portal ID.</param>
        /// <param name="portalGUID">The portal GUID.</param>
        /// <param name="boardId">The board Id.</param>
        private static void SyncYaf(DateTime yafTime, int yafUserId, YafUserProfile yafUserProfile, UserInfo dnnUserInfo, int portalID, Guid portalGUID, int boardId)
        {
            var cacheKeyYafName = "yafsync_userid{0}_portalid{1}".FormatWith(dnnUserInfo.UserID, portalID);

            DateTime cacheTime = yafTime;

            // Make sure its syncs only when needed
            try
            {
                if (DataCache.GetCache(cacheKeyYafName) != null)
                {
                    cacheTime = (DateTime)DataCache.GetCache(cacheKeyYafName);
                }
            }
            catch (Exception)
            {
                cacheTime = yafTime;
            }

            if (yafTime < cacheTime)
            {
                return;
            }
            
            var yafUserData = new CombinedUserDataHelper(yafUserId);

            YafCultureInfo userCuluture = new YafCultureInfo
            {
                LanguageFile = yafUserData.LanguageFile,
                Culture = yafUserData.CultureUser
            };

            if (!string.IsNullOrEmpty(dnnUserInfo.Profile.PreferredLocale))
            {
                CultureInfo newCulture = new CultureInfo(dnnUserInfo.Profile.PreferredLocale);

                foreach (DataRow row in
                    StaticDataHelper.Cultures().Rows.Cast<DataRow>().Where(
                        row => dnnUserInfo.Profile.PreferredLocale == row["CultureTag"].ToString() || newCulture.TwoLetterISOLanguageName == row["CultureTag"].ToString()))
                {
                    userCuluture.LanguageFile = row["CultureFile"].ToString();
                    userCuluture.Culture = row["CultureTag"].ToString();
                }
            }

            LegacyDb.user_save(
                yafUserId,
                boardId,
                null,
                dnnUserInfo.DisplayName,
                null,
                yafUserData.TimeZone,
                userCuluture.LanguageFile,
                userCuluture.Culture,
                yafUserData.ThemeFile,
                yafUserData.UseSingleSignOn,
                yafUserData.TextEditor,
                yafUserData.UseMobileTheme,
                null,
                null,
                null,
                yafUserData.DSTUser,
                yafUserData.IsActiveExcluded,
                null);

            yafUserProfile.RealName = dnnUserInfo.Profile.FullName;

            yafUserProfile.Country = GetRegionInfoFromCountryName(dnnUserInfo.Profile.Country).TwoLetterISORegionName;
            
            yafUserProfile.City = dnnUserInfo.Profile.City;
            yafUserProfile.Homepage = dnnUserInfo.Profile.Website;

            if (!string.IsNullOrEmpty(dnnUserInfo.Profile.Photo))
            {
                SaveDnnAvatar(dnnUserInfo.Profile.Photo, yafUserId, portalGUID);
            }

            // Save other Yaf Profile Properties as Custom DNN Profile Properties
            try
            {
                ////yafUserProfile.Interests = dnnUserInfo.Profile.GetPropertyValue("Biography");
                yafUserProfile.Birthday = DateTime.Parse(dnnUserInfo.Profile.GetPropertyValue("Birthday"));
                yafUserProfile.Occupation = dnnUserInfo.Profile.GetPropertyValue("Occupation");
                yafUserProfile.Gender = int.Parse(dnnUserInfo.Profile.GetPropertyValue("Gender"));
                yafUserProfile.Blog = dnnUserInfo.Profile.GetPropertyValue("Blog");
                yafUserProfile.MSN = dnnUserInfo.Profile.GetPropertyValue("MSN");
                yafUserProfile.YIM = dnnUserInfo.Profile.GetPropertyValue("YIM");
                yafUserProfile.AIM = dnnUserInfo.Profile.GetPropertyValue("AIM");
                yafUserProfile.ICQ = dnnUserInfo.Profile.GetPropertyValue("ICQ");
                yafUserProfile.Facebook = dnnUserInfo.Profile.GetPropertyValue("Facebook");
                yafUserProfile.Twitter = dnnUserInfo.Profile.GetPropertyValue("Twitter");
                yafUserProfile.TwitterId = dnnUserInfo.Profile.GetPropertyValue("TwitterId");
                yafUserProfile.XMPP = dnnUserInfo.Profile.GetPropertyValue("XMPP");
                yafUserProfile.Skype = dnnUserInfo.Profile.GetPropertyValue("Skype");
            }
            finally 
            {
                yafUserProfile.Save();
            }

            var currentTime = DateTime.Now;

            YafContext.Current.Get<IRaiseEvent>().Raise(new UpdateUserEvent(yafUserId));

            YafContext.Current.Get<IDataCache>().Clear();

            DataCache.SetCache(cacheKeyYafName, currentTime);
        }

        /*
        /// <summary>
        /// Extract YAF Avatar to DNN Photo
        /// </summary>
        /// <param name="avatarUrl">
        /// The avatar url.
        /// </param>
        /// <param name="dnnUserInfo">
        /// The dnn user info.
        /// </param>
        /// <param name="portalId">
        /// The portal id.
        /// </param>
        /// <returns>
        /// File ID of the yaf Avatar inside dnn
        /// </returns>
        private static string SaveYafAvatar(string avatarUrl, UserInfo dnnUserInfo, int portalId)
        {
            try
            {
                var folderPath = FileSystemUtils.GetUserFolderPath(dnnUserInfo.UserID);

                // Make sure the user folder exists
                DnnUtils.DnnFileSystem.AddUserFolder(
                    CurrentPortalSettings,
                    CurrentPortalSettings.HomeDirectoryMapPath,
                    (int)FolderController.StorageLocationTypes.InsecureFileSystem,
                    dnnUserInfo.UserID);

                var yafAvatarName = "YafAvatar{0}.jpg".FormatWith(dnnUserInfo.UserID);

                if (!avatarUrl.StartsWith("http"))
                {
                    avatarUrl = BaseUrlBuilder.BaseUrl + avatarUrl;
                }

                // Download Yaf Avatar
                WebClient wc = new WebClient();

                wc.Headers.Add("Referer", YafBuildLink.GetLink(ForumPages.forum));

                wc.DownloadFile(avatarUrl, Path.Combine(CurrentPortalSettings.HomeDirectoryMapPath, Path.Combine(folderPath, yafAvatarName)));

                FolderInfo folder = new FolderController().GetFolder(portalId, Path.Combine(CurrentPortalSettings.HomeDirectoryMapPath, folderPath), true);

                FileSystemUtils.AddFile(yafAvatarName, portalId, true, folder);

                FileController fileController = new FileController();

                FileInfo info = fileController.GetFile(yafAvatarName, portalId, folderPath);

                return info.FileId.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }*/

        /// <summary>
        /// Gets the name of the region info from country (English Name).
        /// </summary>
        /// <param name="countryEnglishName">Name of the country english.</param>
        /// <returns>The RegionInfo for the Country</returns>
        private static RegionInfo GetRegionInfoFromCountryName([NotNull]string countryEnglishName)
        {
            return
                CultureInfo.GetCultures(CultureTypes.SpecificCultures).Select(ci => new RegionInfo(ci.LCID)).FirstOrDefault(region => region.EnglishName.Equals(countryEnglishName));
        }

        /// <summary>
        /// Save Dnn Avatar as Yaf Remote Avatar with relative Path.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="yafUserId">The yaf user id.</param>
        /// <param name="portalGUID">The portal GUID.</param>
        private static void SaveDnnAvatar(string fileId, int yafUserId, Guid portalGUID)
        {
            var dnnAvatarUrl =
                Globals.ResolveUrl(
                    "~/LinkClick.aspx?fileticket={0}".FormatWith(
                        UrlUtils.EncryptParameter(fileId, portalGUID.ToString())));

            // update
            LegacyDb.user_saveavatar(
                yafUserId,
                BaseUrlBuilder.BaseUrl + dnnAvatarUrl,
                null,
                null);

            // clear the cache for this user...
            YafContext.Current.Get<IRaiseEvent>().Raise(new UpdateUserEvent(yafUserId));
        }
    }
}