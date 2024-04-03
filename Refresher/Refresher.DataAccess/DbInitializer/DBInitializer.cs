using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Refresher.DataAccess.Data;
using Refresher.Models;
using Refresher.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refresher.DataAccess.DbInitializer
{
    public class DBInitializer : IDBInitializer
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DBInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();
            }

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@dotnetmastery.com",
                Email = "admin@dotnetmastery.com",
                Name = "martin Husmillo",
                PhoneNumber = "1234567890",
                StreetAddress = "test123",
                State = "test23123",
                PostalCode = "1234567890",
                City = "1234567890",
            }, "Admin123*").GetAwaiter().GetResult();
            ApplicationUser User = _db.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@dotnetmastery.com");
            _userManager.AddToRoleAsync(User, SD.Role_Admin).GetAwaiter().GetResult();

            return;
        }
    }
}
