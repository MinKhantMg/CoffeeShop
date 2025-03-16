using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.UserDTO;
using Microsoft.AspNetCore.Mvc;

namespace Application.Logic.AuthService
{
    public interface IAuthService
    {
        Task<int> LoginUserAsync(UserLoginDto userDto);

        Task<int> RefreshToken(AuthResponse tokenRequest);
    }
}
