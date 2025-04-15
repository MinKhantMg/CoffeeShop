using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.ProductVariantDTO;

public class ProductVariantDto
{
    public string Name { get; set; }

    public string ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int? Price { get; set; }

}

public class ProductVariantDtoProfile : Profile
{
    public ProductVariantDtoProfile()
    {
        CreateMap<ProductVariantDto, ProductVariant>();
    }
}