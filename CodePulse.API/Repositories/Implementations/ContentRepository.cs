using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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



        public async Task<IEnumerable<Content>> GetAllAsync()
        {
            return await dbContext.Contents.Include(X => X.Genres).ToListAsync();
        }

        public async Task<Content?> GetByIDAsync(Guid id)
        {
            return await dbContext.Contents.Include(X => X.Genres).FirstOrDefaultAsync(x => x.Id == id);

        }

        //public async Task<BlogSpot?> GetByUrlHandleAsync(string urlHandle)
        //{
        //    return await dbContext.BlogSpot.Include(x => x.categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        //}

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
