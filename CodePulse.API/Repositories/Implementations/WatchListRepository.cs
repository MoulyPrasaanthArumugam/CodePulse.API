using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace CodePulse.API.Repositories.Implementations
{
    public class WatchListRepository:IWatchListRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public WatchListRepository(ApplicationDBContext applicationDBContext) 
        {
           _dbContext = applicationDBContext;
        }
        public async Task<Watchlist?> CreateAsync(Watchlist watchlist) 
        {
            var exists = await _dbContext.WatchList
        .AnyAsync(w => w.UserId == watchlist.UserId && w.ContentId == watchlist.ContentId);

            if (exists)
            {
                return watchlist;
            }

            await _dbContext.WatchList.AddAsync(watchlist);
            await _dbContext.SaveChangesAsync();
            return watchlist;



        }
        public async Task<IEnumerable<Watchlist>> GetAllAsync(string userId)
        {
            var watchList = await _dbContext.WatchList.Where(x =>x.UserId == userId).Include(x=>x.Content).ToListAsync();
            return watchList;
        }

        public async Task<Watchlist?> DeleteAsync(string userId , Guid contentId)
        {

            var existingWatchlistItem = await _dbContext.WatchList
        .FirstOrDefaultAsync(w => w.UserId == userId && w.ContentId == contentId);

            if (existingWatchlistItem == null)
            {
                return null;
            }

           // var existingWatchlist = await _dbContext.WatchList.FirstOrDefaultAsync(x => x.Id == id);
             _dbContext.WatchList.Remove(existingWatchlistItem);
                await _dbContext.SaveChangesAsync();
                return existingWatchlistItem;
            
        }
    }
}
