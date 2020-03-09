using System;
using System.Threading;
using EssentialsAddin.Helpers;
using EssentialsAddin.SolutionFilter;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Pads;

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

        #region Events

        protected void OnFilterEntryChanged(object sender, EventArgs e)
        {
            StartTimer();
        }

        protected void oneClickCheckbutton_Toggled(object sender, EventArgs e)
        {
            EssentialProperties.OneClickShowFile = oneClickCheckbutton.Active;
            FilterSolutionPad();
        }

        protected void collapseButton_Clicked(object sender, EventArgs e)
        {
            ExpandOnlyCSharpProjects();
        }

        protected void clearButton_Clicked(object sender, EventArgs e)
        {
            filterEntry.Text = "";
        }

        protected void OnEditingDone(object sender, EventArgs e)
        {
        }

        #endregion

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
                var option = EssentialProperties.OneClickShowFile;
                EssentialProperties.OneClickShowFile = false;
                pad.TreeView.RefreshNode(root);

                SolutionTreeExtensions.ExpandAll(root);

                EssentialProperties.OneClickShowFile = option;
            }

            IdeApp.Workbench.StatusBar.ShowReady();
        }

        private void ExpandOnlyCSharpProjects()
        {
            EssentialProperties.ExpandFilter = collapseEntry.Text;
            SolutionTreeExtensions.ExpandOnlyCSharpProjects(EssentialProperties.ExpandFilterArray);
        }
    }
}