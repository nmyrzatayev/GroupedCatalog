using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Описывает товар
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        /// <summary>
        /// Наименование товара (берется из Excel)
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Наименование единицы измерения для товара (берется из Excel)
        /// </summary>
        public required string Unit { get; set; }
        /// <summary>
        /// Цена в евро (берется из Excel)
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Количество товара (берется из Excel)
        /// </summary>
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<ProductGroup> ProductGroups { get; set; }

        public Product()
        {
            ProductGroups = new List<ProductGroup>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
