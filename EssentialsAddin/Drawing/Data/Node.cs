using System.Collections.Generic;
using Newtonsoft.Json;

namespace Diagram.Drawing.Data
{
	public abstract class Node
	{
		public string id { get; set; }
		public string label { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? x { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? y { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<string> classGroups { get; set; }

		public abstract string color { get; }
	}

	public class ClassNode : Node {
		public override string color => "#72c6ff";
	}

	public class InterfaceNode : Node {
		public override string color => "#c6ffde";
	}

	public class AbstractNode : Node {
		public override string color => "#ff7a92";
	}
}
