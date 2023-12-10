using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Стандартные CRUD-ы для Товаров для манипуляции с БД
    /// </summary>
    public interface IProductRepository
    {
        Task Add(Product product);
        Task<IEnumerable<Product>> GetAll();
        Task Update(Product product);
        Task<Product?> GetById(int id);
    }
}
