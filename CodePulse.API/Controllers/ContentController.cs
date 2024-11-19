using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Implementations;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentRepository contentRepository;
        private readonly IGenreRepository genreRepository;

        public ContentController(IContentRepository contentRepository, IGenreRepository genreRepository)
        {
            this.contentRepository = contentRepository;
            this.genreRepository = genreRepository;
        }

        // POST: {apibaseurl}/api/blogposts
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateContent([FromBody] CreateContentDTO request)
        {
            // Convert DTO to DOmain
            var content = new Content
            {
                Title = request.Title,
                Description = request.Description,
                FeaturedImageUrl = request.FeaturedImageUrl,
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

            // Convert Domain Model back to DTO
            var response = new ContentDTO
            {
                Id = content.Id,
                Title = content.Title,
                Description = content.Description,
                FeaturedImageUrl = content.FeaturedImageUrl,
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

        [HttpGet]
        public async Task<IActionResult> GetAllContents()
        {
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


        //// GET: {apibaseurl}/api/blogPosts/{urlhandle}
        //[HttpGet]
        //[Route("{urlHandle}")]
        //public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        //{
        //    // Get blogpost details from repository
        //    var blogPost = await blogPostRepository.GetByUrlHandleAsync(urlHandle);

        //    if (blogPost is null)
        //    {
        //        return NotFound();
        //    }

        //    // Convert Domain Model to DTO
        //    var response = new BlogPostDTO
        //    {
        //        Id = blogPost.Id,
        //        Author = blogPost.Author,
        //        Content = blogPost.Content,
        //        FeaturedImageUrl = blogPost.FeaturedImageUrl,
        //        IsVisible = blogPost.IsVisible,
        //        PublishedDate = blogPost.PublishedDate,
        //        ShortDescription = blogPost.Description,
        //        Title = blogPost.Title,
        //        UrlHandle = blogPost.UrlHandle,
        //        Categories = blogPost.categories.Select(x => new CategoryDTO
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            UrlHandle = x.UrlHandle
        //        }).ToList()
        //    };

        //    return Ok(response);
        //}


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
