using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;

namespace EssentialsAddin
{
    public class SolutionFilterPadMenuHandler : CommandHandler
    {
        protected override void Run()
        {
            var pad = IdeApp.Workbench.GetPad<SolutionFilterPad>();
            if (pad == null)
            {
                var fp = new SolutionFilterPad();
                IdeApp.Workbench.AddPad(fp,
                                        fp.Id,
                                        "(Essentials) Solution Filter",
                                        "ProjectPad.Bottom",
                                        MonoDevelop.Components.Docking.DockItemStatus.Dockable,
                                        Stock.Solution);
            }
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;//IdeApp.Workbench.ActiveDocument?.Editor != null;
        }
    }
}
