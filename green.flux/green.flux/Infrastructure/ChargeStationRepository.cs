using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using green.flux.Domain;
using Npgsql;

namespace green.flux.Infrastructure
{
	public class ChargeStationRepository : IChargeStationRepository
	{
		private readonly string _connectionString;

		public ChargeStationRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetSection("ConnectionStrings:GreenFluxDb").Value ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<ChargeStation> CreateAsync(ChargeStation chargeStation)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var insertQuery = "INSERT INTO charge_stations (name, group_id) VALUES (@name, @groupId) RETURNING *;";

				using (var command = new NpgsqlCommand(insertQuery, connection))
				{
					command.Parameters.AddWithValue("@name", chargeStation.Name);
					command.Parameters.AddWithValue("@groupId", chargeStation.GroupId);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
						{
							return new ChargeStation
							{
								ID = reader.GetGuid(reader.GetOrdinal("id")),
								Name = reader.GetString(reader.GetOrdinal("name")),
								GroupId = reader.GetGuid(reader.GetOrdinal("group_id")),
							};
						}
					}
				}
			}
			return null;
		}

		public async Task<ChargeStation> GetByIdAsync(Guid id)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("SELECT * FROM charge_stations WHERE id = @id", connection);
				command.Parameters.AddWithValue("@id", id);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						var chargeStation = new ChargeStation
						{
							ID = reader.GetGuid(reader.GetOrdinal("id")),
							Name = reader.GetString(reader.GetOrdinal("name")),
							GroupId = reader.GetGuid(reader.GetOrdinal("group_id")),
							// Assume Connectors are loaded in a separate call
							Connectors = new List<Connector>()
						};

						return chargeStation;
					}
				}
			}
			return null;
		}

		public async Task UpdateAsync(ChargeStation chargeStation)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("UPDATE charge_stations SET name = @name, group_id = @groupId WHERE id = @id", connection);
				command.Parameters.AddWithValue("@name", chargeStation.Name);
				command.Parameters.AddWithValue("@groupId", chargeStation.GroupId);
				command.Parameters.AddWithValue("@id", chargeStation.ID);

				await command.ExecuteNonQueryAsync();
			}
		}

		public async Task DeleteAsync(Guid id)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand("DELETE FROM charge_stations WHERE id = @id", connection);
				command.Parameters.AddWithValue("@id", id);

				await command.ExecuteNonQueryAsync();
			}
		}

	}
}
