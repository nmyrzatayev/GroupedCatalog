using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Описывает связку многие-ко-многим с дополнительным свойством
    /// </summary>
    public class ProductGroup
    {
        public int ProductId {  get; set; }
        public required Product Product { get; set; }
        public int GroupId { get; set; }
        public required Group Group { get; set; }

        /// <summary>
        /// дополнительное свойство для указания количества товара в данной группе
        /// </summary>
        public int Quantity { get; set; }
    }
}
