using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.UserDTO;
using AutoMapper.Execution;

namespace Application.Logic.UserService
{
    public interface IUserService
    {
        Task<int> RegisterUserAsync(UserRegistrationDto userDto);

    }
}
