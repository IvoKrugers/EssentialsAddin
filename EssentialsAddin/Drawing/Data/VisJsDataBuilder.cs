using System.Collections.Generic;
using MonoDevelop.Ide.TypeSystem;
using Newtonsoft.Json;
using Diagram.Storage;

namespace Diagram.Drawing.Data
{
	public class VisJsDataBuilder
	{
		public static VisJsDataBuilder Instance => instance ?? (instance = new VisJsDataBuilder());
		static VisJsDataBuilder instance;

		VisJsDataBuilder() { }

		public string MakeData(List<ClassDefinition> classes, Dictionary<string, StoredNode> storedNodes)
		{
			List<Node> nodes = new List<Node>();
			List<Edge> edges = new List<Edge>();

			foreach (var c in classes)
			{
				nodes.Add(SetNodeData(MakeNode(c), storedNodes));
				edges.AddRange(MakeEdges(c));
			}

			return $@"{{
									nodes: new vis.DataSet({JsonConvert.SerializeObject(nodes)}),
									edges: new vis.DataSet({JsonConvert.SerializeObject(edges)})
								}}";
		}

		Node MakeNode(ClassDefinition c)
		{
			var id = c.Symbol.GetFullName();
			var name = c.DisplayName;

			switch (c.ClassType)
			{
				case ClassType.Class:
					return new ClassNode { id = id, label = name };
				case ClassType.Interface:
					return new InterfaceNode { id = id, label = name };
				case ClassType.Abstract:
					return new AbstractNode { id = id, label = name };
			}
			return null;
		}

		Node SetNodeData(Node n, Dictionary<string, StoredNode> storedNodes)
		{
			if (storedNodes != null && storedNodes.ContainsKey(n.id))
			{
				var node = storedNodes[n.id];
				n.x = node.x;
				n.y = node.y;
				n.classGroups = node.classGroups;
			}

			return n;
		}

		List<Edge> MakeEdges(ClassDefinition c)
		{
			var edges = new List<Edge>();

			foreach (var baseClass in c.BaseClasses)
				edges.Add(new ClassEdge(baseClass, c));

			foreach (var interfaceClass in c.Interfaces)
				edges.Add(new InterfaceEdge(interfaceClass, c));

			foreach (var dependantClass in c.Dependencies)
				edges.Add(new DependancyEdge(c, dependantClass));

			return edges;
		}
	}
}
