using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.CartDTO;

public class CartDto
{
    public bool Status { get; set; }
}

public class CartDtoProfile : Profile
{
    public CartDtoProfile()
    {
        CreateMap<CartDto, Cart>();
    }
}
