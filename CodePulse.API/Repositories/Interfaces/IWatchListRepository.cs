using CodePulse.API.Model.Domain;

namespace CodePulse.API.Repositories.Interfaces
{
    public interface IWatchListRepository
    {
        Task<Watchlist?> CreateAsync(Watchlist watchlist);
        Task<IEnumerable<Watchlist>> GetAllAsync(string userId);
        Task<Watchlist> DeleteAsync(string userId, Guid contentId);
    }
}
