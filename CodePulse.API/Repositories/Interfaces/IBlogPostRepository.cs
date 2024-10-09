using CodePulse.API.Model.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IBlogPostRepository
    {
        Task<BlogSpot> CreateAsync(BlogSpot blogPost);
        Task<IEnumerable<BlogSpot>> GetAllAsync();

        Task<BlogSpot?>GetBlogPostByID(Guid id);

        Task<BlogSpot?> GetByUrlHandleAsync(string urlHandle);


        Task<BlogSpot?> UpdateAsync(BlogSpot blogPost);

        Task<BlogSpot?> DeleteAsync(Guid id);
    }
}
