using green.flux.Application;
using green.flux.Domain;
using System;
using System.Threading.Tasks;

namespace green.flux.Infrastructure
{
	public class ChargeStationService : IChargeStationService
	{
		private readonly IChargeStationRepository _chargeStationRepository;
		private readonly IGroupRepository _groupRepository; // Needed to perform validations

		public ChargeStationService(IChargeStationRepository chargeStationRepository, IGroupRepository groupRepository)
		{
			_chargeStationRepository = chargeStationRepository;
			_groupRepository = groupRepository;
		}

		public async Task CreateChargeStationAsync(ChargeStation chargeStation)
		{
			if (chargeStation == null)
				throw new ArgumentNullException(nameof(chargeStation));

			// Validate that the group exists
			var group = await _groupRepository.GetByIdAsync(chargeStation.GroupId);
			if (group == null)
				throw new InvalidOperationException($"Group with ID {chargeStation.GroupId} does not exist.");

			await _chargeStationRepository.CreateAsync(chargeStation);
		}

		public async Task UpdateChargeStationAsync(ChargeStation chargeStation)
		{
			if (chargeStation == null)
				throw new ArgumentNullException(nameof(chargeStation));

			await _chargeStationRepository.UpdateAsync(chargeStation);
		}

		public async Task DeleteChargeStationAsync(Guid chargeStationId)
		{
			// Validation could be added here as needed

			await _chargeStationRepository.DeleteAsync(chargeStationId);
		}

		public async Task<ChargeStation> GetChargeStationByIdAsync(Guid chargeStationId)
		{
			return await _chargeStationRepository.GetByIdAsync(chargeStationId);
		}
	}
}
