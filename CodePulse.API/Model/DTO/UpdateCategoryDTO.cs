using System.ComponentModel.DataAnnotations;

namespace CodePulse.API.Model.DTO
{
    public class UpdateCategoryDTO
    {
        [Required (ErrorMessage = "Category Name is Required")]
        public string Name { get; set; }
    }
}
