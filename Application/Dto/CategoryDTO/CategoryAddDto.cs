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

}

public class CategoryDtoProfile : Profile
{
    public CategoryDtoProfile()
    {
        CreateMap<CategoryAddDto, Category>();
    }
}

