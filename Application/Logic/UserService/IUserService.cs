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

        //Task<int> LoginUserAsync(UserLoginDto userDto);

        //Task<int> Create(MemberDto dto);
        //Task<int> Update(string id, MemberDto dto);
        //Task<IEnumerable<Member>> GetAll();
        //Task<IEnumerable<Member>> Search(string fieldName, string keyword);
        //Task<int> CountAll();
        //Task<Member> GetById(string id);
        //Task<bool> Delete(string id);

    }
}
