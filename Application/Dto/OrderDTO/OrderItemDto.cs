﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.OrderDTO
{
    public class OrderItemDto
    {
        public string ProductVariantName { get; set; }
        public int Quantity { get; set; }
        public int SubTotal { get; set; }
    }
}
