using FluentValidation;
using green.flux.Application;
using green.flux.Domain;
using green.flux.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class ChargeStationsController : ControllerBase
	{
		private readonly IChargeStationService _chargeStationService;
		private readonly IValidator<ChargeStation> _validator;
		//we can use ILog if we needs but I decided not use here because of overengineering
		public ChargeStationsController(IChargeStationService chargeStationService, IValidator<ChargeStation> validator)
		{
			_chargeStationService = chargeStationService;
			_validator = validator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateChargeStation([FromBody] ChargeStation chargeStation)
		{
			var validationResult = await _validator.ValidateAsync(chargeStation);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);
			try
			{
				var result = await _chargeStationService.CreateChargeStationAsync(chargeStation);
				return Ok(result);
			}
			catch (Exception ex)
			{   // Log the exception details here if necessary (ILog),also we can use custom exception like ConnectorController
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateChargeStation([FromBody] ChargeStation chargeStation)
		{
			var validationResult = await _validator.ValidateAsync(chargeStation);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			try
			{
				await _chargeStationService.UpdateChargeStationAsync(chargeStation);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteChargeStation(Guid id)
		{
			try
			{
				await _chargeStationService.DeleteChargeStationAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<ChargeStation>> GetChargeStation(Guid id)
		{
			try
			{
				var chargeStation = await _chargeStationService.GetChargeStationByIdAsync(id);
				if (chargeStation == null)
				{
					return NotFound();
				}
				return Ok(chargeStation);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
