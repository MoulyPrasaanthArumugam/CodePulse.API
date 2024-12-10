using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodePulse.API.Data;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;
using CodePulse.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Drawing.Printing;
using AutoMapper;
using System.Text.Json;
using CodePulse.API.CustomActionFilters;


namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoriesController> logger)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.logger = logger;
        }


        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> SaveCategory(CreateCategoryDTO request)
        {
            string serializedRequest = JsonSerializer.Serialize(request);
            logger.LogInformation($"Insert Category: {serializedRequest}");
            //Map DTO to Domain Model 
            var category = mapper.Map<Category>(request);

            await categoryRepository.CreateCategoryAsynch(category);

            //Map Domain Model to DTO
            var response = mapper.Map<CategoryDTO>(category);

           

            string InsertDESerializedCategory = JsonSerializer.Serialize(response);

            logger.LogInformation($"Inserted Category:{InsertDESerializedCategory}");

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoryAsynch()
        {
            logger.LogInformation("Get All Category  Action Method Started");

            //throw new Exception("This denotes error");
            var categories = await categoryRepository.GetAllCategoriesAsynch();

            //Serialize the Object to JSON for Logging
            string serializedCategories = JsonSerializer.Serialize(categories);

            logger.LogInformation($"Caregories:{serializedCategories}");

            //Map Domain Model to DTO
            var response = mapper.Map<List<CategoryDTO>>(categories);
            
            string GetAllCategory = JsonSerializer.Serialize(response);

            logger.LogInformation($"Get All Category:{GetAllCategory}");
            return Ok(response);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllCategoryAsynch([FromQuery] string? query,
        //    [FromQuery] string? sortBy,
        //    [FromQuery] string? sortDirection, [FromQuery] int? pageNumber,
        //    [FromQuery] int? pageSize)
        //{
        //    var categories = await categoryRepository.GetAllCategoriesAsynch(query,sortBy,sortDirection, pageNumber, pageSize);

        //    var response = new List<CategoryDTO>();
        //    //Map Domain Model to DTO
        //    foreach (var category in categories)
        //    {
        //        response.Add(new CategoryDTO
        //        {
        //            Id = category.Id,
        //            Name = category.Name,
        //            UrlHandle = category.UrlHandle
        //        });
        //    }
        //    return Ok(response);
        //}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryByID([FromRoute] Guid id)
        {
            
            logger.LogInformation("Get Category ById");
            var category = await categoryRepository.GetCategoryByID(id);

            if (category is null)
            {
                return NotFound();
            }
            //Converting Domain model to DTO
            var response = mapper.Map<CategoryDTO>(category);
            string GetAllCategories = JsonSerializer.Serialize(response);

            logger.LogInformation($"Get Category ByID:{GetAllCategories}");

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute] Guid id, UpdateCategoryDTO request)
        {
            var category = new Category()
            logger.LogInformation("Update Category:");

            string serializedreqbyId = JsonSerializer.Serialize(id);
            string serializedreq = JsonSerializer.Serialize(request);

            logger.LogInformation($"Caregories:{serializedreq},{serializedreqbyId}");
            
            var catrgory = new Category()
            {
                Id = id,
                Name = request.Name
            };
            category = await categoryRepository.UpdateAsynch(category);

            if (category is null)
            {
                return NotFound();
            }
            //Converting Domain Model to DTO
           
           
            var response = mapper.Map<CategoryDTO>(category);

            string UpdateCategories = JsonSerializer.Serialize(response);

            logger.LogInformation($"Updated Categories:{UpdateCategories}");
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            string DeleteCategories = JsonSerializer.Serialize(id);

            logger.LogInformation($"Delete Categories:{DeleteCategories}");


            var category = await categoryRepository.DeleteAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            //Converting Domain Model to DTO
            
            
            var response = mapper.Map<CategoryDTO>(category);
            string DeletedCategories = JsonSerializer.Serialize(response);

            logger.LogInformation($"Updated Categories:{DeletedCategories}");
            return Ok(response);
        }

        //// GET: https://localhost:7226/api/categories/count
        //[HttpGet]
        //[Route("count")]
        ////[Authorize(Roles = "Writer")]
        //public async Task<IActionResult> GetCategoriesTotal()
        //{
        //    var count = await categoryRepository.GetCount();

        //    return Ok(count);
        //}
    }
}
