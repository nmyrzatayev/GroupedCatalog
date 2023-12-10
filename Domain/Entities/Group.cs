using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Описыват группу товаров, который сгенерируется таском
    /// </summary>
    public class Group
    {
        public int Id { get; set; }
        /// <summary>
        /// Название группы (генерируется как "Группа $")
        /// </summary>
        public required string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<ProductGroup> ProductGroups { get; set; }
        public Group()
        {
            ProductGroups = new List<ProductGroup>();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
