using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class CreateGenreDTO
    {
        [Required (ErrorMessage = "Genre Name is Empty")]
        public string? Name { get; set; }  
    }
}
