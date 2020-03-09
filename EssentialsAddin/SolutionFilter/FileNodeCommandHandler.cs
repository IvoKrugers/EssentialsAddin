using System;
using System.IO;
using EssentialsAddin.Helpers;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
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

            if (EssentialProperties.IsRefreshingTree)
                return;

            if (CurrentNode.DataItem is ProjectFile f)
            {
                string ext = Path.GetExtension(f.FilePath);
                if (EssentialProperties.ExcludedExtensionsFromOneClick.FindIndex((s) => s == ext) == -1)
                {
                    if (IdeApp.Workbench.ActiveDocument == null || IdeApp.Workbench.ActiveDocument.Name != f.FilePath.FileName)
                        IdeApp.Workbench.OpenDocument(f.FilePath, project: null);
                }
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
