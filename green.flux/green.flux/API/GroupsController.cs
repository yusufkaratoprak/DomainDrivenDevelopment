using green.flux.Application;
using green.flux.Domain;
using Microsoft.AspNetCore.Mvc;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class GroupsController : ControllerBase
	{
		private readonly IGroupService _groupService;

		public GroupsController(IGroupService groupService)
		{
			_groupService = groupService;
		}

		[HttpPost]
		public async Task<ActionResult> CreateGroup([FromBody] Group group)
		{
			await _groupService.CreateGroupAsync(group);
			return Ok(group);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateGroup([FromBody] Group group)
		{
			await _groupService.UpdateGroupAsync(group);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteGroup([FromBody] Group group)
		{
			await _groupService.DeleteGroupAsync(group.ID);
			return NoContent();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Group>> GetGroup(Guid id)
		{
			var group = await _groupService.GetGroupByIdAsync(id);
			if (group == null)
			{
				return NotFound();
			}
			return group;
		}


	}

}
