using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementations
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly ApplicationDBContext dbContext;

        public BlogPostRepository(ApplicationDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<BlogSpot> CreateAsync(BlogSpot blogPost)
        {
            await dbContext.BlogSpot.AddAsync(blogPost);
            await dbContext.SaveChangesAsync();
            return blogPost;
        }

      

        public async  Task<IEnumerable<BlogSpot>> GetAllAsync()
        {
            return await dbContext.BlogSpot.Include(X=> X.categories).ToListAsync();
        }

        public async Task<BlogSpot?> GetBlogPostByID(Guid id)
        {
            return await dbContext.BlogSpot.Include(X=> X.categories).FirstOrDefaultAsync(x=>x.Id == id);

        }

        public async Task<BlogSpot?> GetByUrlHandleAsync(string urlHandle)
        {
            return await dbContext.BlogSpot.Include(x => x.categories).FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }

        public async Task<BlogSpot?> UpdateAsync(BlogSpot blogPost)
        {
            var existingBlogPost = await dbContext.BlogSpot.Include(x => x.categories)
                 .FirstOrDefaultAsync(x => x.Id == blogPost.Id);

            if (existingBlogPost == null)
            {
                return null;
            }

            // Update BlogPost
            dbContext.Entry(existingBlogPost).CurrentValues.SetValues(blogPost);

            // Update Categories
            existingBlogPost.categories = blogPost.categories;

            await dbContext.SaveChangesAsync();

            return blogPost;
        }

        public async Task<BlogSpot?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await dbContext.BlogSpot.FirstOrDefaultAsync(x => x.Id == id);

            if (existingBlogPost != null)
            {
                dbContext.BlogSpot.Remove(existingBlogPost);
                await dbContext.SaveChangesAsync();
                return existingBlogPost;
            }

            return null;
        }
    }
}
