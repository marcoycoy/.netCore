using Refresher.DataAccess.Data;
using Refresher.DataAccess.Repository.IRepository;
using Refresher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Refresher.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) :base(db)
        {
            _db = db;

        }
        public void update(Category obj)
        {
          _db.Categories.Update(obj);
        }

    }
}
