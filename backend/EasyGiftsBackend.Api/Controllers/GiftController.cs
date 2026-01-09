using EasyGiftsBackend.Application.Interfaces;
using EasyGiftsBackend.Domain.DTOs.GiftDTOs;
using EasyGiftsBackend.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyGiftsBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GiftController : ControllerBase
    {
        private readonly IGiftsService _giftsService;
        public GiftController(IGiftsService giftsService) 
        {
            _giftsService = giftsService;
        }


        [HttpPost("create")]
        public async Task<ActionResult> CreateGift(GiftDtoCreate giftDto)
        {
            try
            {
                var result = await _giftsService.CreateGift(giftDto);
                return Ok(result);

            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteGift(Guid giftId)
        {
            try
            {
                var result = await _giftsService.DeleteGift(giftId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        [HttpPost("update")]
        public async Task<ActionResult> UpdateGift(GiftDto giftDto)
        {
            try
            {
                var result = await _giftsService.UpdateGift(giftDto);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("getByGiftId")]
        public async Task<ActionResult> GetGiftByGiftId(Guid giftId)
        {
            try
            {
                var result = await _giftsService.GetGiftByGiftId(giftId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("getByUserId")]
        public async Task<ActionResult> GetGiftsByUserId(Guid userId)
        {
            try
            {
                var result = await _giftsService.GetGiftsByUserId(userId);
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("myGifts")]
        public async Task<ActionResult> GetMyGifts()
        {
            try
            {
                var gifts = await _giftsService.GetGiftsCurrentUser();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
