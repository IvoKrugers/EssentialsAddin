using MonoDevelop.Components.Commands;

namespace EssentialsAddin.CommandHandlers
{
    public class CurrentVersionMenuHandler : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            info.Enabled = false;//IdeApp.Workbench.ActiveDocument?.Editor != null;
            info.Text = $"Essentials Addin ({Constants.Version})";
        }
    }
}