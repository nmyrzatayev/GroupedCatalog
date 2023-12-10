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
    public class ProductRepository : IProductRepository
    {
        private readonly MainDbContext _dbContext;

        public ProductRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _dbContext.Products
                .Include(p=>p.ProductGroups)
                .ThenInclude(pg=>pg.Group)
                .ToListAsync();
        }

        public async Task Update(Product product)
        {
            product.UpdatedAt = DateTime.UtcNow;
            _dbContext.Update(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _dbContext.Products
                .Include(p => p.ProductGroups)
                .ThenInclude(pg => pg.Group)
                .FirstOrDefaultAsync(p=>p.Id==id);
        }
    }
}
