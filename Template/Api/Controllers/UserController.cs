using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Utils;
using AutoMapper;
using Logic.Dtos;
using Logic.Models;
using Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private IMapper _mapper;
        private readonly Settings _appSettings;

        public UserController(IUserService userService, IMapper mapper, IOptions<Settings> appSettings)
        : base()
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserLoginDto userLoginDto)
        {
            var result = _userService.Authenticate(userLoginDto.Email, userLoginDto.Password);

            if (result.IsFailure)
                return Error(result.Error);

            var user = result.Value;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new UserResultDto
            {
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserRegisterDto userRegisterDto)
        {
            var validationResult = _userService.ValidateCreateUserDto(userRegisterDto);
            if (validationResult.IsFailure)
                return Error(validationResult.Error);

            var user = _mapper.Map<User>(userRegisterDto);
            
            _userService.Create(user, userRegisterDto.Password);

            return Ok();
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserRegisterDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDto = _mapper.Map<UserRegisterDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserRegisterDto userRegisterDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<User>(userRegisterDto);
            user.Id = id;

            try
            {
                // save 
                _userService.Update(user, userRegisterDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        public IActionResult GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            UserResultDto model = new UserResultDto()
            {
                Email = identityClaims.FindFirst("Email").Value,
                FirstName = identityClaims.FindFirst("FirstName").Value,
                LastName = identityClaims.FindFirst("LastName").Value
            };
            return Ok(model);
        }
    }
}