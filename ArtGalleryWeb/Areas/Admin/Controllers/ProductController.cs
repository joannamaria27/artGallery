using ArtGallery.DataAcess.Data;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using ArtGallery.DataAcess.Repository.IRepository;

namespace ArtGalleryWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository context)
        {
            _productRepository = context;
        }
        public IActionResult Index()
        {
            List<Product> objProductList = _productRepository.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.CreatedDate <= DateTime.Now)
                {
                    _productRepository.Add(obj);
                    _productRepository.Save();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CreatedDate", "Created date must be today or in the past");
                }
            }
            return View();
        }

        public IActionResult Edit(int? id)
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
            return View(productFromDb);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product obj)
        {
            if (obj.CreatedDate <= DateTime.Now)
            {
                _productRepository.Update(obj);
                _productRepository.Save();
                TempData["success"] = "Product edited successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("CreatedDate", "Created date must be today or in the past");
            }
            return View();
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
    }
}
