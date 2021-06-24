using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RockyMvcCoreWebApp;
using RockyMvcCoreWebApp_DataAccess.Data;
using RockyMvcCoreWebApp_DataAccess.Repository.IRepository;
using RockyMvcCoreWebApp_Models;
using RockyMvcCoreWebApp_Models.ViewModel;
using RockyMvcCoreWebApp_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> objList = _productRepo.GetAll(includeProperties: "Category,ApplicationType");

            //foreach (var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(x => x.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(x => x.Id == obj.ApplicationTypeId);
            //}
            return View(objList);
        }

        // GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            //ViewBag.CategoryDropDown = CategoryDropDown;
            //Product product = new Product();
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _productRepo.GetAllDropdownList(WC.CategoryName),
                ApplicationTypeSelectList = _productRepo.GetAllDropdownList(WC.ApplicationTypeName),
            };
            if (id == null)
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _productRepo.Find(id.GetValueOrDefault());
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.Image = fileName + extension;

                    _productRepo.Add(productVM.Product);
                    _productRepo.Save();
                }
                else
                {
                    //Updating
                    var objFromDb = _productRepo.FirstOrDefault(x => x.Id == productVM.Product.Id, isTracking: false);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _productRepo.Update(productVM.Product);
                    _productRepo.Save();
                }

                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _productRepo.GetAllDropdownList(WC.CategoryName);

            productVM.ApplicationTypeSelectList = _productRepo.GetAllDropdownList(WC.ApplicationTypeName);

            return View(productVM);
        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _productRepo.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ApplicationType");
            //product.Category = _db.Category.Find(product.CategoryId);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            var obj = _productRepo.Find(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _productRepo.Remove(obj);
            _productRepo.Save();
            return RedirectToAction("Index");

        }
    }
}
