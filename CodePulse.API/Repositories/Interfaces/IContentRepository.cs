using CodePulse.API.Model.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IContentRepository
    {
        Task<Content> CreateAsync(Content content);
        Task<IEnumerable<Content>> GetAllAsync();
        Task<Content?> GetByIDAsync(Guid id);

        //Task<BlogSpot?> GetByUrlHandleAsync(string urlHandle);
        Task<Content?> UpdateAsync(Content blogPost);
        Task<Content?> DeleteAsync(Guid id);
    }
}
