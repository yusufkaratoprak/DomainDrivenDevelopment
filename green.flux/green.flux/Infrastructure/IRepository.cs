﻿using green.flux.Domain;

namespace green.flux.Infrastructure
{

	public interface IGroupRepository 
	{
		Task<Group> CreateAsync(Group group);
		Task UpdateAsync(Group group);
		Task<Group> GetByIdAsync(Guid id);
		Task DeleteAsync(Guid id);
	}

	public interface IChargeStationRepository 
	{
		Task<ChargeStation> CreateAsync(ChargeStation chargeStation);
		Task UpdateAsync(ChargeStation chargeStation);
		Task<ChargeStation> GetByIdAsync(Guid id);
		Task DeleteAsync(Guid id);
	}
	public interface IConnectorRepository 
	{
		Task<Connector> CreateAsync(Connector connector);
		Task UpdateAsync(Connector connector);
		Task<Connector> GetByIdAsync(int id);
		Task DeleteAsync(int id);
	}
}