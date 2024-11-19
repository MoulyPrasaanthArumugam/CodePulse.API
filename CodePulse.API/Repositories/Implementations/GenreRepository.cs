using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Repositories.Interfaces;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;
using System.Drawing.Text;

namespace CodePulse.API.Repositories.Implementations
{
    public class GenreRepository:IGenreRepository
    {
        private readonly ApplicationDBContext _Context;
        public GenreRepository(ApplicationDBContext applicationDBContext) {
          _Context = applicationDBContext;
        }

        public async Task<Genre> CreateAsync(Genre genre)
        {
            _Context.Genre.Add(genre);
            await _Context.SaveChangesAsync();
            return genre;
        }

        public async Task <IEnumerable<Genre>> GetAllAsync() 
        { 
        return await _Context.Genre.ToListAsync();
        }
       

        public async Task <Genre> GetByIdAsync(Guid Id)
        {
            return await _Context.Genre.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<Genre?> UpdateAsync(Genre genre)
        {
            var existingGenre = await _Context.Genre.FirstOrDefaultAsync(x=>x.Id == genre.Id);
            if (existingGenre != null)
            {
                _Context.Entry(existingGenre).CurrentValues.SetValues(genre);
                await _Context.SaveChangesAsync();
                return genre;
            }
            return null;
        }

        public async Task<Genre?> DeleteAsync(Guid Id)
        {
            var existingGenre = await _Context.Genre.FirstOrDefaultAsync(x=>x.Id == Id);

            if (existingGenre is null)
            {
                return null;
            }
            _Context.Genre.Remove(existingGenre);
            await _Context.SaveChangesAsync();
            return existingGenre;

        }
    }
}
