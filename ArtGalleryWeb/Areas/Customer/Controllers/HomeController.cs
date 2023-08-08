using ArtGallery.DataAcess.Repository.IRepository;
using ArtGallery.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ArtGalleryWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepository.GetAll(includePropertis: "Category").ToList();
            return View(productList);
        }
        public IActionResult Details(int productId)
        {
            Product product = _productRepository.Get(u=>u.Id== productId, includePropertis: "Category");
            return View(product);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}