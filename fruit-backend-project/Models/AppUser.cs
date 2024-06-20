using Microsoft.AspNetCore.Identity;

namespace fruit_backend_project.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public Basket Basket { get; set; }
    }
}
