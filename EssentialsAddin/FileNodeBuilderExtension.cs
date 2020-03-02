
using System;
using MonoDevelop.Core.Collections;
using MonoDevelop.Components.Commands;
using MonoDevelop.Projects;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using System.IO;
using MonoDevelop.Core;

namespace EssentialsAddin
{
	public class FileNodeBuilderExtension : NodeBuilderExtension
	{

		public static string[] ExcludedExtensions = { ".storyboard", ".xib" };

		public override bool CanBuildNode(Type dataType)
		{
			return typeof(ProjectFile).IsAssignableFrom(dataType);
		}

        public override void BuildChildNodes(ITreeBuilder treeBuilder, object dataObject)
        {
            base.BuildChildNodes(treeBuilder, dataObject);
        }

        public override void PrepareChildNodes(object dataObject)
        {
            base.PrepareChildNodes(dataObject);
        }
        public override void GetNodeAttributes(ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
        {
            base.GetNodeAttributes(parentNode, dataObject, ref attributes);
        }

        public override bool HasChildNodes(ITreeBuilder builder, object dataObject)
        {
            return base.HasChildNodes(builder, dataObject);
        }

        public override void OnNodeAdded(object dataObject)
        {
            base.OnNodeAdded(dataObject);
        }

        public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
		{
			ProjectFile file = (ProjectFile)dataObject;
			var ext = Path.GetExtension(file.FilePath);
			var filename = Path.GetFileName(file.FilePath);
			
			// Change node label if OneClick is active
			if (treeBuilder.Options["OneClickShowFile"] && ExcludedExtensions.FindIndex((s) => s == ext) == -1)
			{
				nodeInfo.Label = string.Format("{0} {1}", Path.GetFileName(file.FilePath), "->");
			}
			
			var filterText = PropertyService.Get<string>(SolutionFilter.SolutionFilterPad.PROPERTY_KEY, String.Empty).ToLower();
            //filterText = "ViewModels".ToLower();
			if (!String.IsNullOrWhiteSpace(filterText)) {
				// Search for keys from the filter in the filename.
				char[] delimiterChars = { ' ', ';', ':', '\t', '\n' };
				
				var filter = filterText.Split(delimiterChars);
				var disableNode = true;
				
				foreach (var key in filter) {
					if (file.ProjectVirtualPath.ToString().ToLower().Contains(key)){
						disableNode = false;
						break;
					}
				}
				
				nodeInfo.DisabledStyle = disableNode;
                
			}
		}

		public override Type CommandHandlerType
		{
			get
			{
				if (Context.GetTreeBuilder().Options["OneClickShowFile"])
					return typeof(FileNodeCommandHandler);
				else
					return null;
			}
		}
	}

	public class FileNodeCommandHandler : NodeCommandHandler
	{
		
		// Double-Clicked
		public override void ActivateItem()
		{
			base.ActivateItem();
			//var aref = (ProjectFile)CurrentNode.DataItem;
			//IdeApp.Workbench.OpenDocument(aref.FilePath, project: null);
		}
		
		// Single-Clicked
		public override void OnItemSelected()
		{
			base.OnItemSelected();

			var f = (ProjectFile)CurrentNode.DataItem;
			string ext = Path.GetExtension(f.FilePath);
			if (FileNodeBuilderExtension.ExcludedExtensions.FindIndex((s) => s == ext) == -1)
			{
				IdeApp.Workbench.OpenDocument(f.FilePath, project: null);
			}
		}

		public override void RefreshItem()
		{
			base.RefreshItem();
		}

		//[CommandHandler (Commands.SynchWithMakefile)]
		//[AllowMultiSelection]
		//public void OnExclude ()
		//{
		//	//if all of the selection is already checked, then toggle checks them off
		//	//else it turns them on. hence we need to find if they're all checked,
		//	bool allChecked = true;
		//	foreach (ITreeNavigator node in CurrentNodes) {
		//		ProjectFile file = (ProjectFile) node.DataItem;
		//		if (file.Project != null) {
		//			MakefileData data = file.Project.GetMakefileData ();
		//			if (data != null && data.IsFileIntegrationEnabled (file.BuildAction)) {
		//				if (data.IsFileExcluded (file.FilePath)) {
		//					allChecked = false;
		//					break;
		//				}
		//			}
		//		}
		//	}

		//	Set<SolutionItem> projects = new Set<SolutionItem> ();

		//	foreach (ITreeNavigator node in CurrentNodes) {
		//		ProjectFile file = (ProjectFile) node.DataItem;
		//		if (file.Project != null) {
		//			projects.Add (file.Project);
		//			MakefileData data = file.Project.GetMakefileData ();
		//			if (data != null && data.IntegrationEnabled) {
		//				data.SetFileExcluded (file.FilePath, allChecked);
		//			}
		//		}
		//	}

		//	IdeApp.ProjectOperations.SaveAsync (projects);
		//}

		//[CommandUpdateHandler (Commands.SynchWithMakefile)]
		//public void OnUpdateExclude (CommandInfo cinfo)
		//{
		//	bool anyChecked = false;
		//	bool allChecked = true;
		//	bool anyEnabled = false;
		//	bool allEnabled = true;

		//	foreach (ITreeNavigator node in CurrentNodes) {
		//		ProjectFile file = (ProjectFile) node.DataItem;
		//		if (file.Project != null) {
		//			MakefileData data = file.Project.GetMakefileData ();
		//			if (data != null && data.IsFileIntegrationEnabled (file.BuildAction)) {
		//				anyEnabled = true;
		//				if (!data.IsFileExcluded (file.FilePath)) {
		//					anyChecked = true;
		//				} else {
		//					allChecked = false;
		//				}
		//			} else {
		//				allEnabled = false;
		//			}
		//		}
		//	}

		//	cinfo.Visible = anyEnabled;
		//	cinfo.Enabled = anyEnabled && allEnabled;
		//	cinfo.Checked = anyChecked;
		//	cinfo.CheckedInconsistent = anyChecked && !allChecked;
		//}
	}
}
