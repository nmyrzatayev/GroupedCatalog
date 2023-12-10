using WebApi.Dtos;

namespace WebApi.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupDto>> GetAll();
        Task GenerateGroups(decimal sum);
    }
}
