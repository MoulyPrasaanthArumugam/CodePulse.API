namespace CodePulse.API.Model.Domain
{
    public class Like
    {
        public Guid Id { get; set; } // Primary key
        public string UserId { get; set; } // Foreign key to IdentityUser
        public Guid ContentId { get; set; } // Foreign key to Content

        // Optional: Navigation properties
        public Content Content { get; set; }
    }
}
