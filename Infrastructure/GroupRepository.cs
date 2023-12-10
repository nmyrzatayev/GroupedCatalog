using Domain.Entities;
using Domain.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    /// <summary>
    /// Реализация интерфейса репозитория с помощью MainDbContext
    /// </summary>
    public class GroupRepository: IGroupRepository
    {
        private readonly MainDbContext _dbContext;

        public GroupRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Group group)
        {
            _dbContext.Groups.Add(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Group group)
        {
            _dbContext.Update(group);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Group>> GetAll()
        {
            return await _dbContext.Groups
                .Include(g=>g.ProductGroups)
                .ThenInclude(pg=>pg.Product)
                .ToListAsync();
        }

        public async Task Delete(Group group)
        {
            _dbContext.Groups.Remove(group); 
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Group?> GetById(int id)
        {
            return await _dbContext.Groups
                .Include(g=>g.ProductGroups)
                .ThenInclude(g=>g.Product)
                .FirstOrDefaultAsync(g=>g.Id==id);
        }
    }
}
