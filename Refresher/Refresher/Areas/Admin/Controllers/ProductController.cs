using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using NuGet.Packaging.Signing;
using Refresher.DataAccess.Repository.IRepository;
using Refresher.Models;
using Refresher.Models.ViewModels;
using Refresher.Utility;
using System.Drawing.Text;

namespace Refresher.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _unitofwork.Product.GetAll(includeProperties:"Category").ToList();
          
            IEnumerable<SelectListItem> CategoryList = _unitofwork.Category.
                GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(products);
        }

        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> CategoryList = _unitofwork.Category.
            // GetAll().Select(u => new SelectListItem
            // {
            //     Text = u.Name,
            //     Value = u.Id.ToString()
            // });
            ////ViewBag.CategoryList = CategoryList;
            //ViewData["CategoryList"] = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.
                                 GetAll().Select(u => new SelectListItem
                                 {
                                     Text = u.Name,
                                     Value = u.Id.ToString()
                                 }),
                Product = new Product()
            };
            return View(productVM);
        }

        public IActionResult Upsert(int? id) //Update and insert
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitofwork.Category.
                                 GetAll().Select(u => new SelectListItem
                                 {
                                     Text = u.Name,
                                     Value = u.Id.ToString()
                                 }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //Create
                return View(productVM);
            }
            else
            {
                //UPdate
                productVM.Product = _unitofwork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
                
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
      
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete old image
                        var oldimagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimagePath))
                        {
                            System.IO.File.Delete(oldimagePath);

                        }
                    }

                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if(productVM.Product.Id != 0)
                {

                    _unitofwork.Product.update(productVM.Product);
                    TempData["success"] = "Product Updated Successfully";
                }
                else
                {
                    _unitofwork.Product.Add(productVM.Product);
                    TempData["success"] = "Product Added Successfully";

                }
                _unitofwork.Save();
              
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitofwork.Category.
                            GetAll().Select(u => new SelectListItem
                            {
                                Text = u.Name,
                                Value = u.Id.ToString()
                            });
                return View(productVM);
            }
        }
        //public IActionResult Create(ProductVM productVM)
        //{
        //    if (productVM.Product.Description.ToString() == productVM.Product.Title.ToString())
        //    {
        //        ModelState.AddModelError("title", "The Title Cannot exactly to the Description");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _unitofwork.Product.Add(productVM.Product);
        //        _unitofwork.Save();
        //        TempData["success"] = "Product Created Successfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {

        //        productVM.CategoryList = _unitofwork.Category.
        //                    GetAll().Select(u => new SelectListItem
        //                    {
        //                        Text = u.Name,
        //                        Value = u.Id.ToString()
        //                    });
        //        return View(productVM);
        //    }
        //}
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product ProductId = _unitofwork.Product.Get(u => u.Id == id);
            if (ProductId == null)
            {
                return NotFound();
            }
            return View(ProductId);
        }


        [HttpPost]
        public IActionResult Edit(Product obj)
        {


            if (ModelState.IsValid)
            {
                _unitofwork.Product.Update(obj);
                _unitofwork.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productData = _unitofwork.Product.Get(u => u.Id == id);
            if (productData == null)
            {
                return NotFound();
            }
            return View(productData);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product ProductData = _unitofwork.Product.Get(u => u.Id == id);
            if (ProductData == null)
            {
                return NotFound();
            }
            _unitofwork.Product.Remove(ProductData);
            _unitofwork.Save();
            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");

        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll() {
            List <Product> objProductList = _unitofwork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {
            Product productData = _unitofwork.Product.Get(u => u.Id == id);
            if (productData == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            //delete old image
            var oldimagePath = Path.Combine(_webHostEnvironment.WebRootPath, productData.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldimagePath))
            {
                System.IO.File.Delete(oldimagePath);

            }
            _unitofwork.Product.Remove(productData);
            _unitofwork.Save();
            return Json(new { success = true, message= "Successfully Deleted" });
        }
        #endregion
    }
}
