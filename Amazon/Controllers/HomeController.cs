using Amazon.Data;
using Amazon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Amazon.Controllers
{
    public class HomeController : Controller
    {
        private readonly AmazonDbContext _context;

        public HomeController(AmazonDbContext context)
        {
            _context = context;
        }

        private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            var categories = _context.categories.ToList();
           ViewData["categories"] = categories;
            return View(products);

        }
        public IActionResult SearchByCategory(int Id)
        {
            var Products = _context.Products.Where(p => p.CategoryId == Id).ToList();
            var categories = _context.categories.ToList();
            ViewData["categories"] = categories;
            return View("Index", Products);
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

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

    }
}