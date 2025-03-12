using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;

namespace Application.Dto.UserDTO
{
    public class UserLoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

    }

}
