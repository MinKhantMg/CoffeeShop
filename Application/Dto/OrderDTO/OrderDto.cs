using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.OrderDTO;

public class OrderDto
{
    public string Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public string PaymentType { get; set; }
}

public class OrderDtoProfile : Profile
{
    public OrderDtoProfile()
    {
        CreateMap<OrderDto, Order>();
    }

}
