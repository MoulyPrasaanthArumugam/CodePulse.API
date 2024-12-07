using AutoMapper;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper) 
        { 
           _genreRepository = genreRepository;
            this.mapper = mapper;
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> SaveGenre(CreateGenreDTO request)
        {
           //Map DTO to Domain Model 
           var genre = mapper.Map<Genre>(request);
           var savedGenre =  await _genreRepository.CreateAsync(genre);
           //Map Domain Model to DTO
           var response = mapper.Map<GenreDTO>(savedGenre);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenreAsynch()
        {
            var genres = await _genreRepository.GetAllAsync();
            var response = mapper.Map<List<GenreDTO>>(genres);
            return Ok(response);
        }       

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetGenreByID([FromRoute] Guid id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);

            if (genre is null)
            {
                return NotFound();
            }
            var response = mapper.Map<GenreDTO>(genre);

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditGenre([FromRoute] Guid id, UpdateGenreDTO request)
        {
            var genre = new Genre()
            {
                Id = id,
                Name = request.Name
            };
            genre = await _genreRepository.UpdateAsync(genre);

            if (genre is null)
            {
                return NotFound();
            }
            //Converting Domain Model to DTO

            var response = mapper.Map<GenreDTO>(genre);
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var genre = await _genreRepository.DeleteAsync(id);
            if (genre is null)
            {
                return NotFound();
            }
            var response = mapper.Map<GenreDTO>(genre);
            return Ok(response);
        }

    }
}
