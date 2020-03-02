//using System;
//using MonoDevelop.Components.Commands;
//using MonoDevelop.Ide.Gui.Components;
//using MonoDevelop.Projects;

//namespace EssentialsAddin
//{

//	class AddinProjectNodeBuilder : NodeBuilderExtension
//	{
//		public override bool CanBuildNode(Type dataType)
//		{
//			return typeof(DotNetProject).IsAssignableFrom(dataType);
//		}

//		public override Type CommandHandlerType
//		{
//			get { return typeof(AddinProjectCommandHandler); }
//		}

//		public override bool HasChildNodes(ITreeBuilder builder, object dataObject)
//		{
//			return base.HasChildNodes(builder, dataObject);
//		}

//		public override void BuildChildNodes(ITreeBuilder treeBuilder, object dataObject)
//		{
//			base.BuildChildNodes(treeBuilder, dataObject);
//		}

//		class AddinProjectCommandHandler : NodeCommandHandler
//		{
//			public override void ActivateItem()
//			{
//				base.ActivateItem();
//			}
//		}
//	}
	
//	public class FileNodeBuilder : TypeNodeBuilder
//	{
//		public override Type NodeDataType 
//		=> typeof(ProjectItem);

//		public override string GetNodeName(ITreeNavigator thisNode, object dataObject)
//		{
//			var f = (ProjectItem)dataObject;
//			return f.ItemName;
//		}

//		public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
//		{
//			var f = (ProjectItem)dataObject;
//			nodeInfo.Label = f.ItemName;
//		}

//		public override Type CommandHandlerType
//		=> typeof(FileNodeCommandHandler); 
//	}

//	public class FileNodeCommandHandler: NodeCommandHandler
//	{
//		//[CommandHandler(AddinCommands.ShowProperties)]
//		//public void OnShowProperties()
//		//{	
//		//	System.Diagnostics.Debug.Print("OnShowProperties");
//		//}

//		//[CommandUpdateHandler(AddinCommands.ShowProperties)]
//		//public void UpdateShowProperties(CommandInfo info)
//		//{
//		//	info.Enabled = true;
//		//}
		
		
		
//		//[CommandHandler(EssentialsAddin.AddinCommands.ViewFile)]
//		//public void OnViewFile()
//		//{
//		//	System.Diagnostics.Debug.Print("OnShowProperties");
//		//}
		
//		public override void OnItemSelected()
//		{
//			base.OnItemSelected();


//			ProjectFile f;
			
//		}
		
//		public override void ActivateItem()
//		{
//			base.ActivateItem();
//		}
//	}
//}
