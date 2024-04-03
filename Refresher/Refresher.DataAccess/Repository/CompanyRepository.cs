using Refresher.DataAccess.Data;
using Refresher.DataAccess.Repository.IRepository;
using Refresher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refresher.DataAccess.Repository
{
    public class CompanyRepository :Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;

        }
        public void update(Company obj)
        {
            _db.Companies.Update(obj);
        }

    }
}
