using OneCoreTest.DataAccess.Contexts;
using OneCoreTest.DataAccess.Entities.Security;
using OneCoreTest.Services.Infrastructure.Repositories;
using OneCoreTest.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Implementations
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(OneCoreTestDbContext context)
            : base(context)
        { }
    }
}
