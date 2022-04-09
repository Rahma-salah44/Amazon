using Amazon.Data;
using Amazon.Data.Static;
using Amazon.Data.ViewModels;
using Amazon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Amazon.Data.Enums.Enums;

namespace Amazon.Controllers
{
    public class AccountManagementController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly AmazonDbContext context;
       
        public AccountManagementController( UserManager<User> _userManager, SignInManager<User> _signManager, AmazonDbContext _context)
        {
            userManager = _userManager;
            signInManager = _signManager;
            context= _context;

        }
        public async Task<IActionResult> Login(string returnUrl) {
           return  View(new LoginVM() {
              ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList(),
              ReturnUrl = returnUrl

           });
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM) {
            loginVM.ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            loginVM.ReturnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return View(loginVM);
            var user = await userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user,loginVM.Password);
                if (passwordCheck)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, loginVM.Password, false);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, true);
                        return RedirectToAction("Index", "Home");
                    }

                }
            }
            TempData["Error"] = "Wrong Credentials. Please, try again.";
            return View(loginVM);
        }
        public IActionResult Signup() => View(new SignupVM());
        [HttpPost]
        public async Task<IActionResult> Signup(SignupVM signupVM)
        {
            var user = await userManager.FindByEmailAsync(signupVM.EmailAddress);
            if (user != null)
                ModelState.AddModelError("EmailAddress", "This  Email Address already in use");
            user = await userManager.FindByNameAsync(signupVM.UserName);
            if (user != null)
                ModelState.AddModelError("UserName", "This UserName already in use");

            if (!ModelState.IsValid) return View(signupVM);

            var newUser = new User
            {
                Name= signupVM.FullName,
                Email= signupVM.EmailAddress,
                UserName= signupVM.UserName,
                Address = signupVM.Address,
                UserType=signupVM.UserType,
                EmailConfirmed = true

            };
            var newUserResponse = await userManager.CreateAsync(newUser,signupVM.Password );
                if (newUserResponse.Succeeded)
                {
                     await userManager.AddToRoleAsync(newUser, UserRoles.Client);

                }
            return View("RegisterCompleted");
        }
        
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
         }
        public async  Task<IActionResult> Users() {
            var Users = await context.Users.ToListAsync();
            return View(Users);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "AccountManagement", new { ReturnUrl =returnUrl});
       
            var properties =  signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);

         }
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl=null, string remoteError=null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new LoginVM()
            {
                ReturnUrl= returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            if (remoteError != null)
            {
                ModelState.AddModelError(String.Empty, $"Error from External Provider:{remoteError}");
                return View("Login", loginVM);
            }
            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(String.Empty, $"Error loading External Provider:{remoteError}");
                return View("Login", loginVM);
            }
            var signInResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {

                    var user = await userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new User
                        {
                            Name = info.Principal.FindFirstValue(ClaimTypes.Name),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Address = "",
                            UserType = UserType.Client,
                            EmailConfirmed = true

                        };
                        await userManager.CreateAsync(user, "Coding@1234?");
                    }
                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);

                }
            }
            ViewBag.ErrorTitle = $"Email Claim not recieved from:{ info.LoginProvider}";
            ViewBag.ErrorMessage = $"Please Contact Support on Amazon.com";
            return View("Error");
        }

    }
}
