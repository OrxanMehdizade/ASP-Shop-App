namespace ASP_Shop_App.Models
{
    public class Product : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<Order>? Orders { get; set; }

        public List<AppUser>? AppUsers { get; set; }


    }
}
