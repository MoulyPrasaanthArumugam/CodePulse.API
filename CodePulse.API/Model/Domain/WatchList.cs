using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Model.Domain
{
    public class Watchlist
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to IdentityUser
        public Guid ContentId { get; set; }

        //public DateTime AddedDate { get; set; } // Optional: Track when the item was added

        // Navigation properties
        public IdentityUser User { get; set; }
        public Content Content { get; set; }
    }
}
