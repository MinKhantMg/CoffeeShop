using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.ProductVariantDTO;

public class ProductVariantDto
{
    public string Name { get; set; }

    public string ProductId { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public int? Calorie { get; set; }

    public int Price { get; set; }

}

public class ProductVariantDtoProfile : Profile
{
    public ProductVariantDtoProfile()
    {
        CreateMap<ProductVariantDto, ProductVariant>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
          .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
          .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
          .ForMember(dest => dest.Calorie, opt => opt.MapFrom(src => src.Calorie))
          .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));
    }
}