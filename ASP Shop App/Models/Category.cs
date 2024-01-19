namespace ASP_Shop_App.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? ImageUrlCategory { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
