using FluentValidation;
using green.flux.Domain;
using green.flux.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectorsController : ControllerBase
	{
		private readonly IConnectorService _connectorService;
		private readonly IValidator<Connector> _connectorValidator;

		public ConnectorsController(IConnectorService connectorService, IValidator<Connector> connectorValidator)
		{
			_connectorService = connectorService;
			_connectorValidator = connectorValidator;
		}

		[HttpPost]
		public async Task<ActionResult> CreateConnector([FromBody] Connector connector)
		{
			var validationResult = await _connectorValidator.ValidateAsync(connector);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			await _connectorService.CreateConnectorAsync(connector);
			return Ok(connector);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateConnector([FromBody] Connector connector)
		{
			var validationResult = await _connectorValidator.ValidateAsync(connector);
			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			await _connectorService.UpdateConnectorAsync(connector);
			return NoContent();
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteConnector([FromBody] Connector connector)
		{
			await _connectorService.DeleteConnectorAsync(connector.ID);
			return NoContent();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Connector>> GetConnector(int id)
		{
			var connector = await _connectorService.GetConnectorByIdAsync(id);
			if (connector == null)
			{
				return NotFound();
			}
			return connector;
		}
	}
}
