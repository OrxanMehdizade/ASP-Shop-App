using ASP_Shop_App.Data;
using ASP_Shop_App.Models;
using ASP_Shop_App.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ASP_Shop_App.Controllers
{
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
            var products=_appContext.Products.Include(c=>c.Category).ToList();
            return View(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartProduct(int id)
        {
            var product = await _appContext.Products.FindAsync(id);

            if (product != null)
            {
                // Check if the user is authenticated
                if (User.Identity.IsAuthenticated)
                {
                    // Retrieve user ID
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    // Retrieve or create a user order
                    var userOrder = await _appContext.Orders
                        .Include(o => o.Products)
                        .FirstOrDefaultAsync(o => o.UserId == userId);

                    if (userOrder == null)
                    {
                        // Create a new order if the user doesn't have one
                        userOrder = new Order
                        {
                            UserId = userId,
                            Products = new List<Product> { product }
                        };

                        _appContext.Orders.Add(userOrder);
                    }
                    else
                    {
                        // Add the product to the existing user order
                        userOrder.Products ??= new List<Product>();
                        userOrder.Products.Add(product);
                    }

                    await _appContext.SaveChangesAsync();
                }
                else
                {
                    // For non-authenticated users, you might want to implement session-based cart logic
                    // Example: HttpContext.Session.Get<List<Product>>("Cart") ?? new List<Product>();
                }

                return RedirectToAction("GetAllProducts");
            }

            return NotFound();
        }


    }

}
