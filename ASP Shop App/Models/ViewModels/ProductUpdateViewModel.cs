namespace ASP_Shop_App.Models.ViewModels
{
    public class ProductUpdateViewModel
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageUrl { get; set; }
    }
}
