namespace green.flux.Domain
{
	public class ChargeStation
	{
		public Guid ID { get;  set; } 
		public string? Name { get; set; }
		public Guid GroupId { get; set; }
		public List<Connector> Connectors { get; set; } = new List<Connector>();
	
	}

}
