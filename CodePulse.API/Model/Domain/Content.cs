namespace CodePulse.API.Model.Domain
{
    public class Content
    {
        public Guid Id { get; set; }
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
        //Many to Many relationship - Each content can have multiple Genres, Each Genres can have multiple contents.
        public ICollection<Genre> Genres { get; set; }

        public Guid CategoryId { get; set; } // Foreign key to Category
        public Category Category { get; set; } // Navigation property


    }
}
