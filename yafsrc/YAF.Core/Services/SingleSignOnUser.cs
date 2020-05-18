/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2020 Ingo Herbote
 * https://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Core.Services
{
    using YAF.Core.Context;
    using YAF.Core.Helpers;
    using YAF.Core.Model;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.EventProxies;
    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Events;
    using YAF.Types.Models;

    /// <summary>
    /// Single Sign On User Class to handle Twitter and Facebook Logins
    /// </summary>
    public class SingleSignOnUser : ISingeSignOnUser, IHaveServiceLocator
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleSignOnUser"/> class.
        /// </summary>
        /// <param name="serviceLocator">
        /// The service locator.
        /// </param>
        public SingleSignOnUser(IServiceLocator serviceLocator)
        {
            this.ServiceLocator = serviceLocator;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ServiceLocator.
        /// </summary>
        public IServiceLocator ServiceLocator { get; set; }

        #endregion

        /// <summary>
        /// Generates the oAUTH callback login URL.
        /// </summary>
        /// <param name="authService">The AUTH service.</param>
        /// <param name="generatePopUpUrl">if set to <c>true</c> [generate pop up URL].</param>
        /// <param name="connectCurrentUser">if set to <c>true</c> [connect current user].</param>
        /// <returns>
        /// Returns the login Url
        /// </returns>
        public string GenerateLoginUrl([NotNull] AuthService authService, [NotNull] bool generatePopUpUrl, [CanBeNull] bool connectCurrentUser = false)
        {
            return authService switch
                {
                   // AuthService.twitter => new Auth.Twitter().GenerateLoginUrl(generatePopUpUrl, connectCurrentUser),
                    //AuthService.facebook => new Auth.Facebook().GenerateLoginUrl(generatePopUpUrl, connectCurrentUser),
                    //AuthService.google => new Auth.Google().GenerateLoginUrl(generatePopUpUrl, connectCurrentUser),
                    _ => string.Empty
                };
        }

        /// <summary>
        /// Do login and set correct flag
        /// </summary>
        /// <param name="authService">The AUTH service.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="doLogin">if set to <c>true</c> [do login].</param>
        public void LoginSuccess([NotNull] AuthService authService, [CanBeNull] string userName, [NotNull] int userID, [NotNull] bool doLogin)
        {
            // Add Flag to User that indicates with what service the user is logged in
            BoardContext.Current.GetRepository<User>().UpdateAuthServiceStatus(userID, authService);

            if (!doLogin)
            {
                return;
            }

            // TODO : Login
            //IdentityHelper.SignIn();

            BoardContext.Current.Get<IRaiseEvent>().Raise(new SuccessfulUserLoginEvent(userID));
        }
    }
}