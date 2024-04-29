using green.flux.Application;
using green.flux.Domain;
using green.flux.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class ChargeStationsController : ControllerBase
	{
		private readonly IChargeStationService _chargeStationService;

		public ChargeStationsController(IChargeStationService chargeStationService)
		{
			_chargeStationService = chargeStationService;
		}

		[HttpPost]
		public async Task<ActionResult> CreateChargeStation([FromBody] ChargeStation chargeStation)
		{
			await _chargeStationService.CreateChargeStationAsync(chargeStation);
			return Ok(chargeStation);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateChargeStation([FromBody] ChargeStation chargeStation)
		{
			await _chargeStationService.UpdateChargeStationAsync(chargeStation);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteChargeStation([FromBody] ChargeStation chargeStation)
		{
			await _chargeStationService.DeleteChargeStationAsync(chargeStation.ID);
			return NoContent();
		}
		[HttpGet("{id}")]
		public async Task<ActionResult<ChargeStation>> GetChargeStation(Guid id)
		{
			var chargeStation = await _chargeStationService.GetChargeStationByIdAsync(id);
			if (chargeStation == null)
			{
				return NotFound();
			}
			return chargeStation;
		}


	}

}
