/* Yet Another Forum.NET
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
namespace YAF.Classes.Core
{
  using System.Web.UI;

  /// <summary>
  /// Yaf Header Interface
  /// </summary>
  public interface IYafHeader
  {
    /// <summary>
    /// Gets or sets a value indicating whether SimpleRender.
    /// </summary>
    bool SimpleRender
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets RefreshURL.
    /// </summary>
    string RefreshURL
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets RefreshTime.
    /// </summary>
    int RefreshTime
    {
      get;
      set;
    }

    /// <summary>
    /// Gets ThisControl.
    /// </summary>
    Control ThisControl
    {
      get;
    }
  }
}