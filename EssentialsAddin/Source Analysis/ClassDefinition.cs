using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using MonoDevelop.Ide.TypeSystem;

namespace Diagram
{
	public class ClassDefinition
	{
		public ITypeSymbol Symbol { get; }

		public string Id => Symbol.GetFullName();
		public string DisplayName => Symbol.Name;

		public ClassType ClassType { get; internal set; } = ClassType.Class;
		public List<ClassDefinition> Interfaces { get; private set; }
		public List<ClassDefinition> BaseClasses { get; private set; }
		public List<ClassDefinition> Dependencies { get; private set; }

		public ClassDefinition(ITypeSymbol symbol)
		{
			Symbol = symbol;
			Interfaces = new List<ClassDefinition>();
			BaseClasses = new List<ClassDefinition>();
			Dependencies = new List<ClassDefinition>();
		}
	}

	public enum ClassType
	{
		Class,
		Interface,
		Abstract
	}
}
