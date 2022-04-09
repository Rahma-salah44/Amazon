using Amazon.Data;
using Amazon.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Amazon.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AmazonDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, AmazonDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
             var users = await _context.Users.ToListAsync();
            var clients = users.Where(n =>n.UserType == Data.Enums.Enums.UserType.Client).ToList(); //users.SelectMany(User.IsInRole("Client"));
            return View(clients);
        }

        public async Task<IActionResult> search(string searchString)
        {
            var allUsers = await _context.Users.ToListAsync();
            var clients = allUsers.Where(n => n.UserType == Data.Enums.Enums.UserType.Client).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allUsers.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
                return View("Index", filteredResultNew);
            }
            return View(allUsers);
        }


        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }




        //// GET: Client/Edit/5
        //public async Task<IActionResult> Edit(string? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var client = await _context.Users.FindAsync(id);
        //    if (client == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(client);
        //}

        //// POST: clients/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Id,Name,UserName,Email")] User client)
        //{
        //    if (id != client.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(client);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClientExists(client.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(client);
        //}

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Users.FindAsync(id);
            _context.Users.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}