using ASP_Shop_App.Data;
using ASP_Shop_App.Models;
using ASP_Shop_App.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP_Shop_App.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appContext = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if(ModelState.IsValid)
            {
                AppUser user = new()
                {
                    Email = registerModel.Email,
                    FullName = registerModel.FullName,
                    UserName = registerModel.Email
                };
                var result=await _userManager.CreateAsync(user, registerModel.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View (registerModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginModel.Email);
                if (user is not null) 
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return Redirect("/");
                    }
                    ModelState.AddModelError("All","email or password not valid");

                }


            }
            return View(loginModel);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products=_appContext.Products.ToList();
            return View(products);
        }
    }
}
