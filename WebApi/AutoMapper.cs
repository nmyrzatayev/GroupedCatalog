using AutoMapper;

using Domain.Entities;

using WebApi.Dtos;

namespace WebApi
{
    /// <summary>
    /// Для удобства)
    /// </summary>
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Group, GroupDto>();
            CreateMap<ProductImportDto, Product>();
        }
    }
}
