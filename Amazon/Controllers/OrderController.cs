using Amazon.Data;
using Amazon.Data.Cart;
using Amazon.Data.Services;
using Amazon.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Amazon.Controllers
{
    public class OrderController : Controller
    {
        private readonly AmazonDbContext _context;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;


        public OrderController(AmazonDbContext context, ShoppingCart shoppingCart, IOrdersService ordersService)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
        }
        //private readonly IProductServices _productService;
        //private readonly ShoppingCart _shoppingCart;
        ////private readonly IOrdersService _ordersService;

        //public OrderController(IProductServices productService, ShoppingCart shoppingCart)/*, IOrdersService ordersService*/
        //{
        //    _productService = productService;
        //    _shoppingCart = shoppingCart;
        //    //_ordersService = ordersService;
        //}

        //public async Task<IActionResult> Index()
        //{
        //    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    string userRole = User.FindFirstValue(ClaimTypes.Role);

        //    var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
        //    return View(orders);
        //}

        public IActionResult shoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(response);
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, ClaimTypes.Role);

            return View(orders);
        }

        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            var item = await _context.Products
               .Include(v => v.Vendor)
               .Include(c => c.Category)
               .FirstOrDefaultAsync(n => n.Id == id);

            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            var item = await  _context.Products
               .Include(v => v.Vendor)
               .Include(c => c.Category)
               .FirstOrDefaultAsync(n => n.Id == id);
            

            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        public async Task<IActionResult> CompleteOrder()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

            await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);

            await _shoppingCart.ClearShoppingCartAsync();

            return View("OrderCompeleted");
        }
    }
}
