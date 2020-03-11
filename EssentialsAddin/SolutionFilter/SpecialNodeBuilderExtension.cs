using System;
using System.Diagnostics;
using EssentialsAddin.Helpers;
using MonoDevelop.Ide.Gui.Components;

namespace EssentialsAddin.SolutionFilter
{
    public class SpecialNodeBuilderExtension : NodeBuilderExtension
    {
        public override bool CanBuildNode(Type dataType)
        {
            var canBuild =
                    dataType.Name == "ConnectedServiceFolderNode" ||
                    dataType.Name == "ProjectReferenceCollection" ||
                    dataType.Name == "AssetFolder" ||
                    dataType.Name == "GettingStartedNode" ||
                    dataType.Name == "DependenciesNode" ||
                    dataType.Name == "ProjectPackagesFolderNode";
                    //||                    dataType.Name == "SolutionFolder";

            Debug.WriteLine($"[CanBuildNode] {dataType}, canBuild: {canBuild}");
            return canBuild;
        }

        public override void GetNodeAttributes(ITreeNavigator parentNode, object dataObject, ref NodeAttributes attributes)
        {
            base.GetNodeAttributes(parentNode, dataObject, ref attributes);

            if (!string.IsNullOrEmpty(EssentialProperties.SolutionFilter))
            {
                attributes = NodeAttributes.Hidden;
            }
        }
    }
}
