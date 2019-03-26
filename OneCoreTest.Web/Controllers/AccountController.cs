using Newtonsoft.Json;
using OneCoreTest.Common.Constants;
using OneCoreTest.Common.Security;
using OneCoreTest.DataAccess.Contexts;
using OneCoreTest.DataAccess.Entities.Security;
using OneCoreTest.Services.Implementations;
using OneCoreTest.Services.Interfaces;
using OneCoreTest.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OneCoreTest.Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository usersRepository { get; set; }

        public AccountController()
        {
            var dbContext = new OneCoreTestDbContext();

            usersRepository = new UserRepository(dbContext);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelStateInvalid", "Los datos capturados son incorrectos");
                return View(login);
            }

            // encrypt to compare agains stored password
            string encryptedPassword = PasswordEncryptor.EncryptPassword(login.Password);

            // Lookup for any user matching credentials
            ApplicationUser user = await usersRepository.Get(u =>
                u.Email == login.Email
                && u.Password == encryptedPassword
            );

            // No user with valid credentials or status active found
            if(user == null)
            {
                ModelState.AddModelError("InvalidCredentials", "El usuario y/o contraseña son incorrectos");
                return View(login);
            }

            // serialize the user data
            string json = JsonConvert.SerializeObject(user);

            // create the auth ticket
            FormsAuthentication.SetAuthCookie(user.Email, true);
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                version: 1,
                name: user.Email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(120),
                isPersistent: false,
                userData: json,
                cookiePath: FormsAuthentication.FormsCookiePath
            );

            // encrypt the ticket and create a cookie
            Response.Cookies.Add(new HttpCookie(SessionConstants.CookieName, FormsAuthentication.Encrypt(ticket)));

            // Success redirect
            return RedirectToAction("Index", "Users");
        }
    }
}