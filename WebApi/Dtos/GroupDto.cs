namespace WebApi.Dtos
{
    public class GroupDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Sum { get; set; }
    }
}
