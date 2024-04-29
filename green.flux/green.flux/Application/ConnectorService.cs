using green.flux.Application;
using green.flux.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace green.flux.Infrastructure
{
	public class ConnectorService : IConnectorService
	{
		private readonly IConnectorRepository _connectorRepository;
		private readonly IChargeStationRepository _chargeStationRepository;

		public ConnectorService(IConnectorRepository connectorRepository, IChargeStationRepository chargeStationRepository)
		{
			_connectorRepository = connectorRepository;
			_chargeStationRepository = chargeStationRepository;
		}

		public async Task CreateConnectorAsync(Connector connector)
		{
			if (connector == null)
				throw new ArgumentNullException(nameof(connector));

			// Validate that the charge station exists
			var chargeStation = await _chargeStationRepository.GetByIdAsync(connector.ChargeStationId);
			if (chargeStation == null)
				throw new InvalidOperationException($"Charge station with ID {connector.ChargeStationId} does not exist.");

			// Validate that adding the connector does not exceed the number of allowed connectors
			if (chargeStation.Connectors.Count >= 5)
				throw new InvalidOperationException("The charge station already has the maximum number of connectors.");

			await _connectorRepository.CreateAsync(connector);
		}

		public async Task UpdateConnectorAsync(Connector connector)
		{
			if (connector == null)
				throw new ArgumentNullException(nameof(connector));

			await _connectorRepository.UpdateAsync(connector);
		}

		public async Task DeleteConnectorAsync(int connectorId)
		{
			// Validate as needed

			await _connectorRepository.DeleteAsync(connectorId);
		}

		public async Task<Connector> GetConnectorByIdAsync(int connectorId)
		{
			return await _connectorRepository.GetByIdAsync(connectorId);
		}
	}
}
