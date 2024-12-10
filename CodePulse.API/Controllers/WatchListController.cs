using AutoMapper;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;
using System.Text.Json;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListRepository watchListRepository;
        private readonly IMapper mapper;
        private readonly ILogger<WatchListController> logger;

        public WatchListController(IWatchListRepository watchListRepository, IMapper mapper
            )
        {
            this.watchListRepository = watchListRepository;
            this.mapper = mapper;
            this.logger = logger;

        }

        // POST: {apibaseurl}/api/Watchlist
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddToWatchList(Guid contentId)
        {
            string InsertWatchlist = JsonSerializer.Serialize(contentId);

            logger.LogInformation($"Inserted Watchlist:{InsertWatchlist}");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var watchList = new Watchlist
            {
                UserId = userId,
                ContentId = contentId
            };

           watchList = await watchListRepository.CreateAsync(watchList);

            // Convert Domain Model back to DTO
            var response = mapper.Map<WatchListDTO>(watchList);
            string InsertedWatchlist = JsonSerializer.Serialize(response);

            logger.LogInformation($"Inserted Watchlist:{InsertedWatchlist}");
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWatchListItems()
        {
            logger.LogInformation("Get All Watched Items:");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var watchlists = await watchListRepository.GetAllAsync(userId);

            //Convert Domain Model to DTO
           // var res = mapper.Map<List<Watchlist>>(watchlists);
            var response = new List<WatchListDTO>();
            foreach (var watchlist in watchlists)
            {
                response.Add(new WatchListDTO
                {
                    Id = watchlist.Id,
                    ContentId= watchlist.ContentId,
                    UserId= watchlist.UserId,
                     Contents = new List<ContentDTO>
                     {
                         new ContentDTO
                         {
                             Id = watchlist.Content.Id,
                             Title = watchlist.Content.Title,
                             Description = watchlist.Content.Description,
                             FeaturedImageUrl = watchlist.Content.FeaturedImageUrl,
                             TrailerUrl = watchlist.Content.TrailerUrl,
                             PublishedDate = watchlist.Content.PublishedDate,
                             Info = watchlist.Content.Info,
                             RentalDuration = watchlist.Content.RentalDuration,
                             IsExpired = watchlist.Content.IsExpired,
                             LikeCount = watchlist.Content.LikeCount,
                             DislikeCount = watchlist.Content.DislikeCount,
                             CategoryId = watchlist.Content.CategoryId,
                             CategoryName = watchlist.Content.Category.Name,
                                Genres = watchlist.Content.Genres.Select(genre => new GenreDTO
                                    {
                                         Id = genre.Id,
                                         Name = genre.Name
                                    }).ToList()
                         }
                     }

                });

            }

            string Getwatchlist =  JsonSerializer.Serialize(response);

            logger.LogInformation($"Get  Watchlist:{Getwatchlist}");
            return Ok(response);

        }


        // DELETE: {apibaseurl}/api/WatchList/{id}
        [HttpDelete]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> RemoveFromWatchList( Guid contentId)
        {
            logger.LogInformation($"Remove  Watchlist:{contentId}");
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return BadRequest("User ID not found");
            }
            
            var deletedContent = await watchListRepository.DeleteAsync(userId, contentId);

            if (deletedContent == null)
            {
                return NotFound("Content not found in watchlist.");
            }

            // Convert Domain model to DTO
            var response = mapper.Map<WatchListDTO>(deletedContent);
            string Removedwatchlist = JsonSerializer.Serialize(response);

            logger.LogInformation($"Get  Watchlist:{Removedwatchlist}");
            return Ok(response);
        }

    }
}
