using CodePulse.API.Model.Domain;

namespace CodePulse.API.Model.DTO
{
    public class WatchListDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } // Foreign key to IdentityUser
        public Guid ContentId { get; set; }

        public List<ContentDTO> Contents { get; set; }
    }
}
