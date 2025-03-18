using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Execution;
using AutoMapper;
using Infrastructure.GenericRepository;
using Infrastructure.UnitOfWork;
using Infrastructure.Repository.Interface;
using Domain.Contracts;
using Application.Dto;
using Application.Dto.UserDTO;

namespace Application.Logic.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnit _unit;
        private readonly IGenericRepository<User, string> _genericRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUnit unit, IUserRepository userRepository, IMapper mapper)
        {
            _unit = unit;
            _genericRepository = _unit.GetRepository<User, string>();
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<int> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userRegistrationDto.Email))
                {
                    throw new ArgumentException("Email cannot be empty.", nameof(userRegistrationDto.Email));
                }

                var existingUser = await _userRepository.FindByEmailAsync(userRegistrationDto.Email);

                if (existingUser != null)
                {
                    Console.WriteLine("Email is already in use.");
                    return -1;
                }

                var user = _mapper.Map<User>(userRegistrationDto);
                user.Id = Guid.NewGuid().ToString().ToUpper();

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.PasswordHash);

                int result = await _genericRepository.Add(user);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering user: {ex.Message}");
                return -1;
            }
        }

        


        //public async Task<User> GetById(string id)
        //{
        //    var user = await _genericRepository.Get(id);
        //    if (user == null)
        //        throw new Exception("Member record does not exist.");
        //    return user;
        //}


    }
}
