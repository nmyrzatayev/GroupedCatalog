using AutoMapper;

using Domain.Entities;
using Domain.Repository;

using OfficeOpenXml;

using WebApi.Dtos;


namespace WebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductGroupRepository _productGroupsRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IProductGroupRepository productGroupsRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _productGroupsRepository = productGroupsRepository;
        }

        public async Task<List<string>> ImportProducts(IFormFile file)
        {
            using (var stream = file.OpenReadStream())
            {
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    // Предполагается, что данные начинаются с первой строки и первой колонки
                    int rowCount = worksheet.Dimension.Rows;

                    var errors = new List<string>();

                    for (var i = 2; i <= rowCount; i++)
                    {
                        try
                        {
                            var productImportDto = new ProductImportDto();
                            productImportDto.Name = worksheet.Cells[i, 1].Value?.ToString() ?? throw new Exception("1");
                            productImportDto.Unit = worksheet.Cells[i, 2].Value?.ToString() ?? throw new Exception("2");
                            productImportDto.Price = Convert.ToDecimal(worksheet.Cells[i, 3].Value?.ToString() ?? throw new Exception("3"));
                            productImportDto.Quantity = Convert.ToInt32(worksheet.Cells[i, 4].Value?.ToString() ?? throw new Exception("4"));
                            var product = _mapper.Map<Product>(productImportDto);

                            var existProduct = await _productRepository.GetByName(productImportDto.Name);
                            if (existProduct is null)
                            {
                                await _productRepository.AddProduct(product);
                            }
                            else
                            {
                                existProduct.Quantity = productImportDto.Quantity; //если файл нужен чтобы обновить товары
                                //existProduct.Quantity += productImportDto.Quantity; //если файл нужен чтобы добавить товары сверху
                                existProduct.Price = productImportDto.Price;
                                await _productRepository.Update(existProduct);
                            }

                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Не удалось прочитать в строке {i} столбец {ex.Message}");
                        }
                    }

                    return errors;
                }
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductByGroupId(int groupId)
        {
            var productGroups = await _productGroupsRepository.GetAll();
            productGroups = productGroups.Where(pg=>pg.GroupId==groupId);

            //в примере задачи вывод товаров по убываю цены за единицу
            productGroups = productGroups.OrderByDescending(pg => pg.Product.Price).ToList();

            var productDtoList = new List<ProductDto>();
            foreach (var productGroup in productGroups)
            {
                var productDto = _mapper.Map<ProductDto>(productGroup.Product);
                productDto.Quantity = productGroup.Quantity;
                productDtoList.Add(productDto);
            }
            return productDtoList;
        }
    }
}
