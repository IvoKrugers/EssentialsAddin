//using System;
//using MonoDevelop.Core.Serialization;
//using MonoDevelop.Ide.Gui.Components;

//namespace EssentialsAddin.SolutionPad
//{

//	// https://github.com/mhutch

//	class ClassBuilder : TypeNodeBuilder
//	{
//		public override Type NodeDataType
//		{

//			get {
//				return typeof(IExtendedDataItem);
//				//return typeof(IClass); 
//			}
//		}

//		public override Type CommandHandlerType
//		{
//			get { return typeof(ClassCommandHandler); }
//		}

//		//public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
//		//{
//		//	//IClass cls = (IClass)dataObject;
//		//	//icon = Context.GetIcon(Runtime.Gui.Icons.GetIcon(cls));
//		//	//label = cls.Name;
//		//}

//		public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
//		{
//			base.BuildNode(treeBuilder, dataObject, nodeInfo);
//		}

//		public override string GetNodeName(ITreeNavigator thisNode, object dataObject)
//		{
//			//return ((IClass)dataObject).Name;
//			return "IVO";
//		}


//		public override string ContextMenuAddinPath
//		{
//			get { return "/SampleProjectPadExtension/ProjectPad/ContextMenu"; }
//		}
//	}
//}
