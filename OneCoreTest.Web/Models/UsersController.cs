using OneCoreTest.Services.Interfaces;
using OneCoreTest.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OneCoreTest.DataAccess.Contexts;
using System.Threading.Tasks;
using OneCoreTest.DataAccess.Entities.Security;
using OneCoreTest.Common.Security;

namespace OneCoreTest.Web.Models
{
    public class UsersController : Controller
    {
        private IUserRepository usersRepository { get; set; }

        public UsersController()
        {
            // Create new instance of db context
            var dbContext = new OneCoreTestDbContext();

            // inject db context into repository
            usersRepository = new UserRepository(dbContext);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            // query for all users
            IQueryable<ApplicationUser> users = await usersRepository.GetAll();

            // list users in the UI
            return View(users.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            // Lookup for specified active user
            ApplicationUser user = await usersRepository.Get(u =>
                u.Id.ToString() == id
            );

            // Check if user was found
            if (user == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con Id={id}");
            }

            // Send view model to UI
            var userViewModel = new ApplicationUserModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Genre = user.Genre,
                Username = user.Username
            };

            return View(userViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(ApplicationUserModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelStateInvalid", "Algunos campos son incorrectos");
                return View(model);
            }

            // Lookup for any duplicated user
            ApplicationUser existingUser = await usersRepository.Get(u =>
                u.Email == model.Email
            );

            // check if user already exists with that email
            if (existingUser != null)
            {
                ModelState.AddModelError("DuplicatedUser", "Ya existe un usuario con ese correo electrónico");
                return View(model);
            }

            // create new user
            ApplicationUser newUser = new ApplicationUser()
            {
                Username = model.Username,
                Password = PasswordEncryptor.EncryptPassword(model.Password),
                Genre = model.Genre,
                Email = model.Email,
                CreatedBy = "user",
                CreatedDate = DateTime.Now
            };

            // insert into db
            int affectedRecords = await usersRepository.Insert(newUser);
            bool success = affectedRecords > 0;

            // Notify
            if (success)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("CreationError", "No es posible crear el registro en éste momento. Inténte más tarde.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit(ApplicationUserModel model)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("ModelStateInvalid", "Algunos campos son incorrectos");
                return View(model);
            }

            // Lookup for active users matching id
            ApplicationUser old = await usersRepository.Get(u =>
                u.Id.ToString() == model.Id
            );

            // check if user is null
            if(old == null)
            {
                throw new KeyNotFoundException($"No se encontró un registro con Id={model.Id}");
            }

            // Lookup for any duplicated user
            ApplicationUser existingUser = await usersRepository.Get(u =>
                u.Email == model.Email
                && u.Id.ToString() != model.Id
            );

            // check if user already exists with that email
            if (existingUser != null)
            {
                ModelState.AddModelError("DuplicatedUser", "Ya existe un usuario con ese correo electrónico");
                return View(model);
            }

            // update the data
            old.Username = model.Username;
            old.Password = PasswordEncryptor.EncryptPassword(model.Password);
            old.Genre = model.Genre;
            old.Email = model.Email;
            old.UpdatedBy = "user";
            old.UpdatedDate = DateTime.Now;

            // update the object
            int affectedRecords = await usersRepository.Update(old);
            bool success = affectedRecords > 0;

            // Notify
            if(success)
                return RedirectToAction("Index");
            else
            {
                ModelState.AddModelError("UpdateError", "No es posible actualizar la información en éste momento. Inténte más tarde.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            // Lookup for specified active user
            ApplicationUser user = await usersRepository.Get(u =>
                u.Id.ToString() == id
            );

            // Check if user was found
            if(user == null)
            {
                throw new KeyNotFoundException($"No se encontró un usuario con Id={id}");
            }

            // Update the record with inactive flag
            user.Status = false;

            int affectedRecords = await usersRepository.Update(user);
            bool success = affectedRecords > 0;

            // Notify
            if(success)
            {
                return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new Exception("El registro no pudo ser actualizado. Intente más tarde.");
            }
            
        }
    }
}