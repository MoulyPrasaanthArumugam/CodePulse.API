using System.Text.Json.Serialization;

namespace CodePulse.API.Model.Domain
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        //Many to Many relationship - Each content can have multiple Genres, Each Genres can have multiple contents.
        //[JsonIgnore] // Break the cycle here
        public ICollection<Content> Contents { get; set; }
    }
}
