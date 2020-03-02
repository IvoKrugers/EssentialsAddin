//using System;
//using MonoDevelop.Ide;
//using MonoDevelop.Ide.Gui.Components;
//using MonoDevelop.Projects;



//using MonoDevelop.Core;
//using MonoDevelop.Core.Execution;
//using MonoDevelop.Ide.Gui;
//using MonoDevelop.Ide.Tasks;
//using MonoDevelop.Core.Instrumentation;

//namespace EssentialsAddin.SolutionPad
//{
//	class FileClassExtension : NodeBuilderExtension
//	{
//		//ClassInformationEventHandler changeClassInformationHandler;

//		//protected override void Initialize()
//		//{
//		//	changeClassInformationHandler = (ClassInformationEventHandler)Runtime.DispatchService.GuiDispatch(new ClassInformationEventHandler(OnClassInformationChanged));
//		//	Runtime.ParserService.ClassInformationChanged += changeClassInformationHandler;
//		//}

//		//public override void Dispose()
//		//{
//		//	Runtime.ParserService.ClassInformationChanged -= changeClassInformationHandler;
//		//}

//		//void OnClassInformationChanged(object sender, ClassInformationEventArgs e)
//		//{
//		//	if (e.Project == null) return;
//		//	ProjectFile file = e.Project.GetProjectFile(e.FileName);
//		//	if (file == null) return;

//		//	ITreeBuilder builder = Context.GetTreeBuilder(file);
//		//	if (builder != null)
//		//		builder.UpdateAll();
//		//}



//		public override bool CanBuildNode(Type dataType)
//		{
//			return typeof(ProjectFile).IsAssignableFrom(dataType);
//		}

//		public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
//		{
//			if (treeBuilder.Options["ShowFileClasses"])
//			{
//				ProjectFile file = (ProjectFile)dataObject;
//				//MonoDevelop.Projects.IFileItem cls;


//				//IClass[] cls = IdeApp.Services.ParserService;// Runtime.ParserService.GetFileContents(file.Project, file.Name);
//				//nodeInfo.Label = string.Format("{0} ({1})", label, cls.Length);
//			}
//		}

//		public override void BuildChildNodes(ITreeBuilder builder, object dataObject)
//		{
//			if (builder.Options["ShowFileClasses"])
//			{
//				ProjectFile file = (ProjectFile)dataObject;
//				//IClass[] cls = Runtime.ParserService.GetFileContents(file.Project, file.Name);
//				//foreach (IClass c in cls)
//					//builder.AddChild(c);
//			}
//		}

//		public override bool HasChildNodes(ITreeBuilder builder, object dataObject)
//		{
//			if (builder.Options["ShowFileClasses"])
//			{
//				ProjectFile file = (ProjectFile)dataObject;
//				//IClass[] cls = Runtime.ParserService.GetFileContents(file.Project, file.Name);
//				//return cls.Length > 0;
//				return false;
//			}
//			else
//				return false;
//		}
//	}
//}
