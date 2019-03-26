using Newtonsoft.Json;
using OneCoreTest.Common.Constants;
using OneCoreTest.DataAccess.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OneCoreTest.Web.Controllers
{
    public class BaseController : Controller
    {
        private ApplicationUser currentUser;

        protected ApplicationUser CurrentUser
        {
            get
            {
                return currentUser ?? GetCurrentUser();
            }
        }

        private ApplicationUser GetCurrentUser()
        {
            HttpCookie cookie = HttpContext.Request.Cookies[SessionConstants.CookieName];

            if (cookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null)
                {
                    currentUser = JsonConvert.DeserializeObject<ApplicationUser>(ticket.UserData);
                }
            }

            return currentUser;
        }
    }
}