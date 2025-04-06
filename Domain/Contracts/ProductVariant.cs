using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Contracts
{
    [Table("ProductVariants")]
    public class ProductVariant : BaseEntity<string>
    {
        public string ProductId { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }
    }
}
