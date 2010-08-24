﻿/* Yet Another Forum.net
 * Copyright (C) 2006-2010 Jaben Cargman
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
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.Caching;
using YAF.Classes.Core;
using YAF.Classes.Data;
using YAF.Classes.Utils;

namespace YAF.Classes
{
  #region Using

  using System.Web;

  #endregion

  /// <summary>
  /// The dot net nuke url builder.
  /// </summary>
  public class DotNetNukeUrlBuilder : BaseUrlBuilder
  {
    #region Constants and Fields

      /// <summary>
      /// The cache size.
      /// </summary>
      private int _cacheSize = 500;

      #endregion

      #region Properties

      /// <summary>
      /// Gets or sets CacheSize.
      /// </summary>
      protected int CacheSize
      {
          get
          {
              return _cacheSize;
          }

          set
          {
              if (_cacheSize > 0)
              {
                  _cacheSize = value;
              }
          }
      }

      #endregion

      #region Public Methods

      /// <summary>
      /// The build url.
      /// </summary>
      /// <param name="sUrl">
      /// The url.
      /// </param>
      /// <returns>
      /// The build url.
      /// </returns>
      public override string BuildUrl(string sUrl)
      {
          string sNewURL;

          if (!Config.EnableURLRewriting)
          {
              string sScriptName = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
              string iTabid = HttpContext.Current.Request.QueryString.GetFirstOrDefault("tabid");

              // escape & to &amp;
              sUrl = sUrl.Replace("&", "&amp;");

              return iTabid != null
                         ? string.Format("{0}?tabid={1}&{2}", sScriptName, iTabid, sUrl)
                         : string.Format("{0}?{1}", sScriptName, sUrl);
          }

          try
          {
              //string sTabid = HttpContext.Current.Request.QueryString.GetFirstOrDefault("tabid");

               string sTabid =  HttpContext.Current.Request.QueryString["tabid"];

              sNewURL = ResolveUrl(string.Format("~/tabid/{0}/", sTabid));

             
              

              var parser = new SimpleURLParameterParser(sUrl);

              string useKey = string.Empty;


              switch (parser["g"])
              {
                  case "topics":
                      {
                          useKey = "f";

                          sNewURL += string.Format("g/{2}/f/{0}/{1}", parser[useKey],
                                                   GetForumName(Convert.ToInt32(parser[useKey])), parser["g"]);
                      }
                      break;
                  case "posts":
                      {
                          if (!String.IsNullOrEmpty(parser["t"]))
                          {
                              useKey = "t";

                              sNewURL += string.Format("g/{2}/t/{0}/{1}", parser["t"],
                                                       GetTopicName(Convert.ToInt32(parser["t"])), parser["g"]);

                          }
                          else if (!String.IsNullOrEmpty(parser["m"]))
                          {

                              useKey = "m";

                              sNewURL += string.Format("g/{2}/m/{0}/{1}", parser["m"],
                                                       GetTopicNameFromMessage(Convert.ToInt32(parser["m"])), parser["g"]);
                          }
                      }
                      break;
                  case "profile":
                      {
                          useKey = "u";

                          sNewURL += string.Format("g/{2}/u/{0}/{1}", parser[useKey],
                                                   GetProfileName(Convert.ToInt32(parser[useKey])), parser["g"]);
                      }
                      break;
                  case "forum":
                      {
                          if (!String.IsNullOrEmpty(parser["c"]))
                          {
                              useKey = "c";

                              sNewURL += string.Format("g/{2}/c/{0}/{1}", parser[useKey],
                                                       GetCategoryName(Convert.ToInt32(parser[useKey])), parser["g"]);
                          }
                          else
                          {
                              sNewURL += YafContext.Current.BoardSettings.Name;
                          }

                      }
                      break;
                  case "activeusers":
                      {
                          useKey = "v";

                          sNewURL += string.Format("g/{1}/v/{0}/{1}", parser[useKey], parser["g"]);

                      }
                      break;
                  case "admin_editforum":
                      {
                          useKey = "f";

                          if (parser[useKey] != null)
                          {
                              sNewURL += string.Format("g/{1}/f/{0}/{1}", parser[useKey], parser["g"]);
                          }
                          else
                          {
                              sNewURL += string.Format("g/{0}/{0}", parser["g"]);
                          }

                      }
                      break;
                      
                  default:
                      {
                          sNewURL += string.Format("g/{0}/{0}", parser["g"]);

                      }
                      break;

              }

              sNewURL += ".aspx";

              string restURL = parser.CreateQueryString(new[] {"g", useKey});

              // append to the url if there are additional (unsupported) parameters
              if (restURL.Length > 0)
              {
                  sNewURL += "?" + restURL;
              }

              // add anchor
              if (parser.HasAnchor)
              {
                  sNewURL += "#" + parser.Anchor;
              }

              // just make sure & is &amp; ...
              sNewURL = sNewURL.Replace("&amp;", "&").Replace("&", "&amp;");
              

          }
          catch (Exception)
          {
              string scriptname = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"];
              string sTabId = HttpContext.Current.Request.QueryString.GetFirstOrDefault("tabid");

              return sTabId != null
                         ? string.Format("{0}?tabid={1}&{2}", scriptname, sTabId, sUrl)
                         : string.Format("{0}?{1}", scriptname, sUrl);
          }

          return sNewURL;
      }

      #endregion

      #region Methods

      ///<summary>
      /// Get Domain Name
      ///</summary>
      ///<param name="url"></param>
      ///<returns></returns>
      public string ResolveUrl(string url)
      {

          // String is Empty, just return Url
          if (url.Length == 0)
          {
              return url;
          }

          // String does not contain a ~, so just return Url
          if (url.StartsWith("~") == false)
          {
              return url;
          }

          // There is just the ~ in the Url, return the appPath
          if (url.Length == 1)
          {
              return HttpContext.Current.Request.ApplicationPath;
          }

          if (url.ToCharArray()[1] == '/' || url.ToCharArray()[1] == '\\')
          {
              // Url looks like ~/ or ~\
              if (HttpContext.Current.Request.ApplicationPath.Length > 1)
              {
                  return HttpContext.Current.Request.ApplicationPath + "/" + url.Substring(2);
              }

              return "/" + url.Substring(2);
          }

          // Url look like ~something
          if (HttpContext.Current.Request.ApplicationPath.Length > 1)
          {
              return HttpContext.Current.Request.ApplicationPath + "/" + url.Substring(1);
          }

          return HttpContext.Current.Request.ApplicationPath + url.Substring(1);
      }
      /// <summary>
      /// The high range.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The high range.
      /// </returns>
      protected int HighRange(int id)
      {
          return (int)(Math.Ceiling((double)(id / _cacheSize)) * _cacheSize);
      }

      /// <summary>
      /// The low range.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The low range.
      /// </returns>
      protected int LowRange(int id)
      {
          return (int)(Math.Floor((double)(id / _cacheSize)) * _cacheSize);
      }

      /// <summary>
      /// The clean string for url.
      /// </summary>
      /// <param name="str">
      /// The str.
      /// </param>
      /// <returns>
      /// The clean string for url.
      /// </returns>
      private static string CleanStringForURL(string str)
      {
          var sb = new StringBuilder();

          // trim...
          str = HttpContext.Current.Server.HtmlDecode(str.Trim());

          // fix ampersand...
          str = str.Replace("&", "and");

          // normalize the Unicode
          str = str.Normalize(NormalizationForm.FormD);

          foreach (char currentChar in str)
          {
              if (char.IsWhiteSpace(currentChar) || currentChar == '.')
              {
                  sb.Append('-');
              }
              else if (char.GetUnicodeCategory(currentChar) != UnicodeCategory.NonSpacingMark && !char.IsPunctuation(currentChar) &&
                       !char.IsSymbol(currentChar) && currentChar < 128)
              {
                  sb.Append(currentChar);
              }
          }

          return sb.ToString();
      }
      /// <summary>
      /// The get profile name.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get profile name.
      /// </returns>
      private string GetProfileName(int id)
      {
          const string type = "Profile";
          const string primaryKey = "UserID";
          const string nameField = "Name";

          DataRow row = GetDataRowFromCache(type, id);

          if (row == null)
          {
              // get the section desired...
              DataTable list = DB.user_simplelist(LowRange(id), CacheSize);

              // set it up in the cache
              row = SetupDataToCache(ref list, type, id, primaryKey);

              if (row == null)
              {
                  return string.Empty;
              }
          }

          return CleanStringForURL(row[nameField].ToString());
      }
      /// <summary>
      /// The get cache name.
      /// </summary>
      /// <param name="type">
      /// The type.
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get cache name.
      /// </returns>
      private string GetCacheName(string type, int id)
      {
          return String.Format(@"urlRewritingDT-{0}-Range-{1}-to-{2}", type, HighRange(id), LowRange(id));
      }

      /// <summary>
      /// The get category name.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get category name.
      /// </returns>
      private string GetCategoryName(int id)
      {
          const string type = "Category";
          const string primaryKey = "CategoryID";
          const string nameField = "Name";

          DataRow row = GetDataRowFromCache(type, id);

          if (row == null)
          {
              // get the section desired...
              DataTable list = DB.category_simplelist(LowRange(id), CacheSize);

              // set it up in the cache
              row = SetupDataToCache(ref list, type, id, primaryKey);

              if (row == null)
              {
                  return string.Empty;
              }
          }

          return CleanStringForURL(row[nameField].ToString());
      }

      /// <summary>
      /// The get data row from cache.
      /// </summary>
      /// <param name="type">
      /// The type.
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// </returns>
      private DataRow GetDataRowFromCache(string type, int id)
      {
          // get the datatable and find the value
          var list = HttpContext.Current.Cache[GetCacheName(type, id)] as DataTable;

          if (list != null)
          {
              DataRow row = list.Rows.Find(id);

              // valid, return...
              if (row != null)
              {
                  return row;
              }
              // invalidate this cache section
              HttpContext.Current.Cache.Remove(GetCacheName(type, id));
          }

          return null;
      }

      /// <summary>
      /// The get forum name.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get forum name.
      /// </returns>
      private string GetForumName(int id)
      {
          const string type = "Forum";
          const string primaryKey = "ForumID";
          const string nameField = "Name";

          DataRow row = GetDataRowFromCache(type, id);

          if (row == null)
          {
              // get the section desired...
              DataTable list = DB.forum_simplelist(LowRange(id), CacheSize);

              // set it up in the cache
              row = SetupDataToCache(ref list, type, id, primaryKey);

              if (row == null)
              {
                  return string.Empty;
              }
          }

          return CleanStringForURL(row[nameField].ToString());
      }

      /// <summary>
      /// The get topic name.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get topic name.
      /// </returns>
      private string GetTopicName(int id)
      {
          const string type = "Topic";
          const string primaryKey = "TopicID";
          const string nameField = "Topic";

          DataRow row = GetDataRowFromCache(type, id);

          if (row == null)
          {
              // get the section desired...
              DataTable list = DB.topic_simplelist(LowRange(id), CacheSize);

              // set it up in the cache
              row = SetupDataToCache(ref list, type, id, primaryKey);

              if (row == null)
              {
                  return string.Empty;
              }
          }

          return CleanStringForURL(row[nameField].ToString());
      }

      /// <summary>
      /// The get topic name from message.
      /// </summary>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <returns>
      /// The get topic name from message.
      /// </returns>
      private string GetTopicNameFromMessage(int id)
      {
          const string type = "Message";
          const string primaryKey = "MessageID";

          DataRow row = GetDataRowFromCache(type, id);

          if (row == null)
          {
              // get the section desired...
              DataTable list = DB.message_simplelist(LowRange(id), CacheSize);

              // set it up in the cache
              row = SetupDataToCache(ref list, type, id, primaryKey);

              if (row == null)
              {
                  return string.Empty;
              }
          }

          return GetTopicName(Convert.ToInt32(row["TopicID"]));
      }

      /// <summary>
      /// The setup data to cache.
      /// </summary>
      /// <param name="list">
      /// The list.
      /// </param>
      /// <param name="type">
      /// The type.
      /// </param>
      /// <param name="id">
      /// The id.
      /// </param>
      /// <param name="primaryKey">
      /// The primary key.
      /// </param>
      /// <returns>
      /// </returns>
      private DataRow SetupDataToCache(ref DataTable list, string type, int id, string primaryKey)
      {
          DataRow row = null;

          if (list != null)
          {
              list.Columns[primaryKey].Unique = true;
              list.PrimaryKey = new[] { list.Columns[primaryKey] };

              // store it for the future
              var randomValue = new Random();
              HttpContext.Current.Cache.Insert(
                GetCacheName(type, id),
                list,
                null,
                DateTime.UtcNow.AddMinutes(randomValue.Next(5, 15)),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Low,
                null);

              // find and return profile..
              row = list.Rows.Find(id);

              if (row == null)
              {
                  // invalidate this cache section
                  HttpContext.Current.Cache.Remove(GetCacheName(type, id));
              }
          }

          return row;
      }

      #endregion
  }
}