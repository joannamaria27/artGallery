﻿using ArtGallery.DataAcess.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.DataAcess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArtGallery.Models.ViewModels;
using System.Collections.Generic;

namespace ArtGalleryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _productRepository.GetAll(includePropertis: "Category").ToList();
            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductViewModel productVM = new()
            {

                CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _productRepository.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (productVM.Product.CreatedDate <= DateTime.Now)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");
                        if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\images\product\" + fileName;
                    }

                    if (productVM.Product.Id != 0)
                    {
                        _productRepository.Update(productVM.Product);
                        _productRepository.Save();
                        TempData["success"] = "Product edit successfully";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        _productRepository.Add(productVM.Product);
                        _productRepository.Save();
                        TempData["success"] = "Product created successfully";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("CreatedDate", "Created date must be today or in the past");
                    return View(productVM);
                }
            }
            else
            {
                productVM.CategoryList = _categoryRepository.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVM);
            }

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _productRepository.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (!string.IsNullOrEmpty(productFromDb.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, productFromDb.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product obj = _productRepository.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _productRepository.Remove(obj);
            _productRepository.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _productRepository.GetAll(includePropertis: "Category").ToList();
            return Json(new { data = objProductList });
        }
        #endregion



    }

}
