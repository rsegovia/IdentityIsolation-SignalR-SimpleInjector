﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OAuth;

namespace EP.IdentityIsolation.Infra.CrossCutting.Identity.Provider
{
    public class ApplicationOAuthBearerAuthenticationProvider
        : OAuthBearerAuthenticationProvider
    {
        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            // try to find bearer token in a cookie 
            // (by default OAuthBearerAuthenticationHandler 
            // only checks Authorization header)
            var tokenCookie = context.OwinContext.Request.Cookies["BearerToken"];
            if (!string.IsNullOrEmpty(tokenCookie))
                context.Token = tokenCookie;
            return Task.FromResult<object>(null);
        }

    }
}
