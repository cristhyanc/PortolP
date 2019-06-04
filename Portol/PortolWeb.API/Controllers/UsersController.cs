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
using Portol.Common.DTO;
using Portol.Common.Helper;
using Serilog;
using System.Net;
using Portol.Common;

namespace PortolWeb.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private ICustomerService _userService;
        ISmsService _smsService;
        private readonly AppSettings _appSettings;

        public UsersController(ICustomerService userService, IOptions<AppSettings> appSettings, ISmsService smsService)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
            _smsService = smsService;
        }

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
                Log.Error(ex, "User.Delete");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _userService.GetAll();
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.GetAll");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }


        [AllowAnonymous]
        [HttpPost("VerifyCode")]
        public IActionResult VerifyCode([FromBody]CustomerDto details)
        {
            try
            {


                //if (_userService.ValidateVerificationCode(details.PhoneNumber, details.PhoneCountryCode, Int16.Parse(details.Token)))
                //{
                    return Ok();
                //}
                //else
                //{
                //    return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, StringResources.WrongCode));
                //}

            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.SendVerificationCode");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword([FromBody]CustomerDto details)
        {
            try
            {
                _userService.ResetPassword(details);
                return Ok();

            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.ResetPassword");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }


        [AllowAnonymous]
        [HttpPost("VerifyEmailUniqueness")]
        public IActionResult VerifyEmailUniqueness([FromBody]string email)
        {
            try
            {
                return Ok(_userService.VerifyEmailUniqueness(email));

            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.VerifyEmailUniqueness");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [AllowAnonymous]
        [HttpPost("VerifyMobileUniqueness")]
        public IActionResult VerifyMobileUniqueness([FromBody]CustomerDto phoneDetails)
        {
            try
            {
                return Ok(_userService.VerifyMobileUniqueness(phoneDetails));

            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.VerifyMobileUniqueness");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [AllowAnonymous]
        [HttpPost("SendVerificationCode")]
        public IActionResult SendVerificationCode([FromBody]CustomerDto details)
        {
            try
            {
                _smsService.SendNewCode(details.PhoneNumber, details.PhoneCountryCode);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.SendVerificationCode");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        // GET api/values
        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]CustomerDto userDto)
        {

            try
            {
                var user = _userService.Authenticate(userDto.Email, userDto.Password);               
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.CustomerID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                user.Token = tokenString;
                // return basic user info (without password) and token to store client side
                return Ok(user);
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
            finally
            {
                _userService.Dispose();
            }

        }

        [AllowAnonymous]
        [HttpPost("RegisterNewuser")]
        public IActionResult RegisterNewuser([FromBody]CustomerDto userDto)
        {

            try
            {
                var result = _userService.Create(userDto, userDto.Password);
                return Ok(true);
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
            finally
            {
                _userService.Dispose();
            }

        }


        [HttpPost("SaveUser")]
        public IActionResult SaveUser([FromBody]CustomerDto userDto)
        {

            try
            {
                var result = _userService.SaveCustomer(userDto);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.SaveUser");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [HttpGet("GetCustomerByPhoneNumber")]
        public IActionResult GetCustomerByPhoneNumber([FromQuery]string phoneNumber, [FromQuery]string countryCode)
        {
            try
            {
                long phone;
                int country;

                long.TryParse(phoneNumber, out phone);
                int.TryParse(countryCode, out country);

                var result = _userService.GetCustomerByPhoneNumber(phone, country);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.GetCustomerByPhoneNumber");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [HttpGet("GetCustomer")]
        public IActionResult GetCustomer([FromQuery]Guid customerID)
        {
            try
            {
                var result = _userService.GetCustomer(customerID);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.GetCustomer");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }

        [HttpGet("GetCustomerByEmail")]
        public IActionResult GetCustomerByEmail([FromQuery]string email)
        {
            try
            {
                var result = _userService.GetCustomerByEmail(email);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.GetCustomerByEmail");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }


        [HttpPost("SavePaymentMethod")]
        public IActionResult SavePaymentMethod([FromBody]PaymentMethodDto paymentMethod)
        {

            try
            {
                var result = _userService.SavePaymentMethod(paymentMethod);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "User.SavePaymentMethod");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
            finally
            {
                _userService.Dispose();
            }

        }


    }
}