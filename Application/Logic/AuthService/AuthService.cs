using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Dto;
using Application.Dto.UserDTO;
using AutoMapper;
using Domain.Contracts;
using Infrastructure.GenericRepository;
using Infrastructure.Repository.Interface;
using Infrastructure.UnitOfWork;

namespace Application.Logic.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponse> LoginUserAsync(UserLoginDto userLoginDto)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(userLoginDto.Email))
                {
                    throw new ArgumentException("Email cannot be empty.", nameof(userLoginDto.Email));
                }

                var user = await _userRepository.FindByEmailAsync(userLoginDto.Email);


                if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
                    throw new UnauthorizedAccessException("Invalid email or password.");


                var claims = new List<Claim>
                {
                     new Claim(ClaimTypes.NameIdentifier, user.Id),
                     new Claim(ClaimTypes.Email, user.Email),
                     new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                var accessToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = _tokenService.GenerateRefreshToken();
                var expiryDate = DateTime.UtcNow.AddDays(7);

                _userRepository.UpdateRefreshToken(user.Id, refreshToken, expiryDate);

                return new AuthResponse
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    StatusCode = 200,
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error login user: {ex.Message}");
                return new AuthResponse
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Invalid email or password."
                };
            }
        }


        public async Task<int> RefreshToken(AuthResponse tokenRequest)
        {

            if (string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return 401;
            }

            // Get user by refresh token
            var user = await _userRepository.GetUserByRefreshToken(tokenRequest.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return 401;
            }

            // Generate new tokens
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var newAccessToken = _tokenService.GenerateAccessToken(claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // Update refresh token in Users table
            await _userRepository.UpdateRefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

            return 200;

        }
    }
}
