using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Globalization;

namespace CodePulse.API.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDBContext _context;

        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsynch(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

       

        public async Task<IEnumerable<Category>> GetAllCategoriesAsynch(string? query = null, string? sortBy = null, string? sortDirection = null, int? pageNumber = 1,
        int? pageSize = 100)
        {
            //return await _context.Category.ToListAsync();
            // Query
            var categories = _context.Category.AsQueryable();

            // Filtering
            if (string.IsNullOrWhiteSpace(query) == false)
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                    ? true : false;


                    categories = isAsc ? categories.OrderBy(x => x.Name) : categories.OrderByDescending(x => x.Name);
                }

                if (string.Equals(sortBy, "URL", StringComparison.OrdinalIgnoreCase))
                {
                    var isAsc = string.Equals(sortDirection, "asc", StringComparison.OrdinalIgnoreCase)
                   ? true : false;

                    categories = isAsc ? categories.OrderBy(x => x.UrlHandle) : categories.OrderByDescending(x => x.UrlHandle);
                }


            }   


            // Pagination
            // Pagenumber 1 pagesize 5 - skip 0, take 5
            // Pagenumber 2 pagesize 5 - skip 5, take 5, [6,7,8,9,10]
            // Pagenumber 3 pagesize 5 - skip 10, take 5

            var skipResults = (pageNumber - 1) * pageSize;
            categories = categories.Skip(skipResults ?? 0).Take(pageSize ?? 100);

            return await categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryByID(Guid id)
        {
            return await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetCount()
        {
            return await _context.Category.CountAsync();
        }
        public async  Task<Category?> UpdateAsynch(Category category)
        {
            var existingCategory = await _context.Category.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (existingCategory != null)
            {
                _context.Entry(existingCategory).CurrentValues.SetValues(category);
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await _context.Category.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCategory is null)
            {
                return null;
                
            }

            _context.Category.Remove(existingCategory);
            await _context.SaveChangesAsync();
            return existingCategory;
        }
    }
}
