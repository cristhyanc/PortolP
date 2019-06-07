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


        [HttpGet("GetDeliveryDriverInfo")]
        public IActionResult GetDeliveryDriverInfo([FromQuery] Guid deliveryID)
        {
            try
            {

                var result = _deliveryService.GetDeliveryDriverInfo(deliveryID);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.GetDeliveryDriverInfo");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("GetPendingReceiverDeliveries")]
        public IActionResult GetPendingReceiverDeliveries([FromQuery] Guid receiverID)
        {
            try
            {

                var result = _deliveryService.GetPendingReceiverDeliveries(receiverID);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.GetPendingReceiverDeliveries");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("GetSendertDeliveryInProgress")]
        public IActionResult GetSendertDeliveryInProgress([FromQuery] Guid customerID)
        {
            try
            {

                var result = _deliveryService.GetSendertDeliveryInProgress(customerID);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.GetSendertDeliveryInProgress");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("ConfirmDeliveryPickUp")]
        public IActionResult ConfirmDeliveryPickUp([FromQuery] Guid deliveryId)
        {
            try
            {

                 _deliveryService.ConfirmDeliveryPickUp(deliveryId);
                return Ok(true);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.ConfirmDeliveryPickUp");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("GetDeliveryStatus")]
        public IActionResult GetDeliveryStatus([FromQuery] Guid deliveryId)
        {
            try
            {
                var result=_deliveryService.GetDeliveryStatus(deliveryId);
                return Ok(result);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.GetDeliveryStatus");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("RateDelivery")]
        public IActionResult RateDelivery([FromQuery] Guid deliveryId, [FromQuery] int rate)
        {
            try
            {
                _deliveryService.RateDelivery(deliveryId, rate);
                return Ok(true);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.RateDelivery");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }

        [HttpGet("MarkAsDelivered")]
        public IActionResult MarkAsDelivered([FromQuery] Guid deliveryId)
        {
            try
            {
                _deliveryService.MarkAsDelivered(deliveryId);
                return Ok(true);
            }
            catch (AppException ex)
            {
                return BadRequest(new ApiError((int)HttpStatusCode.PreconditionFailed, ex.Message));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "DropoffController.MarkAsDelivered");
                return BadRequest(new ApiError((int)HttpStatusCode.BadRequest, ex.Message));

            }
        }
    }
}