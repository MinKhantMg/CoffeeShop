using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Dto.OrderDTO;

public class CreateOrderDto
{
    public string CartId { get; set; }
    public string PaymentType { get; set; } = Enum.GetName(typeof(OrderType), Domain.Enums.OrderType.DineIn);
    public string OrderType { get; set; } = Enum.GetName(typeof(PaymentType), Domain.Enums.PaymentType.Cash);
}


