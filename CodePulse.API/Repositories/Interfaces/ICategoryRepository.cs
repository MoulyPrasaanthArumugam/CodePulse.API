using CodePulse.API.Model.Domain;
using System.Threading.Tasks;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> CreateCategoryAsynch(Category category);
        // Task<IEnumerable<Category>> GetAllCategoriesAsynch(string? query = null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1,
        //     int? pageSize = 100);
        Task<IEnumerable<Category>> GetAllCategoriesAsynch();
        Task<Category> GetCategoryByID(Guid id);
        Task<Category?> UpdateAsynch(Category category);

        Task<Category?> DeleteAsync(Guid id);

        // Task<int> GetCount();
    }
}
