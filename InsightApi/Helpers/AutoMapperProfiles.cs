using AutoMapper;
using InsightApi.Models;

namespace InsightApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Category, Subcategory>();
        }
    }
}