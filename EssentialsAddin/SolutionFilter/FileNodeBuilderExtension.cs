using System;
using System.Diagnostics;
using System.IO;
using EssentialsAddin.Helpers;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads.ProjectPad;
using MonoDevelop.Projects;

namespace EssentialsAddin.SolutionFilter
{
    public class FileNodeBuilderExtension : NodeBuilderExtension
    {
        private readonly string OneClickChar
#if DEBUG
         = "-->";
#else
         = "=>";
#endif
        public static string OneClickShowFileOption = "OneClickShowFile";

        public override bool CanBuildNode(Type dataType)
        {
            var canBuild =
                typeof(ProjectFolder).IsAssignableFrom(dataType) ||
                typeof(ProjectFile).IsAssignableFrom(dataType) ||
                dataType.Name == "CSharpProject";
            return canBuild;
        }

        public override void PrepareChildNodes(object dataObject)
        {
            base.PrepareChildNodes(dataObject);
        }

        public override void GetNodeAttributes(ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
        {
            base.GetNodeAttributes(parentNode, dataObject, ref attributes);

            var filter = EssentialProperties.SolutionFilterArray;
            if (filter.Length == 0)
                return;

            if (dataObject is DotNetProject project)
            {
                if (!ProjectHasChildNodesInFilter(project))
                {
                    attributes = NodeAttributes.Hidden;
                }
            }

            if (dataObject is ProjectFolder pf )
            {
                if (!ProjectFolderHasChildNodesInFilter((ITreeBuilder)parentNode, pf))
                {
                    attributes = NodeAttributes.Hidden;
                }
                //var txt = attributes == NodeAttributes.Hidden ? "HIDE" : "SHOW";
                //Debug.WriteLine($"{txt} \tProjectFolder {pf.Path} ");
            }

            if (dataObject is ProjectFile file )
            {
                //var hide = true;
                //foreach (var key in filter)
                //{
                //    if (file.ProjectVirtualPath.ToString().ToLower().Contains(key))
                //    {
                //        hide = false;
                //        break;
                //    }
                //}
                if (!FilteredProjectCache.IsProjectItemVisible(file))
                    attributes = NodeAttributes.Hidden;
            }
        }

        private bool ProjectHasChildNodesInFilter(Project project)
        {
            FilteredProjectCache.ScanProjectForFiles(project);
            return FilteredProjectCache.IsProjectItemVisible(project);
        }

        private bool ProjectFolderHasChildNodesInFilter(ITreeBuilder builder, ProjectFolder dataObject)
        {
            Project project = builder.GetParentDataItem(typeof(Project), true) as Project;
            if (project == null)
                return false;

            FilteredProjectCache.ScanProjectForFiles(project);
            return FilteredProjectCache.IsProjectItemVisible(dataObject);
        }

        public override void BuildNode(ITreeBuilder treeBuilder, object dataObject, NodeInfo nodeInfo)
        {
            if (!string.IsNullOrEmpty(EssentialProperties.SolutionFilter))
            {
                if (!FilteredProjectCache.IsProjectItemEnabled(dataObject))

                    nodeInfo.Style = NodeInfo.LabelStyle.Disabled;
            }

            if (!(dataObject is ProjectFile))
                return;

            ProjectFile file = (ProjectFile)dataObject;

            var ext = Path.GetExtension(file.FilePath);

            if (EssentialProperties.OneClickShowFile && EssentialProperties.ExcludedExtensionsFromOneClick.FindIndex((s) => s == ext) == -1)
            {
                nodeInfo.Label = string.Format("{0} {1}", Path.GetFileName(file.FilePath), OneClickChar);
            }
        }

        public override Type CommandHandlerType
        {
            get
            {
                //if (Context.GetTreeBuilder().Options[OneClickShowFileOption])
                if (EssentialProperties.OneClickShowFile)
                    return typeof(FileNodeCommandHandler);
                else
                    return base.CommandHandlerType;
            }
        }
    }
}
