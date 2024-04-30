using FluentValidation;
using green.flux.Domain;
using green.flux.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectorsController : ControllerBase
	{
		private readonly IConnectorService _connectorService;
		private readonly IValidator<Connector> _connectorValidator;
		//we can use ILog if we needs but I decided not use here because of overengineering
		public ConnectorsController(IConnectorService connectorService, IValidator<Connector> connectorValidator)
		{
			_connectorService = connectorService;
			_connectorValidator = connectorValidator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateConnector([FromBody] Connector connector)
		{
			try
			{
				var validationResult = await _connectorValidator.ValidateAsync(connector);
				if (!validationResult.IsValid)
					return BadRequest(validationResult.Errors);
				var result = await _connectorService.CreateConnectorAsync(connector);
				return Ok(new { Status = "200", Message = "Success", Method = "Create", ID = result.ID });
			}
			catch (Exception ex)
			{
				// I am trying to show custome exception usage very primitive way but I don`t want to make too much because of  overengineering
				return StatusCode(400, new
				{
					ErrorCode = "10001",
					Method = "Create",
					CustomException = "CreateConnectorException",
					Message = ex.Message
				});
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateConnector([FromBody] Connector connector)
		{
			try
			{
				var validationResult = await _connectorValidator.ValidateAsync(connector);
				if (!validationResult.IsValid)
				{
					return BadRequest(validationResult.Errors);
				}

				await _connectorService.UpdateConnectorAsync(connector);
				return Ok(new { Status = "200", Message = "Success", Method = "Update"});
			}
			catch (Exception ex)
			{
				return StatusCode(400, new
				{
					ErrorCode = "10002",
					Method = "Update",
					CustomException = "UpdateConnectorException",
					Message = ex.Message
				});
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteConnector(int id)
		{
			try
			{
				await _connectorService.DeleteConnectorAsync(id);
				return Ok(new { Status = "200", Message = "Success", Method = "Delete" });
			}
			catch (Exception ex)
			{
				return StatusCode(400, new
				{
					ErrorCode = "10003",
					Method = "Delete",
					CustomException = "DeleteConnectorException",
					Message = ex.Message
				});
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Connector>> GetConnector(int id)
		{
			try
			{
				var connector = await _connectorService.GetConnectorByIdAsync(id);
				if (connector == null)
				{
					return NotFound();
				}
				return Ok(connector);
			}
			catch (Exception ex)
			{
				return StatusCode(400, new
				{
					ErrorCode = "10004",
					Method = "Get",
					CustomException = "GetConnectorException",
					Message = ex.Message
				});
			}
		}
	}
}
