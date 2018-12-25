using System;
using System.Collections.Generic;
using Logic.Models;
using Logic.Utils;
using System.Linq;
using System.Text.RegularExpressions;
using Logic.Dtos;

namespace Logic.Services
{
    public class UserService : IUserService
    {
        private DataDbContext _context;

        public UserService(DataDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(long id)
        {
            return _context.Users.Find(id);
        }

        public Result ValidateCreateUserDto(UserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Password))
                return Result.Fail("Password is required");

            if (userDto.Password.Length > 100)
                return Result.Fail("Password is too long");

            if (!string.IsNullOrWhiteSpace(userDto.Username) && userDto.Username.Length > 150)
                return Result.Fail("User name is too long");

            if (!string.IsNullOrWhiteSpace(userDto.FirstName) && userDto.FirstName.Length > 150)
                return Result.Fail("First name is too long");

            if (!string.IsNullOrWhiteSpace(userDto.LastName) && userDto.LastName.Length > 150)
                return Result.Fail("Last name is too long");

            if (string.IsNullOrWhiteSpace(userDto.Email))
                return Result.Fail("Email is required");

            if (userDto.Email.Length > 150)
                return Result.Fail("Email is too long");

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(userDto.Email);
            if (!match.Success)
                return Result.Fail("Email format is incorrect");

            if (_context.Users.Any(x => x.Email == userDto.Email))
                return Result.Fail("Email \"" + userDto.Email + "\" is already taken");
            
            return Result.Ok();
        }

        public User Create(User user, string password)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void Update(User userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new Exception("Username " + userParam.Username + " is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(long id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }


        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
