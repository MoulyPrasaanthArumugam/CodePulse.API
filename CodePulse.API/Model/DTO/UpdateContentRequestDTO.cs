namespace CodePulse.API.Model.DTO
{
    public class UpdateContentRequestDTO
    {
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
        public Guid CategoryId { get; set; } // New property for CategoryId

        public List<Guid> Genres { get; set; } = new List<Guid>();
    }
}
