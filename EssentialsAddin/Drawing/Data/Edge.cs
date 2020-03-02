using Newtonsoft.Json;

namespace Diagram.Drawing.Data
{
	public abstract class Edge
	{
		[JsonIgnore]
		public ClassDefinition FromClass { get; }
		[JsonIgnore]
		public ClassDefinition ToClass { get; }

		public string from => FromClass.Id;
		public string to => ToClass.Id;
		public string arrows { get; set; } = "to";

		public virtual string color => "#72c6ff";
		public virtual int width => 3;

		public Edge(ClassDefinition from, ClassDefinition to) {
			FromClass = from;
			ToClass = to;
		}
	}

	public class ClassEdge : Edge {
		public ClassEdge(ClassDefinition from, ClassDefinition to) : base(from, to) { }
	}

	public class InterfaceEdge : Edge {
		public override string color => "#c6ffde";
		public int[] dashes => new int[] { 5, 5 };

		public InterfaceEdge(ClassDefinition from, ClassDefinition to) : base(from, to) { }
	}

	public class DependancyEdge : Edge
	{
		public override int width => 1;
		public int[] dashes => new int[] { 2, 10 };

		public DependancyEdge(ClassDefinition from, ClassDefinition to) : base(from, to) { }
	}
}
