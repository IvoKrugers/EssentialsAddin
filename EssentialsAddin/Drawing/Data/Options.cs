namespace Diagram.Drawing.Data
{
	public class Options
	{
		public Physics physics { get; set; }
		public NodeOptions nodes { get; set; }
		public EdgesOptions edges { get; set; }

		public Options()
		{
			physics = new Physics();
			nodes = new NodeOptions();
			edges = new EdgesOptions();
		}
	}

	public class Physics
	{
		public bool enabled { get; set; } = false;
	}

	public class NodeOptions
	{
		public string shape => "box";
	}

	public class EdgesOptions
	{
		public float hoverWidth { get; set; } = 1;
		public bool arrowStrikethrough { get; set; } = false;
		public bool smooth { get; set; } = false;
	}
}
