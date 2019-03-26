using OneCoreTest.DataAccess.Entities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.DataAccess.Contexts
{
    public class OneCoreTestDbContext : DbContext
    {
        public OneCoreTestDbContext()
            : base("OneCoreTestDbContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #region Database sets
        public DbSet<ApplicationUser> Users { get; set; }
        #endregion Database sets
    }
}
