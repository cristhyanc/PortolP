using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Portol.Common.DTO;
using Portol.Common.Helper;
using Portol.Common.Interfaces.PortolWeb;
using Serilog;

namespace PortolWeb.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DropoffController : ControllerBase
    {
        IDropoffService _dropoffService;

        public DropoffController(IDropoffService dropoffService)
        {
            _dropoffService = dropoffService;
        }

        [HttpGet("GetVehiculeTypesAvailables")]        
        public IActionResult GetVehiculeTypesAvailables()
        {
            try
            {
                var result = _dropoffService.GetVehiculeTypesAvailables();
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.GetVehiculeTypesAvailables");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpPost("CreateDropoffRequest")]
        public IActionResult CreateDropoffRequest([FromBody] DropoffDto dropoff)
        {
            try
            {
                var result = _dropoffService.CreateDropoffRequest(dropoff);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.CreateDropoffRequest");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }
    }
}