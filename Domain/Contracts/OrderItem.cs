using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("OrderItems")]
    public class OrderItem : BaseEntity<string>
    {
        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal TotalPrice { get; set; }

        public string OrderId { get; set; }

        public string ProductVariantId { get; set; }
    }
}
