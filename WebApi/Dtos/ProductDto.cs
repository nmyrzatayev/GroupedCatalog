namespace WebApi.Dtos
{
    public class ProductDto
    {
        public required string Name { get; set; }
        public required string Unit { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
