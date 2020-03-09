using System;
using System.Diagnostics;
using System.Threading;
using EssentialsAddin.Helpers;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Projects;

namespace EssentialsAddin
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SolutionFilterWidget : Gtk.Bin
    {
        private Timer timer;

        public SolutionFilterWidget()
        {
            this.Build();
            this.ShowAll();
            Setup();
        }

        private void Setup()
        {
            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                oneClickCheckbutton.Active = root.Options[FileNodeBuilderExtension.OneClickShowFileOption];
            }

            filterEntry.Text = EssentialProperties.SolutionFilter;
            collapseEntry.Text = EssentialProperties.ExpandFilter;
        }

        protected void OnFilterEntryChanged(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void StartTimer()
        {
            StopTimer();
            timer = new Timer(OnTimerElapsed, null, 2000, Timeout.Infinite); // dueTime in miliseconds
        }

        private void StopTimer()
        {
            timer?.Dispose();
            timer = null;
        }

        object refreshLock = new Object();
        private void OnTimerElapsed(object state)
        {
            lock (refreshLock)
            {
                StopTimer();
                Runtime.RunInMainThread(FilterSolutionPad);
            }
        }

        private void FilterSolutionPad()
        {
            IdeApp.Workbench.StatusBar.ShowMessage("Filtering...");

            EssentialProperties.SolutionFilter = filterEntry.Text;

            if (string.IsNullOrEmpty(filterEntry.Text))
            {
                ExpandOnlyCSharpProjects();
                return;
            }

            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.CollapseTree();

            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                root.Expanded = true;
                var option = root.Options[FileNodeBuilderExtension.OneClickShowFileOption];
                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = false;
                pad.TreeView.RefreshNode(root);

                SolutionTreeExtensions.ExpandAll(root);

                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = option;
            }

            IdeApp.Workbench.StatusBar.ShowReady();
        }

        

        protected void oneClickCheckbutton_Toggled(object sender, EventArgs e)
        {
            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = oneClickCheckbutton.Active;
                pad.TreeView.RefreshNode(root);
            }
        }

        protected void collapseButton_Clicked(object sender, EventArgs e)
        {
            ExpandOnlyCSharpProjects();
        }


        private void ExpandOnlyCSharpProjects()
        {
            EssentialProperties.ExpandFilter = collapseEntry.Text;
            SolutionTreeExtensions.ExpandOnlyCSharpProjects(EssentialProperties.ExpandFilterArray);
        }

        protected void clearButton_Clicked(object sender, EventArgs e)
        {
            filterEntry.Text = "";
        }

        protected void OnEditingDone(object sender, EventArgs e)
        {
        }
    }
}