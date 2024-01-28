using ASP_Shop_App.Data;
using ASP_Shop_App.Models;
using ASP_Shop_App.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP_Shop_App.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _appContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _automapper;


        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,AppDbContext context, IMapper automapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appContext = context;
            _automapper = automapper;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
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

                user.orders = new() { UserId = user.Id };
                var result =await _userManager.CreateAsync(user, registerModel.Password);
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products=_appContext.Products.Include(c=>c.Category).ToList();
            return View(products);
        }


        public IActionResult GetAllCategorys(int? category)
        {
            var categories = _appContext.Categorys.ToList();
            ViewData["Categories"] = categories;

            IQueryable<Product> products = _appContext.Products;

            if (category.HasValue && category > 0)
            {
                products = products.Where(p => p.CategoryId == category);
            }

            var productList = products.ToList();
            return View(productList);
        }


        public async Task<IActionResult> OrderCartAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _appContext.Orders
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.UserId == user!.Id);

            if (cart != null && !cart.IsOrdered)
            {
                var productsInCart = cart.Products.ToList();
                return View(productsInCart);
            }
            return View(new List<Product>());
        }


        public async Task<IActionResult> AddCartAsync(int id)
        {
            var product = await _appContext.Products.FindAsync(id);

            if (product == null) return RedirectToAction("GetAllCategorys");

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var cart = await _appContext.Orders
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && !c.IsOrdered);

                if (cart == null)
                {
                    cart = new Order { UserId = user.Id };
                    _appContext.Orders.Add(cart);
                }

                cart.Products ??= new List<Product>();
                cart.Products.Add(product);

                await _appContext.SaveChangesAsync();

                return RedirectToAction("GetAllCategorys");
            }

            return RedirectToAction("GetAllCategorys");
        }




        public async Task<IActionResult> OrderAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var cart = await _appContext.Orders
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id && !c.IsOrdered);

                if (cart != null)
                {
                    cart.IsOrdered = true;
                    cart.Products.Clear();
                    await _appContext.SaveChangesAsync();
                }
            }

            return RedirectToAction("GetAllCategorys");
        }


    }

}
