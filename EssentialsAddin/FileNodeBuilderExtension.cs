using System;
using System.Diagnostics;
using System.IO;
using MonoDevelop.Core;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin
{
    public class FileNodeBuilderExtension : NodeBuilderExtension
    {
        private string[] Filter
        {
            get
            {
                var filterText = PropertyService.Get<string>(SolutionFilterPad.PROPERTY_KEY, String.Empty).ToLower();
                //filterText = "items".ToLower();
                if (string.IsNullOrEmpty(filterText))
                    return new string[0];

                char[] delimiterChars = { ' ', ';', ':', '\t', '\n' };
                var filter = filterText.Split(delimiterChars);
                return filter;
            }
        }

        private string OneClickChar = "-->";

        public static string[] ExcludedExtensions = { ".storyboard", ".xib" };

        public override bool CanBuildNode(Type dataType)
        {
            var canBuild =
                typeof(ProjectFolder).IsAssignableFrom(dataType) ||
                typeof(ProjectFile).IsAssignableFrom(dataType);
            //Debug.WriteLine($"[CanBuildNode] {dataType}, canBuild: {canBuild}");
            return canBuild;
        }

        public override void PrepareChildNodes(object dataObject)
        {
            //Debug.WriteLine($"PrepareChildNodes {dataObject}");
            base.PrepareChildNodes(dataObject);
        }

        public override void GetNodeAttributes(ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
        {
            //Debug.WriteLine($"GetNodeAttributes {parentNode}, {dataObject}");

            base.GetNodeAttributes(parentNode, dataObject, ref attributes);

            if (dataObject is ProjectFolder pf && Filter.Length > 0)
            {
                //Debug.WriteLine($"ProjectFolder {parentNode}, {dataObject}, ProjectFolder: {pf}");
                if (HasChildNodesInFilter((ITreeBuilder)parentNode, pf))
                {
                    parentNode.ExpandToNode();
                    //var nav = parentNode.GetParentDataItem<ProjectFolder>(true);
                    //if (nav != null)
                    //{
                    //    nav.ExpandToNode();
                    //}
                }
                else
                    attributes = NodeAttributes.Hidden;
            }

            if (dataObject is ProjectFile file && Filter.Length > 0)
            {
                var hide = true;
                foreach (var key in Filter)
                {
                    if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                    {
                        hide = false;
                        break;
                    }
                }
                if (hide)
                    attributes = NodeAttributes.Hidden;
            }

        }

        public bool HasChildNodesInFilter(ITreeBuilder builder, ProjectFolder dataObject)
        {
            Project project = builder.GetParentDataItem(typeof(Project), true) as Project;
            if (project == null)
                return false;

            // For big projects, a real HasChildNodes value is too slow to get
            if (project.Files.Count > 500)
                return true;

            var folder = dataObject.Path;

            foreach (var file in project.Files)
            {
                FilePath path;

                if (!file.Visible || file.Flags.HasFlag(ProjectItemFlags.Hidden))
                    continue;
                if (file.Subtype != Subtype.Directory)
                    path = file.IsLink ? project.BaseDirectory.Combine(file.ProjectVirtualPath) : file.FilePath;
                else
                    path = file.FilePath;

                if (path.IsChildPathOf(folder))
                {
                    foreach (var key in Filter)
                    {
                        if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
        {
            if (!(dataObject is ProjectFile))
                return;

            ProjectFile file = (ProjectFile)dataObject;

            var ext = Path.GetExtension(file.FilePath);
            //var filename = Path.GetFileName(file.FilePath);

            // Change node label if OneClick is active
            if (treeBuilder.Options["OneClickShowFile"] && ExcludedExtensions.FindIndex((s) => s == ext) == -1)
            {
                nodeInfo.Label = string.Format("{0} {1}", Path.GetFileName(file.FilePath), OneClickChar);
            }



            //var filter = Filter;
            //if (filter.Length >= 0)
            //{
            //    var disableNode = true;
            //    foreach (var key in filter)
            //    {
            //        if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
            //        {
            //            disableNode = false;
            //            break;
            //        }
            //    }
            //    nodeInfo.DisabledStyle = disableNode;
            //}
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
}
