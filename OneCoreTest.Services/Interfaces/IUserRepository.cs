using OneCoreTest.DataAccess.Entities.Security;
using OneCoreTest.Services.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneCoreTest.Services.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser>
    {
    }
}
