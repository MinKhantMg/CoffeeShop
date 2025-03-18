using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.UserDTO;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.CategoryDTO;

public class CategoryAddDto
{
    public string Name { get; set; }

    public string Slug { get; set; }

}

public class CategoryDtoProfile : Profile
{
    public CategoryDtoProfile()
    {
        CreateMap<CategoryAddDto, Category>()
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
           .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug));
    }
}

