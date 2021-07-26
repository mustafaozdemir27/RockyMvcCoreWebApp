using RockyMvcCoreWebApp_DataAccess.Data;
using RockyMvcCoreWebApp_DataAccess.Repository.IRepository;
using RockyMvcCoreWebApp_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RockyMvcCoreWebApp_DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository

    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
