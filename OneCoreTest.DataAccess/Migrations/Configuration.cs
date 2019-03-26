using OneCoreTest.Common.Security;
using OneCoreTest.DataAccess.Contexts;
using OneCoreTest.DataAccess.Entities.Enums;
using OneCoreTest.DataAccess.Entities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.DataAccess.Migrations
{
    public class Configuration : DbMigrationsConfiguration<OneCoreTestDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(OneCoreTestDbContext context)
        {
            context.Users.AddOrUpdate(
                usr => usr.Email,
                new ApplicationUser
                {
                    Email = "admin@example.com",
                    Username = "Administrator",
                    Genre = Genre.Male,
                    Password = PasswordEncryptor.EncryptPassword("Password1@"),
                    Status = true,
                    CreatedBy = "seed",
                    CreatedDate = DateTime.Now
                }
            );

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
