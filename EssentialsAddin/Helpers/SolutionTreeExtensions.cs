using System;
using System.Diagnostics;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;
using MonoDevelop.Projects;

namespace EssentialsAddin.Helpers
{
    public static class SolutionTreeExtensions
    {
        public static void ExpandAll(ITreeNavigator node)
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

                        if (!string.IsNullOrWhiteSpace(EssentialProperties.ExpandFilter))
                        {
                            foreach (var item in EssentialProperties.ExpandFilterArray)
                            {
                                if (wso.Name.ToLower().Contains(item))
                                {
                                    ExpandCSharpProjectFiles(node);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ExpandCSharpProjectFiles(node);
                        }
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();
                }
            }
        }

        private static void ExpandCSharpProjectFiles(ITreeNavigator node)
        {
            if (node == null)
                return;

            var typename = node.DataItem.GetType().Name;

            if (typename == "CSharpProject" || typename == "ProjectFolder" || typename == "ProjectFile")
            {
                if (node.HasChildren())
                {
                    var continueLoop = node.MoveToFirstChild();
                    var hasDependingChildren = false;
                    while (continueLoop)
                    {
                        if (node.DataItem is ProjectFile pf && !string.IsNullOrEmpty(pf.DependsOn))
                            hasDependingChildren = true;
                        else
                            ExpandCSharpProjectFiles(node);
                        continueLoop = node.MoveNext();
                    }
                    node.MoveToParent();

                    if (hasDependingChildren)
                        node.ExpandToNode();

                    return;
                }

                if (typename != "CSharpProject")
                    node.ExpandToNode();
            }
        }

        public static void ExpandOnlyCSharpProjects(string[] filter)
        {
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

                                foreach (var item in filter)
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
    }
}
