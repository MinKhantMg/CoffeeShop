using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.CartItemDTO;

public class CartItemDto
{
    public string CartId { get; set; }

    public string ProductVariantId { get; set; }

    public int Quantity { get; set; }
}

public class CartItemDtoProfile : Profile
{
    public CartItemDtoProfile()
    {
        CreateMap<CartItemDto, CartItem>();
    }
}


