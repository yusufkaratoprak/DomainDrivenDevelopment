using green.flux.Application;
using green.flux.Domain;
using green.flux.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace green.flux.API
{
	[ApiController]
	[Route("[controller]")]
	public class ConnectorsController : ControllerBase
	{
		private readonly IConnectorService _connectorService;

		public ConnectorsController(IConnectorService connectorService)
		{
			_connectorService = connectorService;
		}

		[HttpPost]
		public async Task<ActionResult> CreateConnector([FromBody] Connector connector)
		{
			await _connectorService.CreateConnectorAsync(connector);
			return Ok(connector);
		}

		[HttpPut]
		public async Task<IActionResult> UpdateConnector([FromBody] Connector connector)
		{
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
