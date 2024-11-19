using CodePulse.API.Model.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre> CreateAsync(Genre genre);
        Task<IEnumerable<Genre>> GetAllAsync();

        Task<Genre> GetByIdAsync(Guid Id);

        Task<Genre?> UpdateAsync(Genre? genre);

        Task<Genre?>DeleteAsync(Guid Id);
    }
}
