using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class CreateContentDTO
    {
        [Required(ErrorMessage = "Title is Required")]
        public String Title { get; set; }
        public string Description { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string TrailerUrl { get; set; }

        public DateTime PublishedDate { get; set; }
        public string Info { get; set; }
        public int RentalDuration { get; set; }
        public bool IsExpired { get; set; }
        public int? LikeCount { get; set; }
        public int? DislikeCount { get; set; }

        [Required (ErrorMessage = "Category is Required")]
        public Guid CategoryId { get; set; } // New property for CategoryId
        [Required (ErrorMessage = "Genre is Required")]
        public List<Guid> Genres { get; set; } // List of genre IDs
    }
}
