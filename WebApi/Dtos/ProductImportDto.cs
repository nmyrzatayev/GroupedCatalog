namespace WebApi.Dtos
{
    public class ProductImportDto
    {
        public string? Name { get; set; }
        public string? Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
