using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.OrderDTO;

public class CreateOrderDto
{
    public string CartId { get; set; }
    public string PaymentType { get; set; }
}


