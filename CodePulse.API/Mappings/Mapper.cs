
using AutoMapper;
using CodePulse.API.Model.Domain;
using CodePulse.API.Model.DTO;

namespace CodePulse.API.Mappings
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
