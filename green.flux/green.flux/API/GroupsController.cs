using FluentValidation;
using green.flux.Application;
using green.flux.Domain;
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
		private readonly IValidator<Group> _validator;
		//we can use ILog if we needs but I decided not use here because of overengineering
		public GroupsController(IGroupService groupService, IValidator<Group> validator)
		{
			_groupService = groupService;
			_validator = validator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateGroup([FromBody] Group group)
		{
			var validationResult = await _validator.ValidateAsync(group);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			try
			{
				await _groupService.CreateGroupAsync(group);
				return Ok(group);
			}
			catch (Exception ex)
			{
				// Log the exception details here if necessary (ILog),also we can use custom exception like ConnectorController
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
