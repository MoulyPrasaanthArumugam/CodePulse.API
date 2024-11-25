using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListRepository watchListRepository;
        public WatchListController(IWatchListRepository watchListRepository)
        {
            this.watchListRepository = watchListRepository;
        }

        // POST: {apibaseurl}/api/Watchlist
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddToWatchList(Guid contentId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var watchList = new Watchlist
            {
                UserId = userId,
                ContentId = contentId
            };

           watchList = await watchListRepository.CreateAsync(watchList);

            // Convert Domain Model back to DTO
            var response = new WatchListDTO
            {
               Id = watchList.Id,
               UserId = watchList.UserId,
               ContentId = watchList.ContentId,
            };

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWatchListItems()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var watchlists = await watchListRepository.GetAllAsync(userId);

            //Convert Domain Model to DTO

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
                             PublishedDate = watchlist.Content.PublishedDate,
                             Info = watchlist.Content.Info,
                             RentalDuration = watchlist.Content.RentalDuration,
                             IsExpired = watchlist.Content.IsExpired,
                             LikeCount = watchlist.Content.LikeCount,
                             DislikeCount = watchlist.Content.DislikeCount,
                                Genres = watchlist.Content.Genres.Select(genre => new GenreDTO
                                    {
                                         Id = genre.Id,
                                         Name = genre.Name
                                    }).ToList()
                         }
                     }

                });

            }
            return Ok(response);

        }


        // DELETE: {apibaseurl}/api/WatchList/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> RemoveFromWatchList([FromRoute] Guid contentId)
        {
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
            var response = new WatchListDTO
            {
                Id=deletedContent.Id,
                UserId=deletedContent.UserId,
                ContentId = deletedContent.ContentId
            };

            return Ok(response);
        }

    }
}
