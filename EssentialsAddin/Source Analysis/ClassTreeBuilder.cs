using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using MonoDevelop.Ide.TypeSystem;
using System;

namespace Diagram
{
	public class ClassTreeBuilder
	{
		public static ClassTreeBuilder Instance => instance ?? (instance = new ClassTreeBuilder());
		static ClassTreeBuilder instance;

		ClassTreeBuilder() { }

		public async Task<List<ClassDefinition>> ProcessAsync(MonoDevelop.Projects.Project project, IProgress<TreeBuilderProgress> progress = null)
		{
			var classes = new Dictionary<ITypeSymbol, ClassDefinition>();
			//var compilation = await TypeSystemService.GetCompilationAsync(project);
			//var types = compilation.GetAllTypesInMainAssembly();
			//progress?.Report(new TreeBuilderProgress { totalClasses = types.Count() });

			//for (int i = 0; i < types.Count(); i++)
			//{
			//	var type = types.ElementAt(i);
			//	var definition = GetValueOrCreate(type, classes);

			//	if (type.IsAbstract && definition.ClassType == ClassType.Class)
			//		definition.ClassType = ClassType.Abstract;

			//	foreach (var iface in type.Interfaces)
			//	{
			//		var interfaceDefinition = GetValueOrCreate(iface, classes);
			//		interfaceDefinition.ClassType = ClassType.Interface;
			//		definition.Interfaces.Add(interfaceDefinition);
			//	}

			//	foreach (var baseClass in type.GetAllBaseClasses())
			//	{
			//		if (!baseClass.IsDefinedInSource()) continue;

			//		var baseDefinition = GetValueOrCreate(baseClass, classes);
			//		definition.BaseClasses.Add(baseDefinition);
			//	}

			//	HashSet<ClassDefinition> dependencies = new HashSet<ClassDefinition>();
			//	var syntaxDeclarations = type.GetDeclarations();
			//	foreach (var declaration in syntaxDeclarations)
			//	{
			//		var semanticModel = compilation.GetSemanticModel(declaration.SyntaxTree);
			//		var descendants = declaration.GetSyntax().DescendantNodes();
			//		foreach (var descendant in descendants)
			//		{
			//			ITypeSymbol usedType = semanticModel.GetTypeInfo(descendant).Type;
			//			usedType = usedType as INamedTypeSymbol;
			//			if (usedType != null && usedType.IsDefinedInSource())
			//			{
			//				var def = GetValueOrCreate(usedType, classes);
			//				if (def != definition && !definition.Interfaces.Contains(def) && !definition.BaseClasses.Contains(def))
			//					dependencies.Add(def);
			//			}
			//		}
			//	}
			//	definition.Dependencies.AddRange(dependencies);

			//	progress?.Report(new TreeBuilderProgress { totalClasses = types.Count(), doneClasses = i + 1 });
			//}

			return classes.Values.ToList();
		}

		ClassDefinition GetValueOrCreate(ITypeSymbol v, Dictionary<ITypeSymbol, ClassDefinition> c)
		{
			ClassDefinition ret;
			if (c.TryGetValue(v, out ret))
				return ret;
			return c[v] = new ClassDefinition(v);
		}
	}

	public struct TreeBuilderProgress
	{
		public int totalClasses;
		public int doneClasses;
	}
}
