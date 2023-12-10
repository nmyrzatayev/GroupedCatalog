using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Стандартные CRUD-ы для Группы товаров для манипуляции с БД
    /// </summary>
    public interface IGroupRepository
    {
        Task Add(Group group);
        Task Update(Group group);
        Task<IEnumerable<Group>> GetAll();
        Task Delete(Group group);
        Task<Group?> GetById(int id);
    }
}
