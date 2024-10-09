namespace CodePulse.API.Model.Domain
{
    public class BlogSpot
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool IsVisible { get; set; }

        public ICollection<Category> categories { get; set; }

    }
}
