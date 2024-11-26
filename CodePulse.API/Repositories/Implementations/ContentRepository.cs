using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace CodePulse.API.Repositories.Implementations
{
    public class ContentRepository : IContentRepository
    {
        private readonly ApplicationDBContext dbContext;

        public ContentRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Content> CreateAsync(Content content)
        {
            await dbContext.Contents.AddAsync(content);
            await dbContext.SaveChangesAsync();
            return content;
        }

        public async Task<Like?> AddLikeAsync(Like like)
        {
            var liked = await dbContext.Like.FirstOrDefaultAsync(x => x.UserId == like.UserId && x.ContentId == like.ContentId);
            var disLiked = await dbContext.Dislike.FirstOrDefaultAsync(x => x.UserId == like.UserId && x.ContentId == like.ContentId);

            if (liked == null)
            {
                if (disLiked != null)
                {
                    dbContext.Dislike.Remove(disLiked);
                    var dContent = await dbContext.Contents.FindAsync(like.ContentId);
                    dContent.DislikeCount = (dContent.DislikeCount ?? 0) - 1;
                }
                dbContext.Like.Add(new Like { UserId = like.UserId, ContentId = like.ContentId });
                var content = await dbContext.Contents.FindAsync(like.ContentId);
                content.LikeCount = (content.LikeCount ?? 0) + 1;
                await dbContext.SaveChangesAsync();
                return like;
            }
            return null;

        }

        public async Task<DisLike?> AddDisLikeAsync(DisLike disLike)
        {
            var disLiked = await dbContext.Dislike.FirstOrDefaultAsync(x => x.UserId == disLike.UserId && x.ContentId == disLike.ContentId);
            var liked = await dbContext.Like.FirstOrDefaultAsync(x => x.UserId == disLike.UserId && x.ContentId == disLike.ContentId);


            if (disLiked == null)
            {
                if(liked != null)
                {
                    dbContext.Like.Remove(liked);
                    var dContent = await dbContext.Contents.FindAsync(disLike.ContentId);
                    dContent.LikeCount = (dContent.LikeCount ?? 0) - 1;
                }

                dbContext.Dislike.Add(new DisLike { UserId = disLike.UserId, ContentId = disLike.ContentId });
                var content = await dbContext.Contents.FindAsync(disLike.ContentId);
                content.DislikeCount = (content.DislikeCount ?? 0) + 1;
                await dbContext.SaveChangesAsync();
                return disLike;
            }
            return null;
        }

        public async Task<IEnumerable<Content>> GetAllAsync()
        {
            return await dbContext.Contents.Include(X => X.Genres).ToListAsync();
        }

        public async Task<Content?> GetByIDAsync(Guid id)
        {
            return await dbContext.Contents.Include(X => X.Genres).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<IEnumerable< Content?>> GetByGenreAsync(Guid genreId)
        {
            var items = await dbContext.Contents
                         .Include(x => x.Genres)
                         .Where(x => x.Genres.Any(g => g.Id == genreId))
                         .ToListAsync();
            return items;
        }

        public async Task<IEnumerable<Content?>> GetByCategoryAsync(Guid categoryId)
        {
            var items = await dbContext.Contents
                         .Include( x => x.Genres)
                         .Where(x => x.CategoryId == categoryId)
                         .ToListAsync();
            return items;
        }


        public async Task<IEnumerable<Content?>> GetByLikesAsync()
        {
            return await dbContext.Contents.Include(g =>g.Genres).OrderByDescending(x => x.LikeCount ?? 0).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Content?>> GetByFavouritesAsync(string userId)
        {
            return await dbContext.Like.Where(x =>x.UserId == userId)
                .Include(c => c.Content).ThenInclude(g => g.Genres)
                .Select( x => x.Content)
                .ToListAsync();
        }

        public async Task<Content?> UpdateAsync(Content content)
        {
            var existingContents = await dbContext.Contents.Include(x => x.Genres)
                 .FirstOrDefaultAsync(x => x.Id == content.Id);

            if (existingContents == null)
            {
                return null;
            }

            // Update Contents
            dbContext.Entry(existingContents).CurrentValues.SetValues(content);

            // Update Genres
            existingContents.Genres = content.Genres;

            await dbContext.SaveChangesAsync();

            return content;
        }

        public async Task<Content?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.Contents.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                dbContext.Contents.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }
    }
}
