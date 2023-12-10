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
    public class ProductGroupRepository:IProductGroupRepository
    {
        private readonly MainDbContext _mainDbContext;

        public ProductGroupRepository(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        public async Task<IEnumerable<ProductGroup>> GetAll()
        {
            return await _mainDbContext.ProductGroups
                .Include(pg=>pg.Group)
                .Include(pg=>pg.Product).ToListAsync();
        }

        public async Task Add(ProductGroup productGroup)
        {
            _mainDbContext.ProductGroups.Add(productGroup);
            await _mainDbContext.SaveChangesAsync();
        }

    }
}
