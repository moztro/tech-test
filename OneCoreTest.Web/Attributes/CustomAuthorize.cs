using OneCoreTest.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace OneCoreTest.Web.Attributes
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!AuthorizeCore(filterContext.HttpContext))
            {
                // if user is not authorized, redirect to login screen
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { area = "", controller = "Account", action = "Login" })
                );
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool expired = true;

            // Get the cookie with user session
            HttpCookie cookie = httpContext.Request.Cookies[SessionConstants.CookieName];

            if (cookie != null)
            {
                // decrypt the ticket
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                // set the expired ticket flag
                expired = ticket.Expired;
            }

            if (expired)
            {
                // if expired, get the auth token from headers
                var authToken = httpContext.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authToken))
                {
                    expired = false;
                }
            }

            return !expired;
        }
    }
}