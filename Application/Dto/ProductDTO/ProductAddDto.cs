using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.SubCategoryDTO;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.ProductDTO;

public class ProductAddDto
{
    public string Name { get; set; }

    public string SubcategoryId { get; set; }

    public bool Status { get; set; }
}

public class ProductDtoProfile : Profile
{
    public ProductDtoProfile()
    {
        CreateMap<ProductAddDto, Product>();
    }
}
