using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Portol.Common.Interfaces.PortolWeb;
using PortolWeb.API.Helper;
using Portol.DTO;
using Portol.Common.Helper;
using Serilog;
using System.Net;

namespace PortolWeb.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
     
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;          
            _appSettings = appSettings.Value;
        }

        // GET api/values
        
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {            
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {

            try
            {
                var user = _userService.Authenticate(userDto.Email, userDto.Password);

                //if (user == null)
                //    return BadRequest(new { message = "Username or password is incorrect" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
                    }),                    
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info (without password) and token to store client side
                return Ok(new
                {
                    Id = user.UserID,
                    Username = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = tokenString
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.Authenticate");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDto userDto)
        {
           
            try
            {              
                var result=_userService.Create(userDto, userDto.Password);
                return Ok(result);
            }
            catch (AppException ex)
            {                
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch(Exception ex)
            {
                Log.Error(ex, "User.Register");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    var users = _userService.GetAll();
        //    var userDtos = _mapper.Map<IList<UserDto>>(users);
        //    return Ok(userDtos);
        //}

        //[HttpGet("{id}")]
        //public IActionResult GetById(int id)
        //{
        //    var user = _userService.GetById(id);
        //    var userDto = _mapper.Map<UserDto>(user);
        //    return Ok(userDto);
        //}

        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody]UserDto userDto)
        //{
        //    // map dto to entity and set id
        //    var user = _mapper.Map<User>(userDto);
        //    user.Id = id;

        //    try
        //    {
        //        // save 
        //        _userService.Update(user, userDto.Password);
        //        return Ok();
        //    }
        //    catch (AppException ex)
        //    {
        //        // return error message if there was an exception
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _userService.Delete(id);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.Register");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }
    }
}