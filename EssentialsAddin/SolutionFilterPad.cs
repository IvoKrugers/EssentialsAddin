using System.Collections;
using System.Diagnostics;
using EssentialsAddin.Helpers;
using MonoDevelop.Components;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace EssentialsAddin
{
    public class SolutionFilterPad : PadContent
    {
        private SolutionFilterWidget control;
        public override Control Control => control ?? (control = new SolutionFilterWidget());
        public override string Id => Constants.SolutionFilterPadId;

        protected override void Initialize(IPadWindow window)
        {
            base.Initialize(window);
            StartListeningForWorkspaceChanges();
            //window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

            //Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
            //Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");

            //this.Window.Title = $"Solution Filter ({Constants.Version})";
            Initialize();
        }

        void StartListeningForWorkspaceChanges()
        {
            IdeApp.Workbench.ActiveDocumentChanged += (sender, e) =>
            {
                try
                {
                    EssentialProperties.ClearOpenDocuments();

                    var activeWorkbenchWindowProp = sender.GetType().GetProperty("ActiveWorkbenchWindow");
                    object activeWorkbenchWindow = activeWorkbenchWindowProp.GetValue(sender, null);

                    var tabControlProp = activeWorkbenchWindow.GetType().GetProperty("TabControl");
                    object tabControl = tabControlProp.GetValue(activeWorkbenchWindow, null);

                    var tabsProp = tabControl.GetType().GetProperty("Tabs");
                    object tabs = tabsProp.GetValue(tabControl, null);

                    var index = 0;
                    foreach (var tab in (IList)tabs)
                    {
                        var isPinnedProp = tab.GetType().GetProperty("IsPinned");
                        bool isPinned = (bool)isPinnedProp.GetValue(tab, null);
                        if (isPinned)
                            EssentialProperties.AddOpenDocument(IdeApp.Workbench.Documents[index]);
                        index++;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                }
            };

            IdeApp.Workspace.SolutionLoaded += (sender, e) => Initialize();
            IdeApp.Workspace.CurrentSelectedSolutionChanged += (sender, e) => Initialize();
            
        }

        private void Initialize()
        {
            PropertyService.Instance.Init(IdeApp.Workspace.CurrentSelectedSolution);
            ((SolutionFilterWidget)Control).LoadProperties();
        }
    }
}
