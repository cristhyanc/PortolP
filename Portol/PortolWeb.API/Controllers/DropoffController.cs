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
        IDeliveryService _deliveryService;

        public DropoffController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("GetVehiculeTypesAvailables")]        
        public IActionResult GetVehiculeTypesAvailables()
        {
            try
            {
                var result = _deliveryService.GetVehiculeTypesAvailables();
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
        public IActionResult CreateDropoffRequest([FromBody] DeliveryDto delivery)
        {
            try
            {

                var result = _deliveryService.CreateDeliveryRequest(delivery);
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