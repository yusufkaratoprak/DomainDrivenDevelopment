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
	public class GroupsController : ControllerBase
	{

		private readonly IGroupService _groupService;
		private readonly IChargeStationService _chargeStationService;
		private readonly IConnectorService _connectorService;  // Assuming this service exists
		private readonly IValidator<Group> _validator;

		public GroupsController(IGroupService groupService, IChargeStationService chargeStationService, IConnectorService connectorService, IValidator<Group> validator)
		{
			_groupService = groupService;
			_chargeStationService = chargeStationService;
			_connectorService = connectorService;  // Initialize the connector service
			_validator = validator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateGroup([FromBody] Group group)
		{
			var validationResult = await _validator.ValidateAsync(group);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			if (group.ChargeStations?.Count > 1)
				return BadRequest("A group cannot contain more than one charge station.");

			try
			{
				// Create the group first
				var result = await _groupService.CreateGroupAsync(group);

				// If there's exactly one charge station, create it
				if (group.ChargeStations?.Count == 1)
				{
					var chargeStation = group.ChargeStations.First();
					chargeStation.GroupId = result.ID;  // Set the GroupId to the newly created group's ID
					var stationResult = await _chargeStationService.CreateChargeStationAsync(chargeStation);

					// Check if there are any connectors to create
					if (chargeStation.Connectors != null && chargeStation.Connectors.Count > 0)
					{
						foreach (var connector in chargeStation.Connectors)
						{
							connector.ChargeStationId = stationResult.ID;  // Set the ChargeStationId to the newly created charge station's ID
							await _connectorService.CreateConnectorAsync(connector);
						}
					}
				}

				return Ok(result);
			}
			catch (Exception ex)
			{
				// Log the exception details here if necessary
				return BadRequest(ex.Message);
			}
		}



		[HttpPut]
		public async Task<IActionResult> UpdateGroup([FromBody] Group group)
		{
			var validationResult = await _validator.ValidateAsync(group);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			try
			{
				await _groupService.UpdateGroupAsync(group);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteGroup(Guid id)
		{
			try
			{
				await _groupService.DeleteGroupAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> GetGroup(Guid id)
		{
			try
			{
				var group = await _groupService.GetGroupByIdAsync(id);
				if (group == null)
				{
					return NotFound();
				}
				return Ok(group);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
