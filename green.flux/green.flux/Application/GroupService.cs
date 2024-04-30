using green.flux.Application;
using green.flux.Domain;

namespace green.flux.Infrastructure
{
	public class GroupService : IGroupService
	{
		private readonly IGroupRepository _groupRepository;

		public GroupService(IGroupRepository groupRepository)
		{
			_groupRepository = groupRepository;
		}

		public async Task<Group> CreateGroupAsync(Group group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));
			return await _groupRepository.CreateAsync(group);
		}

		public async Task UpdateGroupAsync(Group group)
		{
			if (group == null)
				throw new ArgumentNullException(nameof(group));
			await _groupRepository.UpdateAsync(group);
		}

		public async Task DeleteGroupAsync(Guid groupId)
		{
			var group = await _groupRepository.GetByIdAsync(groupId);
			if (group == null)
				throw new InvalidOperationException($"Group with ID {groupId} does not exist.");
			await _groupRepository.DeleteAsync(groupId);
		}

		public async Task<Group> GetGroupByIdAsync(Guid groupId)
		{
			return await _groupRepository.GetByIdAsync(groupId);
		}


	}
}
