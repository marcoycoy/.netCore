using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Refresher.DataAccess.Repository.IRepository;
using Refresher.Models;
using Refresher.Utility;

namespace Refresher.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> company = _unitofwork.Company.GetAll().ToList();

      
            return View(company);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        public IActionResult Upsert(int? id)
        {
            Company objCompany = _unitofwork.Company.Get(u => u.Id == id);
            if (objCompany == null)
            {
                return View(objCompany);
            }
            return View(objCompany);

        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitofwork.Company.Add(company);
                    TempData["success"] = "Company Added Successfully";
                }
                else
                {
                    _unitofwork.Company.Update(company);
                    TempData["success"] = "Company Updated Successfully";
                }
                _unitofwork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
            
        
        }

        [HttpDelete]
        public IActionResult DeleteCompany(int? id)
        {
            Company companyData = _unitofwork.Company.Get(u => u.Id == id);
            if (companyData == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
        
            _unitofwork.Company.Remove(companyData);
            _unitofwork.Save();
            return Json(new { success = true, message = "Successfully Deleted" });
        }
        #endregion
    }
}
