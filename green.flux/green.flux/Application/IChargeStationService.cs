using green.flux.Domain;

namespace green.flux.Infrastructure
{
	public interface IChargeStationService
	{
		Task CreateChargeStationAsync(ChargeStation chargeStation);
		Task UpdateChargeStationAsync(ChargeStation chargeStation);
		Task DeleteChargeStationAsync(Guid chargeStationId);
		Task<ChargeStation> GetChargeStationByIdAsync(Guid chargeStationId);

	}
}