/* Yet Another Forum.net
 * Copyright (C) 2006-2008 Jaben Cargman
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
using System.Collections.Generic;
using System.Text;

namespace YAF.Classes.Utils
{
	/// <summary>
	/// For globally or multiple times used constants
	/// </summary>
	public static class Constants
	{
		/// <summary>
		/// Constants for UserBox templating
		/// </summary>
		public struct UserBox
		{
			public const string DisplayTemplateDefault = "<yaf:avatar /><yaf:badges /><yaf:rankimage /><yaf:rank /><yaf:groups /><br /><yaf:joindate /><yaf:posts /><yaf:points /><yaf:location />";
			
			public const string Avatar = @"<yaf:avatar\s*/>";
			public const string Badges = @"<yaf:badges\s*/>";
			public const string RankImage = @"<yaf:rankimage\s*/>";
			public const string Rank = @"<yaf:rank\s*/>";
			public const string Groups = @"<yaf:groups\s*/>";
			public const string JoinDate = @"<yaf:joindate\s*/>";
			public const string Posts = @"<yaf:posts\s*/>";
			public const string Points = @"<yaf:points\s*/>";
			public const string Location = @"<yaf:location\s*/>";
		}

		/// <summary>
		/// Cache key constants
		/// </summary>
		public struct Cache
		{
			public const string BannedIP = "BannedIP";
			public const string BoardStats = "BoardStats";
			public const string BoardSettings = "BoardSettings";
			public const string GuestUserID = "GuestUserID";
			public const string CustomBBCode = "CustomBBCode";
			public const string Smilies = "Smilies";
			public const string ActiveUsers = "ActiveUsers";
			public const string ActiveDiscussions = "ActiveDiscussions";
			public const string ForumActiveDiscussions = "ForumActiveDiscussions";
			public const string ForumModerators = "ForumModerators";
			public const string ForumCategory = "ForumCategory";
			public const string UserBoxes = "UserBoxes";
		}
	}
}
