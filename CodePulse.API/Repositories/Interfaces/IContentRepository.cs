using CodePulse.API.Model.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IContentRepository
    {
        Task<Content> CreateAsync(Content content);
        Task<Like> AddLikeAsync(Like like);
        Task<DisLike> AddDisLikeAsync(DisLike disLike);

        Task<IEnumerable<Content>> GetAllAsync();
        Task<Content?> GetByIDAsync(Guid id);
        Task<IEnumerable<Content>> GetByGenreAsync(Guid id);
        Task<IEnumerable<Content?>> GetByCategoryAsync(Guid id);
        Task<IEnumerable<Content?>> GetByLikesAsync();

        Task<Content?> UpdateAsync(Content blogPost);
        Task<Content?> DeleteAsync(Guid id);
    }
}
