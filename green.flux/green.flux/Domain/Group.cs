namespace green.flux.Domain
{
	public class Group
	{
		public Guid ID { get;  set; } // Assuming this is set by the database.
		public string? Name { get; set; }
		public int Capacity { get; set; }
		public List<ChargeStation> ChargeStations { get; set; } = new List<ChargeStation>();

	}

}
