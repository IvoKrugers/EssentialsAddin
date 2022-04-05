using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using EssentialsAddin.Helpers;
using ICSharpCode.NRefactory.MonoCSharp;
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

            Initialize();

            StartListeningForWorkspaceChanges();
            //window.PadHidden += (sender, e) => control.SaveNodeLocationsForSelectedProject();

            //Debug.WriteLine($"Bundle path: {NSBundle.MainBundle.BundlePath}");
            //Debug.WriteLine($"Bundle Resource path: {NSBundle.MainBundle.ResourcePath}");

            this.Window.Title = $"Solution Filter ({Constants.Version})";
        }

        private bool isPinnedDocumentsDirty = false;

        void StartListeningForWorkspaceChanges()
        {
            //IdeApp.Workbench.DocumentOpened += (sender, e) => { isPinnedDocumentsDirty = true; };
            //IdeApp.Workbench.DocumentClosed += (sender, e) => { isPinnedDocumentsDirty = true; };
            IdeApp.Workbench.ActiveDocumentChanged += (sender, e) => StorePinnedDocuments(sender);

            IdeApp.Workspace.SolutionLoaded += (sender, e) => Initialize();
            IdeApp.Workspace.CurrentSelectedSolutionChanged += (sender, e) => Initialize();

            //IdeApp.Workspace.SolutionLoaded += (sender, e) => { log("SolutionLoaded"); };
            //IdeApp.Workspace.SolutionUnloaded += (sender, e) => { log("SolutionUnloaded"); };
            //IdeApp.Workspace.WorkspaceItemLoaded += (sender, e) => { log("WorkspaceItemLoaded"); };
            //IdeApp.Workspace.WorkspaceItemUnloaded += (sender, e) => { log("WorkspaceItemUnloaded"); };
            //IdeApp.Workspace.SolutionItemLoaded += (sender, e) => { log("SolutionItemLoaded"); };
            //IdeApp.Workspace.SolutionItemUnloaded += (sender, e) => { log("SolutionItemUnloaded"); };
            //IdeApp.Workspace.ConfigurationsChanged += (sender, e) => { log("ConfigurationsChanged"); };
            //IdeApp.Workspace.FirstWorkspaceItemOpened += (sender, e) => { log("FirstWorkspaceItemOpened"); };
            //IdeApp.Workspace.StoringUserPreferences += (sender, e) => { log("StoringUserPreferences"); };
            //IdeApp.Workspace.ActiveConfigurationChanged += (sender, e) => { log("ActiveConfigurationChanged"); };
            //IdeApp.Workspace.ActiveExecutionTargetChanged += (sender, e) => { log("ActiveExecutionTargetChanged"); };
            //IdeApp.StartupCompleted += (sender, e) => { log("StartupCompleted"); };
            //IdeApp.Initialized += (sender, e) => { log("Initialized"); };
            IdeApp.FocusIn += (sender, e) => { log("FocusIn"); Initialize(true); };
        }

        private void log([CallerMemberName] string memberName = "", [CallerLineNumber] int ln = 0)
        {
            Debug.WriteLine($"{memberName}:ln {ln}: event fired (CurrentBranch: {GitHub.GitHelper.GetCurrentBranch()})");
        }

        SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private void StorePinnedDocuments(object sender)
        {
            if (sender is null)
                return;

            if (_semaphore.CurrentCount <= 0)
                return;


            System.Threading.Tasks.Task.Run(() =>
           {
               _semaphore.Wait();
               try
               {
                   if (!EssentialProperties.Initialized)
                       return;

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
                               EssentialProperties.AddPinnedDocument(IdeApp.Workbench.Documents[index]);
                           index++;
                       }
                   }
               }
               catch (System.Exception ex)
               {
                   Debug.WriteLine(ex.Message);
                   Debugger.Break();
               }
               _semaphore.Release();
           });
        }

        internal void Initialize(bool forceReload = false)
        {
            PropertyService.Instance.Init(IdeApp.Workspace.CurrentSelectedSolution);

            var filterWidget = ((SolutionFilterWidget)Control);

            var filterChanged =
                filterWidget.FilterText != EssentialProperties.SolutionFilter
                || filterWidget.ExpandText != EssentialProperties.ExpandFilter;

            filterWidget.LoadProperties();

            if (forceReload && filterChanged)
                filterWidget.StartTimer();
        }
    }
}
