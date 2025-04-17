using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Enums;

namespace Application.Dto.OrderDTO;

public class OrderDto
{
    public string Id { get; set; }
    public int TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public string OrderType { get; set; } = Enum.GetName(typeof(OrderType), Domain.Enums.OrderType.DineIn);
    public string PaymentType { get; set; } = Enum.GetName(typeof(PaymentType), Domain.Enums.PaymentType.Cash);
}

public class OrderDtoProfile : Profile
{
    public OrderDtoProfile()
    {
        CreateMap<OrderDto, Order>();
    }

}
