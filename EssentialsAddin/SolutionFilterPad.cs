using System.Collections;
using System.Diagnostics;
using System.Threading;
using EssentialsAddin.Helpers;
using Microsoft.Build.Utilities;
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

        private bool isPinnedDocumentsDirty = false;

        void StartListeningForWorkspaceChanges()
        {
            IdeApp.Workbench.DocumentOpened += (sender, e) => { isPinnedDocumentsDirty = true; };
            IdeApp.Workbench.DocumentClosed += (sender, e) => { isPinnedDocumentsDirty = true; };
            IdeApp.Workbench.ActiveDocumentChanged += (sender, e) => StorePinnedDocuments(sender);

            IdeApp.Workspace.SolutionLoaded += (sender, e) => Initialize();
            IdeApp.Workspace.CurrentSelectedSolutionChanged += (sender, e) => Initialize();
        }

        SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private void StorePinnedDocuments(object sender)
        {
            if (!isPinnedDocumentsDirty)
                return;

            if (_semaphore.CurrentCount <= 0)
                return;
           

            System.Threading.Tasks.Task.Run( () =>
            {
                 _semaphore.Wait();
                try
                {
                    EssentialProperties.ClearOpenDocuments();

                    if (sender is Workbench wb)
                        sender = wb.RootWindow;

                    var activeWorkbenchWindowProp = sender.GetType().GetProperty("ActiveWorkbenchWindow");
                    var activeWorkbenchWindow = activeWorkbenchWindowProp.GetValue(sender, null);

                    var tabControlProp = activeWorkbenchWindow?.GetType().GetProperty("TabControl");
                    object tabControl = tabControlProp?.GetValue(activeWorkbenchWindow, null);

                    var tabsProp = tabControl?.GetType().GetProperty("Tabs");
                    object tabs = tabsProp?.GetValue(tabControl, null);

                    if (tabControl != null)
                    {
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
                    isPinnedDocumentsDirty = false;
                }
                catch (System.Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debugger.Break();
                }
                _semaphore.Release();
            });
        }

        private void Initialize()
        {
            PropertyService.Instance.Init(IdeApp.Workspace.CurrentSelectedSolution);
            ((SolutionFilterWidget)Control).LoadProperties();
            isPinnedDocumentsDirty = false;
        }
    }
}
