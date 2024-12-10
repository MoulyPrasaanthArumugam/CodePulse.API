using AutoMapper;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper mapper;
        private readonly ILogger<GenreController> logger;

        public GenreController(IGenreRepository genreRepository, IMapper mapper, ILogger<GenreController> logger) 
        { 
           _genreRepository = genreRepository;
            this.logger = logger;
            this.mapper = mapper;
        }


        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> SaveGenre(CreateGenreDTO request)
        {
            //Map DTO to Domain Model 
            string serializedRequest = JsonSerializer.Serialize(request);

            logger.LogInformation($"InsertGenre:{serializedRequest}");

            var genre = new Genre()
            {
                Name = request.Name
            };

            logger.LogInformation($"Genre:{request.Name}");

            await _genreRepository.CreateAsync(genre);

            //Map Domain Model to DTO
            var response = new GenreDTO()
            {
                Id = genre.Id,
                Name = request.Name
            };
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGenreAsynch()
        {
            logger.LogInformation($"Get All Genre:");
            var genres = await _genreRepository.GetAllAsync();
            var response = mapper.Map<List<GenreDTO>>(genres);
            
            string dataretrivalGenre = JsonSerializer.Serialize(response);

            logger.LogInformation($"data Retrival Genre:{dataretrivalGenre}");

            return Ok(response);
        }       

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetGenreByID([FromRoute] Guid id)
        {
            logger.LogInformation($"Get Genre ByID:");
            var genre = await _genreRepository.GetByIdAsync(id);

            if (genre is null)
            {
                return NotFound();
            }
            var response = mapper.Map<GenreDTO>(genre);

            string DatabyIDGenre = JsonSerializer.Serialize(response);

            logger.LogInformation($"data Retrival byID Genre:{DatabyIDGenre}");
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditGenre([FromRoute] Guid id, UpdateGenreDTO request)
        {
            string serializedRequestbyID = JsonSerializer.Serialize(id);
            string serializedRequest = JsonSerializer.Serialize(request);

            logger.LogInformation($"data Retrival byID Genre:{serializedRequestbyID},{serializedRequestbyID}");
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
            string UpdatedGenre = JsonSerializer.Serialize(response);

            logger.LogInformation($"Updated Genre:{UpdatedGenre}");
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            string deleteGenre = JsonSerializer.Serialize(id);
            logger.LogInformation($"Delete Genre :{deleteGenre}");
            var genre = await _genreRepository.DeleteAsync(id);
            if (genre is null)
            {
                return NotFound();
            }
            var response = mapper.Map<GenreDTO>(genre);
            string DeletedGenre = JsonSerializer.Serialize(response);

            logger.LogInformation($"Updated Genre:{DeletedGenre}");
            return Ok(response);
        }

    }
}
