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
		public async Task<Connector> CreateAsync(Connector connector)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var insertQuery = "INSERT INTO connectors (max_current, charge_station_id) VALUES (@maxCurrent, @chargeStationId) RETURNING *;";
				using (var command = new NpgsqlCommand(insertQuery, connection))
				{
					command.Parameters.AddWithValue("@maxCurrent", connector.MaxCurrent);
					command.Parameters.AddWithValue("@chargeStationId", connector.ChargeStationId);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new Connector
							{
								ID = reader.GetInt32(reader.GetOrdinal("id")), // Assuming 'id' is an auto-increment field
								MaxCurrent = reader.GetInt32(reader.GetOrdinal("max_current")),
								ChargeStationId = reader.GetGuid(reader.GetOrdinal("charge_station_id")),
							};
						}
					}
				}
			}
			return null;
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
				List<string> updates = new List<string>();

				if (connector.MaxCurrent > 0) // Assuming 0 is not a valid value
					updates.Add("max_current = @maxCurrent");

				if (connector.ChargeStationId != Guid.Empty) // Assuming Guid.Empty means it's not set
					updates.Add("charge_station_id = @chargeStationId");

				if (!updates.Any())
					throw new ArgumentException("No update information provided.");

				string updateClause = string.Join(", ", updates);
				var commandText = $"UPDATE connectors SET {updateClause} WHERE id = @id";

				using (var command = new NpgsqlCommand(commandText, connection))
				{
					if (connector.MaxCurrent > 0)
						command.Parameters.AddWithValue("@maxCurrent", connector.MaxCurrent);

					if (connector.ChargeStationId != Guid.Empty)
						command.Parameters.AddWithValue("@chargeStationId", connector.ChargeStationId);

					command.Parameters.AddWithValue("@id", connector.ID);

					await command.ExecuteNonQueryAsync();
				}
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
