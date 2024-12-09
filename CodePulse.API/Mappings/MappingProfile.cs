
using AutoMapper;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;

namespace CodePulse.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category Mapping
            CreateMap<Category, CategoryDTO>()
                .ForSourceMember(src => src.Contents, opt => opt.DoNotValidate()) // Ignore Contents for forward mapping
           .ReverseMap()
                .ForMember(dest => dest.Contents, opt => opt.Ignore());// Ignore Contents for reverse mapping
           
            CreateMap<CreateCategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.Ignore());

            CreateMap<UpdateCategoryDTO, Category>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Contents, opt => opt.Ignore());
            #endregion

            #region Genre Mapping

            CreateMap<CreateGenreDTO, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest=> dest.Contents, opt => opt.Ignore());

            CreateMap<Genre, GenreDTO>()
                .ForSourceMember(source => source.Contents, opt => opt.DoNotValidate());

            #endregion

            #region Watchlist Mapping
            // Map Content to ContentDTO
            CreateMap<Watchlist, WatchListDTO>()
                .ForMember(dest => dest.Contents, opt => opt.MapFrom(src => src.Content != null ? new List<Content> { src.Content} : new List<Content>()));

            #endregion

            #region Content Mapping
            CreateMap<Content, ContentDTO>()
                .ForSourceMember(src => src.Category, opt => opt.DoNotValidate());

            // Map CreateContentDTO to Content
            CreateMap<CreateContentDTO, Content>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.Genres.Select(genreId => new Genre { Id = genreId }).ToList())) // Map Genre IDs to Genre objects
                .ForMember(dest => dest.Category, opt => opt.Ignore()); // Ignore navigation properties not present in DTO

            // Map Content to ContentDTO
            CreateMap<Content, ContentDTO>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)) // Map Category name
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.Genres.Select(genre => new GenreDTO
                    {
                        Id = genre.Id,
                        Name = genre.Name
                    }).ToList())); // Map Genre objects to GenreDTO list
            #endregion

        }
    }
}
