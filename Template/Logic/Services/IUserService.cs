using System.Collections.Generic;
using Logic.Dtos;
using Logic.Models;
using Logic.Utils;

namespace Logic.Services
{
    public interface IUserService
    {
        Result<User> Authenticate(string email, string password);
        IEnumerable<User> GetAll();
        User GetById(long id);
        Result ValidateCreateUserDto(UserRegisterDto userRegisterDto);
        User Create(User user, string password);
        void Update(User user, string password = null);
        void Delete(long id);
    }
}
