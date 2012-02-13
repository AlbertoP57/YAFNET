/* Yet Another Forum.NET
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

namespace YAF.DotNetNuke.Controller
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    using global::DotNetNuke.Data;

    using YAF.Classes.Data;
    using YAF.Utils;

    #endregion

    /// <summary>
    /// Module Data Controller to Handle SQL Stuff
    /// </summary>
    public class Data
    {
        #region Public Methods

        /// <summary>
        /// Get The Latest Post from SQL
        /// </summary>
        /// <param name="boardId">The Board Id of the Board</param>
        /// <param name="numOfPostsToRetrieve">How many post should been retrieved</param>
        /// <param name="pageUserId">Current Users Id</param>
        /// <param name="useStyledNicks">if set to <c>true</c> [use styled nicks].</param>
        /// <param name="showNoCountPosts">if set to <c>true</c> [show no count posts].</param>
        /// <param name="findLastRead">if set to <c>true</c> [find last read].</param>
        /// <returns>
        /// Data Table of Latest Posts
        /// </returns>
        public static DataTable TopicLatest(
            object boardId,
            object numOfPostsToRetrieve,
            object pageUserId,
            bool useStyledNicks,
            bool showNoCountPosts,
            bool findLastRead = false)
        {
            using (SqlCommand cmd = MsSqlDbAccess.GetCommand("topic_latest"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("BoardID", boardId);
                cmd.Parameters.AddWithValue("NumPosts", numOfPostsToRetrieve);
                cmd.Parameters.AddWithValue("PageUserID", pageUserId);
                cmd.Parameters.AddWithValue("StyledNicks", useStyledNicks);
                cmd.Parameters.AddWithValue("ShowNoCountPosts", showNoCountPosts);
                cmd.Parameters.AddWithValue("FindLastRead", findLastRead);

                return MsSqlDbAccess.Current.GetData(cmd, true);
            }
        }

        /// <summary>
        /// Add active access row for the current user outside of yaf
        /// </summary>
        /// <param name="boardId">The board id.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="isGuest">if set to <c>true</c> [is guest].</param>
        /// <returns>
        /// Data Table of the Active Access User Table
        /// </returns>
        public static DataTable ActiveAccessUser(object boardId, object userId, bool isGuest)
        {
            using (SqlCommand cmd = MsSqlDbAccess.GetCommand("pageaccess"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("BoardID", boardId);
                cmd.Parameters.AddWithValue("UserID", userId);
                cmd.Parameters.AddWithValue("IsGuest", isGuest);
                cmd.Parameters.AddWithValue("UTCTIMESTAMP", DateTime.UtcNow);

                return MsSqlDbAccess.Current.GetData(cmd, true);
            }
        }

        /// <summary>
        /// Get all Messages From The Forum
        /// </summary>
        /// <returns>
        /// Message List
        /// </returns>
        public static List<Messages> YafDnnGetMessages()
        {
            List<Messages> messagesList = new List<Messages>();

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("YafDnn_Messages"))
            {
                while (dr.Read())
                {
                    Messages message = new Messages
                        {
                            Message = Convert.ToString(dr["Message"]),
                            MessageId = dr["MessageID"].ToType<int>(),
                            TopicId = dr["TopicID"].ToType<int>(),
                            Posted = Convert.ToDateTime(dr["Posted"])
                        };

                    messagesList.Add(message);
                }
            }

            return messagesList;
        }

        /// <summary>
        /// Get all Messages From The Forum
        /// </summary>
        /// <returns>
        /// Topics List
        /// </returns>
        public static List<Topics> YafDnnGetTopics()
        {
            List<Topics> topicsList = new List<Topics>();

            using (IDataReader dr = DataProvider.Instance().ExecuteReader("YafDnn_Topics"))
            {
                while (dr.Read())
                {
                    Topics topic = new Topics
                        {
                            TopicName = Convert.ToString(dr["Topic"]),
                            TopicId = dr["TopicID"].ToType<int>(),
                            ForumId = dr["ForumID"].ToType<int>(),
                            Posted = Convert.ToDateTime(dr["Posted"])
                        };

                    topicsList.Add(topic);
                }
            }

            return topicsList;
        }

        #endregion
    }
}