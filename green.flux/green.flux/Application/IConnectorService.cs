using green.flux.Domain;

namespace green.flux.Infrastructure
{
	public interface IConnectorService
	{
		Task<Connector> CreateConnectorAsync(Connector connector);
		Task UpdateConnectorAsync(Connector connector);
		Task DeleteConnectorAsync(int connectorId);
		Task<Connector> GetConnectorByIdAsync(int connectorId);
	}
}