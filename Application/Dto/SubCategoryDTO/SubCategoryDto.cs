using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.SubCategoryDTO;

public class SubCategoryDto
{
    public string Name { get; set; }

    public string CategoryId { get; set; }
}

public class SubCategoryDtoProfile : Profile
{
    public SubCategoryDtoProfile()
    {
        CreateMap<SubCategoryDto, SubCategory>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
    }
}
