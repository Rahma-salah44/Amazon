
       


using Amazon.Data;
using Amazon.Data.Services;
using Amazon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Amazon.Controllers
{
    public class ProductController : Controller
    {
        //private readonly AmazonDbContext _context;

        //public ProductController(AmazonDbContext context)
        //{
        //    _context = context;
        //}
        private readonly IProductServices _service;

        public ProductController(IProductServices service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _service.GetAllAsync();
            var categories = _service.GetAllCategoris();
            ViewData["categories"] = categories;
            return View(products);

        }
        public async Task<IActionResult> Create()
        {

            Product product = new Product();
            var categories = _service.GetAllCategoris();//_context.categories.ToList();
            ViewData["categories"] = categories;
            var vendors = _service.GetAllVendors();//_context.Vendor.ToList();
            ViewData["vendors"] = vendors;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            var categories = _service.GetAllCategoris();
            var vendors = _service.GetAllVendors();
            ViewData["vendors"] = vendors;
            ViewData["categories"] = categories;

            if (product.Name != null && product.CategoryId != 0 && product.Description != null && product.VendorId != 0 && product.ImageURL != null)

            {
                product.ImageURL = "img/product/" + product.ImageURL;
                await _service.AddAsync(product);
                //_context.Products.Add(product);
                //_context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }


            return View(product);


        }
        public async Task<IActionResult> Details(int Id)
        {
            var product = await _service.GetByIdAsync(Id);//_context.Products.FirstOrDefault(P => P.Id == Id);
            //var name = product.Vendor.Name;
            return View(product);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var product = await _service.GetByIdAsync(Id);//_context.Products.Find(Id);
            return View("Details", product);
        }

        public async Task<IActionResult> DeleteConfirmd(int Id)
        {
            await _service.DeleteAsync(Id);
            //_context.Products.Remove(_context.Products.Find(Id));
            //_context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> Edit(int Id)
        {
            var product = await _service.GetByIdAsync(Id);//_context.Products.Find(Id);
            SelectList categoryLst = new SelectList(_service.GetAllCategoris(), "Id", "Name");
            ViewBag.category = categoryLst;
            SelectList vendorLst = new SelectList(_service.GetAllVendors(), "Id", "Name");
            ViewBag.vendor = vendorLst;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product newProduct)
        {

            var product = await _service.GetByIdAsync(newProduct.Id);//_context.Products.Find(newProduct.Id);
            //product.Name = newProduct.Name;
            //product.CategoryId = newProduct.CategoryId;
            //product.VendorId = newProduct.VendorId;
            //product.PricePerUnit = newProduct.PricePerUnit;
            product.ImageURL = "img/product/" + newProduct.ImageURL;
            await _service.UpdateAsync(product.Id, product);
            //_context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}