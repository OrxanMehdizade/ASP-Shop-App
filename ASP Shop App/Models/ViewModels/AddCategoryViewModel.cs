namespace ASP_Shop_App.Models.ViewModels
{
    public class AddCategoryViewModel
    {
        public string Name { get; set; } = null!;
        public IFormFile ImageUrlCategory { get; set; }
    }
}
