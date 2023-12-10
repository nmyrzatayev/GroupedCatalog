using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IProductService
    {
        Task<List<string>> ImportProducts(IFormFile file);
        Task<IEnumerable<ProductDto>> GetProductByGroupId(int groupId);
    }
}
