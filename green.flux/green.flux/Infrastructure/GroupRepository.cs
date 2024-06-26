﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using green.flux.Domain;
using Npgsql;

namespace green.flux.Infrastructure
{

	public class GroupRepository : IGroupRepository
	{
		private string _connectionString;

		public GroupRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetSection("ConnectionStrings:GreenFluxDb").Value ?? throw new ArgumentNullException(nameof(configuration));
		}

		public async Task<Group> CreateAsync(Group group)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				if (!await IsGroupCapacityValid(group))
					throw new InvalidOperationException("The group's capacity is less than the sum of the max current of all connectors.");
				string insertQuery = "INSERT INTO groups (name, capacity) VALUES (@name, @capacity) RETURNING *;";
				using (var command = new NpgsqlCommand(insertQuery, connection))
				{
					command.Parameters.AddWithValue("@name", group.Name);
					command.Parameters.AddWithValue("@capacity", group.Capacity);

					using (var reader = await command.ExecuteReaderAsync())
					{
						if (await reader.ReadAsync())
							return new Group
							{
								ID = reader.GetGuid(reader.GetOrdinal("id")),
								Name = reader.GetString(reader.GetOrdinal("name")),
								Capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
							};
					}
				}
			}
			return null; 
		}


		public async Task DeleteAsync(Guid groupId)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				using (var transaction = await connection.BeginTransactionAsync())
				{
					// Delete connectors first if not using ON DELETE CASCADE
					var deleteConnectorsCommand = new NpgsqlCommand("DELETE FROM connectors WHERE charge_station_id IN (SELECT id FROM charge_stations WHERE group_id = @groupId)", connection);
					deleteConnectorsCommand.Parameters.AddWithValue("@groupId", groupId);
					await deleteConnectorsCommand.ExecuteNonQueryAsync();

					// Delete charge stations belonging to the group
					var deleteStationsCommand = new NpgsqlCommand("DELETE FROM charge_stations WHERE group_id = @groupId", connection);
					deleteStationsCommand.Parameters.AddWithValue("@groupId", groupId);
					await deleteStationsCommand.ExecuteNonQueryAsync();

					// Delete the group itself
					var deleteGroupCommand = new NpgsqlCommand("DELETE FROM groups WHERE id = @groupId", connection);
					deleteGroupCommand.Parameters.AddWithValue("@groupId", groupId);
					await deleteGroupCommand.ExecuteNonQueryAsync();

					await transaction.CommitAsync();
				}
			}
		}



		public async Task<Group> GetByIdAsync(Guid groupId)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				var command = new NpgsqlCommand("SELECT * FROM groups WHERE id = @groupId", connection);
				command.Parameters.AddWithValue("@groupId", groupId);

				using (var reader = await command.ExecuteReaderAsync())
				{
					if (await reader.ReadAsync())
					{
						var group = new Group
						{
							ID = reader.GetGuid(reader.GetOrdinal("id")),
							Name = reader.GetString(reader.GetOrdinal("name")),
							Capacity = reader.GetInt32(reader.GetOrdinal("capacity"))
						};

						return group;
					}
				}
			}

			return null;
		}


		public async Task UpdateAsync(Group group)
		{
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();

				if (!await IsGroupCapacityValid(group))
				{
					throw new InvalidOperationException("The group's capacity is less than the sum of the max current of all connectors.");
				}

				List<string> updates = new List<string>();
				if (!string.IsNullOrEmpty(group.Name))
					updates.Add("name = @name");
				if (group.Capacity > 0) // Assuming 0 is not a valid capacity
					updates.Add("capacity = @capacity");

				if (!updates.Any())
					throw new ArgumentException("No update information provided.");

				string updateClause = string.Join(", ", updates);
				var commandText = $"UPDATE groups SET {updateClause} WHERE id = @id";

				using (var command = new NpgsqlCommand(commandText, connection))
				{
					if (!string.IsNullOrEmpty(group.Name))
						command.Parameters.AddWithValue("@name", group.Name);
					if (group.Capacity > 0)
						command.Parameters.AddWithValue("@capacity", group.Capacity);

					command.Parameters.AddWithValue("@id", group.ID);

					await command.ExecuteNonQueryAsync();
				}
			}
		}


		private async Task<bool> IsGroupCapacityValid(Group group)
		{
			// Sum the max current of all connectors that belong to the group
			int totalMaxCurrent = 0;
			using (var connection = new NpgsqlConnection(_connectionString))
			{
				await connection.OpenAsync();
				var command = new NpgsqlCommand(
					@"SELECT SUM(c.max_current) 
                      FROM connectors c
                      JOIN charge_stations cs ON c.charge_station_id = cs.id
                      WHERE cs.group_id = @groupId", connection);
				command.Parameters.AddWithValue("@groupId", group.ID);

				object result = await command.ExecuteScalarAsync();
				totalMaxCurrent = Convert.IsDBNull(result) ? 0 : Convert.ToInt32(result);
			}

			// Check if the group's capacity is greater than or equal to the sum of the max current
			return group.Capacity >= totalMaxCurrent;
		}


	}

}
