using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;

namespace EssentialsAddin.CommandHandlers
{
    public class AppOutputFilterPadMenuHandler : CommandHandler
    {
        protected override void Run()
        {
            var pad = IdeApp.Workbench.GetPad<OutputFilterPad>();
            if (pad != null)
            {
                pad.Visible = true;
                pad.IsOpenedAutomatically = true;
                pad.BringToFront(true);
            }
        }

        protected override void Update(CommandInfo info)
        {
            info.Enabled = true;
        }
    }
}