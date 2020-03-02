using System.Collections.Generic;
using Diagram.Drawing.Data;
using Newtonsoft.Json;

namespace Diagram.Storage
{
	public class StoredDiagramDetails
	{
		public Dictionary<string, StoredNode> nodes { get; set; }
		public Dictionary<string, ClassGroup> classGroups { get; set; }

		public StoredDiagramDetails() {
			nodes = new Dictionary<string, StoredNode>();
			classGroups = new Dictionary<string, ClassGroup>();
		}
	}

	public class StoredNode
	{
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int x { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int y { get; set; }
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public List<string> classGroups { get; set; }
	}
}
