using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CategoryDTO;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.TableDTO
{
    public class TableDto
    {
        public string Name { get; set; }

        public bool Status { get; set; }
    }

    public class TableDtoProfile : Profile
    {
        public TableDtoProfile()
        {
            CreateMap<TableDto, Table>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
        }
    }
}
