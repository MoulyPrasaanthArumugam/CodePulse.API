using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class UpdateGenreDTO
    {
        [Required (ErrorMessage = "Genre Name is Required")]
        public string Name { get; set; }  
    }
}
