using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;
using System.Text.Json;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentRepository contentRepository;
        private readonly IGenreRepository genreRepository;
        private readonly ILogger<ContentController> logger;

        public ContentController(IContentRepository contentRepository, IGenreRepository genreRepository, ILogger<ContentController> logger)
        {
            this.contentRepository = contentRepository;
            this.genreRepository = genreRepository;
            this.logger = logger;

            //Testing Log Levels
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Info Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");
        }

        // POST: {apibaseurl}/api/blogposts
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateContent([FromBody] CreateContentDTO request)
        {
            //_logger.LogInformation("Initiating Content Creation");

            try
            {
                // Convert DTO to DOmain
                var content = new Content
                {
                    Title = request.Title,
                    Description = request.Description,
                    FeaturedImageUrl = request.FeaturedImageUrl,
                    TrailerUrl = request.TrailerUrl,
                    Info = request.Info,
                    PublishedDate = request.PublishedDate,
                    RentalDuration = request.RentalDuration,
                    IsExpired = request.IsExpired,
                    LikeCount = request.LikeCount,
                    DislikeCount = request.DislikeCount,
                    CategoryId = request.CategoryId,
                    Genres = new List<Genre>()
                };


                foreach (var genreGuid in request.Genres)
                {
                    var existingGenres = await genreRepository.GetByIdAsync(genreGuid);
                    if (existingGenres is not null)
                    {
                        content.Genres.Add(existingGenres);
                    }
                }

                content = await contentRepository.CreateAsync(content);

                //_logger.LogInformation($"Created content: {content}");
                // Convert Domain Model back to DTO
                var response = new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl = content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                   // CategoryName = content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An error occurred while retrieving top content at {Time}", DateTime.UtcNow);
                return StatusCode(500, "Internal Server Error");
            }
            
        }

        // POST: {apibaseurl}/api/Like
        [HttpPost]
        [Route("/like")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddLike(Guid contentId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var content = new Like
            {
               UserId = userId,
               ContentId = contentId,
            };

             await contentRepository.AddLikeAsync(content);            

            return Ok("Liked");
        }

        // POST: {apibaseurl}/api/Dislike
        [HttpPost]
        [Route("/Dislike")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> AddDisLike(Guid contentId)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Convert DTO to DOmain
            var content = new DisLike
            {
                UserId = userId,
                ContentId = contentId,
            };

            await contentRepository.AddDisLikeAsync(content);

            return Ok("DisLiked");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {       
            //throw new Exception("This is a custom Error for Testing");

            var contents = await contentRepository.GetAllAsync();

            //Convert Domain Model to DTO

            var response = new List<ContentDTO>();
            foreach (var content in contents)
            {
                response.Add(new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl = content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                    CategoryName = content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()

                });

            }
            return Ok(response);

        }

        [HttpGet]
        [Route("/MostLiked")]
        public async Task<IActionResult> GetMostLiked()
        {
           // string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contents = await contentRepository.GetByLikesAsync();

            //Convert Domain Model to DTO

            var response = new List<ContentDTO>();
            foreach (var content in contents)
            {
                response.Add(new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl=content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                    CategoryName=content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()

                });

            }
            return Ok(response);

        }

        [HttpGet]
        [Route("/Favourites")]
        public async Task<IActionResult> GetMyFavourite()
        {
             string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var contents = await contentRepository.GetByFavouritesAsync(userId);

            //Convert Domain Model to DTO

            var response = new List<ContentDTO>();
            foreach (var content in contents)
            {
                response.Add(new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl = content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                    CategoryName = content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()

                });

            }
            return Ok(response);

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContentByID([FromRoute] Guid id)
        {
            // Get the BlogPost from Repo
            var content = await contentRepository.GetByIDAsync(id);

            if (content is null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            var response = new ContentDTO
            {
                Id = content.Id,
                Title = content.Title,
                Description = content.Description,
                FeaturedImageUrl = content.FeaturedImageUrl,
                TrailerUrl = content.TrailerUrl,
                PublishedDate = content.PublishedDate,
                Info = content.Info,
                RentalDuration = content.RentalDuration,
                IsExpired = content.IsExpired,
                LikeCount = content.LikeCount,
                DislikeCount = content.DislikeCount,
                CategoryId = content.CategoryId,
                CategoryName= content.Category.Name,
                Genres = content.Genres.Select(x => new GenreDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList()
            };

            return Ok(response);
        }


        [HttpGet]
        [Route("/GenreWise")]
        public async Task<IActionResult> GetContentsByGenre( Guid genreId)
        {
            var contents = await contentRepository.GetByGenreAsync(genreId);

            //Convert Domain Model to DTO

            var response = new List<ContentDTO>();
            foreach (var content in contents)
            {
                response.Add(new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl = content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                    CategoryName = content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()

                });

            }
            return Ok(response);

        }


        [HttpGet]
        [Route("/CategoryWise")]
        public async Task<IActionResult> GetContentsBycategory(Guid categoryId)
        {
            var contents = await contentRepository.GetByCategoryAsync(categoryId);

            //Convert Domain Model to DTO

            var response = new List<ContentDTO>();
            foreach (var content in contents)
            {
                response.Add(new ContentDTO
                {
                    Id = content.Id,
                    Title = content.Title,
                    Description = content.Description,
                    FeaturedImageUrl = content.FeaturedImageUrl,
                    TrailerUrl = content.TrailerUrl,
                    PublishedDate = content.PublishedDate,
                    Info = content.Info,
                    RentalDuration = content.RentalDuration,
                    IsExpired = content.IsExpired,
                    LikeCount = content.LikeCount,
                    DislikeCount = content.DislikeCount,
                    CategoryId = content.CategoryId,
                    CategoryName = content.Category.Name,
                    Genres = content.Genres.Select(x => new GenreDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList()

                });

            }
            return Ok(response);

        }


        // PUT: {apibaseurl}/api/blogposts/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateContenttById([FromRoute] Guid id, UpdateContentRequestDTO request)
        {
            // Convert DTO to Domain Model
            var content = new Content
            {
                Id = id,           
                Title = request.Title,
                Description = request.Description,
                FeaturedImageUrl = request.FeaturedImageUrl,
                TrailerUrl= request.TrailerUrl,
                Info = request.Info,
                PublishedDate = request.PublishedDate,
                RentalDuration = request.RentalDuration,
                IsExpired = request.IsExpired,
                LikeCount = request.LikeCount,
                DislikeCount = request.DislikeCount,
                CategoryId = request.CategoryId,
                Genres = new List<Genre>()
            };

            // Foreach 
            foreach (var genreGuid in request.Genres)
            {
                var existingGenres = await genreRepository.GetByIdAsync(genreGuid);

                if (existingGenres != null)
                {
                    content.Genres.Add(existingGenres);
                }
            }


            // Call Repository To Update BlogPost Domain Model
            var updatedContents = await contentRepository.UpdateAsync(content);

            if (updatedContents == null)
            {
                return NotFound();
            }

            // Convert Domain model back to DTO
            var response = new ContentDTO
            {
                Id = content.Id,
                Title = content.Title,
                Description = content.Description,
                FeaturedImageUrl = content.FeaturedImageUrl,
                TrailerUrl = content.TrailerUrl,
                PublishedDate = content.PublishedDate,
                Info = content.Info,
                RentalDuration = content.RentalDuration,
                IsExpired = content.IsExpired,
                LikeCount = content.LikeCount,
                DislikeCount = content.DislikeCount,
                CategoryId = content.CategoryId,
                Genres = content.Genres.Select(x => new GenreDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList()
               
            };

            return Ok(response);
        }

        // DELETE: {apibaseurl}/api/blogposts/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteContent([FromRoute] Guid id)
        {
            var deletedContent = await contentRepository.DeleteAsync(id);

            if (deletedContent == null)
            {
                return NotFound();
            }

            // Convert Domain model to DTO
            var response = new ContentDTO
            {
                Id = deletedContent.Id,
                Title = deletedContent.Title,
                Description = deletedContent.Description,
                FeaturedImageUrl = deletedContent.FeaturedImageUrl,
                PublishedDate = deletedContent.PublishedDate,
                Info =  deletedContent.Info,
                RentalDuration = deletedContent.RentalDuration,
                IsExpired = deletedContent.IsExpired,
                LikeCount = deletedContent.LikeCount,
                DislikeCount = deletedContent.DislikeCount,
                CategoryId=deletedContent.CategoryId,
                Genres = deletedContent.Genres.Select(x => new GenreDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList()
            };

            return Ok(response);
        }
    }
}
