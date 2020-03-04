using System;
using System.Diagnostics;
using System.Threading;
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
            Setup();

            this.ShowAll();
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

            filterEntry.Text = PropertyService.Get(SolutionFilterPad.PROPERTY_KEY, string.Empty);
            collapseEntry.Text = PropertyService.Get("CollapseFilter", collapseEntry.Text);
        }

        protected void OnFilterEntryChanged(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void StartTimer()
        {
            StopTimer();
            timer = new Timer(OnTimerElapsed, null, 1000, Timeout.Infinite); // dueTime in miliseconds
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
            Debug.WriteLine("[OnFilterEntryChanged] TEST!!!!");
            PropertyService.Set(SolutionFilterPad.PROPERTY_KEY, filterEntry.Text);

            if (string.IsNullOrEmpty(filterEntry.Text))
            {
                CollapseToCSharpProjects();
                return;
            }

            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            pad.TreeView.CollapseTree();

            var root = pad.TreeView.GetRootNode();
            if (root != null)
            {
                root.Expanded = true;
                var option = root.Options[FileNodeBuilderExtension.OneClickShowFileOption];
                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = false;
                pad.TreeView.RefreshNode(root);

                ExpandAll(pad.TreeView, root);

                root.Options[FileNodeBuilderExtension.OneClickShowFileOption] = option;
            }
            filterEntry.GrabFocus();
        }

        private void ExpandAll(ExtensibleTreeView tree, ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;
            if (typename == "Solution")
            {
                node.ExpandToNode();

                Debug.WriteLine($"\t{node.DataItem.GetType().FullName}\t{node.NodeName}");

                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    while (continueLoop)
                    {
                        var wso = node.DataItem as WorkspaceObject;
                            Debug.WriteLine($"{wso.Name} {wso}");
                            foreach (var item in CollapseFilter)
                            {
                                if (wso.Name.ToLower().Contains(item))
                                {
                                    ExpandCSharpProjectFiles(tree, node);
                                    break;
                                }
                            }
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                }
            }
        }

        private void ExpandCSharpProjectFiles(ExtensibleTreeView tree, ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;

            if (typename == "CSharpProject" || typename == "ProjectFolder" || typename == "ProjectFile")
            {
                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    while (continueLoop)
                    {
                        ExpandCSharpProjectFiles(tree, node);
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                    return;
                }

                if (typename != "CSharpProject")
                {
                    Debug.WriteLine($"\tExpandToNode {node.DataItem.GetType().FullName}\t{node.NodeName}");
                    node.ExpandToNode();
                }
            }
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
            CollapseToCSharpProjects();
        }

        private string[] CollapseFilter
        {
            get
            {
                var filterText = PropertyService.Get<string>("CollapseFilter", String.Empty).ToLower();
                //filterText = "items".ToLower();
                if (string.IsNullOrEmpty(filterText))
                    return new string[0];

                char[] delimiterChars = { ' ', ';', ':', '\t', '\n' };
                var filter = filterText.Split(delimiterChars);
                return filter;
            }
        }

        private void CollapseToCSharpProjects()
        {
            PropertyService.Set("CollapseFilter", collapseEntry.Text);

            var pad = (SolutionPad)IdeApp.Workbench.Pads.SolutionPad.Content;
            if (pad == null)
                return;

            pad.TreeView.GrabFocus();
            pad.TreeView.CollapseTree();

            var node = pad.TreeView.GetRootNode();
            if (node != null)
            {
                //node.Expanded = true;
                pad.TreeView.RefreshNode(node);

                var typename = node.DataItem.GetType().Name;
                if (typename == "Solution")
                {
                    if (node.HasChildren())
                    {
                        var continueLoop = node.MoveToFirstChild();
                        while (continueLoop)
                        {
                            if (node.DataItem is Project proj)
                            {
                                Debug.WriteLine($"{proj.Name} {proj}");

                                foreach (var item in CollapseFilter)
                                {
                                    if (proj.Name.ToLower().Contains(item))
                                    {
                                        node.MoveToFirstChild();
                                        node.ExpandToNode();
                                        node.MoveToParent();
                                    }
                                }
                            }
                            continueLoop = node.MoveNext();
                        }
                        node.MoveToParent();
                    }
                }
            }
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