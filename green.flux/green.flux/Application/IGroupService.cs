using green.flux.Domain;

namespace green.flux.Application
{
	public interface IGroupService
	{
		Task<Group> CreateGroupAsync(Group group);
		Task UpdateGroupAsync(Group group);
		Task DeleteGroupAsync(Guid groupId);
		Task<Group> GetGroupByIdAsync(Guid groupId);

	}
}