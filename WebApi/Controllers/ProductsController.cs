using AutoMapper;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApi.Dtos;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost("ImportProducts")]
        public async Task<IActionResult> ImportProducts(IFormFile file)
        {
            try
            {
                var errors = await _productService.ImportProducts(file);
                if (errors.Count > 0)
                {
                    return Ok(errors);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetProductByGroupId/{groupId}")]
        public async Task<IActionResult> GetProductByGroupId(int groupId)
        {
            try
            {
                var list = await _productService.GetProductByGroupId(groupId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
