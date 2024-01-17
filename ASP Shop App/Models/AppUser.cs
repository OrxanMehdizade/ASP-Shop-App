using Microsoft.AspNetCore.Identity;

namespace ASP_Shop_App.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; }=null!;
    }
}
