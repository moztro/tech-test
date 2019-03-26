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

            // Store the user data in session

            return RedirectToAction("Index", "Users");
        }
    }
}