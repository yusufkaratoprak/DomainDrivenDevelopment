using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using green.flux.Domain;
using Npgsql;

namespace green.flux.Infrastructure
{
	public class ConnectorRepository : IConnectorRepository
	{
		private readonly string _connectionString;

		public ConnectorRepository(IConfiguration configuration)
		{

			_connectionString = configuration.GetSection("ConnectionStrings:GreenFluxDb").Value ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task CreateAsync(Connector connector)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("INSERT INTO connectors (max_current, charge_station_id) VALUES (@maxCurrent, @chargeStationId)", connection);
				command.Parameters.AddWithValue("@maxCurrent", connector.MaxCurrent);
				command.Parameters.AddWithValue("@chargeStationId", connector.ChargeStationId);

				await command.ExecuteNonQueryAsync();
			}
		}

		public async Task<Connector> GetByIdAsync(int id)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("SELECT * FROM connectors WHERE id = @id", connection);
				command.Parameters.AddWithValue("@id", id);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						var connector = new Connector
						{
							ID = reader.GetInt32(reader.GetOrdinal("id")),
							MaxCurrent = reader.GetInt32(reader.GetOrdinal("max_current")),
							ChargeStationId = reader.GetGuid(reader.GetOrdinal("charge_station_id")),
						};

						return connector;
					}
				}
			}
			return null;
		}

		public async Task UpdateAsync(Connector connector)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("UPDATE connectors SET max_current = @maxCurrent, charge_station_id = @chargeStationId WHERE id = @id", connection);
				command.Parameters.AddWithValue("@maxCurrent", connector.MaxCurrent);
				command.Parameters.AddWithValue("@chargeStationId", connector.ChargeStationId);
				command.Parameters.AddWithValue("@id", connector.ID);

				await command.ExecuteNonQueryAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("DELETE FROM connectors WHERE id = @id", connection);
				command.Parameters.AddWithValue("@id", id);

				await command.ExecuteNonQueryAsync();
			}
		}
	}
}
