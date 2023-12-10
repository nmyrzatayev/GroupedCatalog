using Domain.Entities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    /// <summary>
    /// Стандартные CRUD-ы для связки Товаров и Групп товаров для манипуляции с БД
    /// </summary>
    public interface IProductGroupRepository
    {
        Task <IEnumerable<ProductGroup>> GetAll();
        Task Add(ProductGroup productGroup);
    }
}
