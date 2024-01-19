using ASP_Shop_App.Data;
using ASP_Shop_App.Helpers;
using ASP_Shop_App.Models;
using ASP_Shop_App.Models.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASP_Shop_App.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMapper _automapper;
        public AdminController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _automapper = mapper;
        }
        public IActionResult IndexA()
        {
            return View();
        }

        //---------------------------------Product methods------------------------------------
        public IActionResult GetAll(int? categoryId)
        {
            var products = _context.Products.Include(p => p.Category).ToList();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            return View(products);
        }


        [HttpGet]
        public IActionResult Add()
        {
            var category = _context.Categorys.ToList();
            ViewData["Categorys"] = category;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAPViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = _automapper.Map<Product>(model);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetAll");


            }
            return View(model);
        }




        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("GetAll");
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ProductUpdateViewModel viewModel = new()
            {
                ImageUrl = null,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,

            };

            return View(viewModel);

        }


        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            product.Title = model.Title;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = await UploadFileHelper.UploadFile(model.ImageUrl);

            await _context.SaveChangesAsync();

            return RedirectToAction("GetAll");

        }



        //---------------------------------Category methods------------------------------------

        [HttpGet]
        public IActionResult GetCategory()
        {
            var categories = _context.Categorys.ToList();
            return View(categories);
        }


        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var category = _automapper.Map<Category>(model);
                _context.Categorys.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("GetCategory");


            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categorys.FirstOrDefaultAsync(p => p.Id == id);
            if (category != null)
            {
                _context.Categorys.Remove(category);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("GetCategory");
        }



        [HttpGet]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _context.Categorys.FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            UpdateCategoryViewModel viewModel = new()
            {
                ImageUrlCategory = null,
                Name = category.Name,

            };

            return View(viewModel);

        }


        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var category = await _context.Categorys.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = model.Name;
            category.ImageUrlCategory = await UploadFileHelper.UploadFile(model.ImageUrlCategory);

            await _context.SaveChangesAsync();

            return RedirectToAction("GetCategory");

        }
    }
}
