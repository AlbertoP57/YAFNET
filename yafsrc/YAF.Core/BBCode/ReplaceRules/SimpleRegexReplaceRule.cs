/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2019 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Core.BBCode.ReplaceRules
{
    using System.Text;
    using System.Text.RegularExpressions;

    using YAF.Core.Extensions;
    using YAF.Types.Interfaces;

    /// <summary>
  /// For basic regex with no variables
  /// </summary>
  public class SimpleRegexReplaceRule : BaseReplaceRule
  {
    #region Constants and Fields

    /// <summary>
    ///   The _reg ex replace.
    /// </summary>
    protected string _regExReplace;

    /// <summary>
    ///   The _reg ex search.
    /// </summary>
    protected Regex _regExSearch;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleRegexReplaceRule"/> class.
    /// </summary>
    /// <param name="regExSearch">
    /// The reg ex search.
    /// </param>
    /// <param name="regExReplace">
    /// The reg ex replace.
    /// </param>
    /// <param name="regExOptions">
    /// The reg ex options.
    /// </param>
    public SimpleRegexReplaceRule(string regExSearch, string regExReplace, RegexOptions regExOptions)
    {
      this._regExSearch = new Regex(regExSearch, regExOptions);
      this._regExReplace = regExReplace;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleRegexReplaceRule"/> class.
    /// </summary>
    /// <param name="regExSearch">
    /// The reg ex search.
    /// </param>
    /// <param name="regExReplace">
    /// The reg ex replace.
    /// </param>
    public SimpleRegexReplaceRule(Regex regExSearch, string regExReplace)
    {
      this._regExSearch = regExSearch;
      this._regExReplace = regExReplace;
    }

    #endregion

    #region Properties

    /// <summary>
    ///   Gets RuleDescription.
    /// </summary>
    public override string RuleDescription => $"RegExSearch = \"{this._regExSearch}\"";

    #endregion

    #region Public Methods

    /// <summary>
    /// The replace.
    /// </summary>
    /// <param name="text">
    /// The text.
    /// </param>
    /// <param name="replacement">
    /// The replacement.
    /// </param>
    public override void Replace(ref string text, IReplaceBlocks replacement)
    {
      var sb = new StringBuilder(text);

      var m = this._regExSearch.Match(text);
      while (m.Success)
      {
        var replaceString = this._regExReplace.Replace("${inner}", this.GetInnerValue(m.Groups["inner"].Value));

        // pulls the htmls into the replacement collection before it's inserted back into the main text
        replacement.ReplaceHtmlFromText(ref replaceString);

        // remove old bbcode...
        sb.Remove(m.Groups[0].Index, m.Groups[0].Length);

        // insert replaced value(s)
        sb.Insert(m.Groups[0].Index, replaceString);

        // text = text.Substring( 0, m.Groups [0].Index ) + tStr + text.Substring( m.Groups [0].Index + m.Groups [0].Length );
        m = this._regExSearch.Match(sb.ToString());
      }

      text = sb.ToString();
    }

    #endregion

    #region Methods

    /// <summary>
    /// The get inner value.
    /// </summary>
    /// <param name="innerValue">
    /// The inner value.
    /// </param>
    /// <returns>
    /// The get inner value.
    /// </returns>
    protected virtual string GetInnerValue(string innerValue)
    {
      return innerValue;
    }

    #endregion
  }
}