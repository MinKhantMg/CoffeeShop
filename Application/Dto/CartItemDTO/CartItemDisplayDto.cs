using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.CartItemDTO
{
    public class CartItemDisplayDto
    {
        public string Id { get; set; }
        public string CartId { get; set; }
        public string ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int SubTotal { get; set; }
        public string ProductVariantName { get; set; }
        public string ImageUrl { get; set; }
    }
}
