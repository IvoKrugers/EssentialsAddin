using EssentialsAddin.Helpers;
using MonoDevelop.Components.Commands;

namespace EssentialsAddin.SolutionFilter
{
    public class FileNodeContextMenuUnPinAllCommandHandler : CommandHandler
    {
        protected override void Update(CommandInfo info)
        {
            base.Update(info);
            info.Enabled = true;
        }

        protected override void Run()
        {
            EssentialProperties.ClearPinnedDocuments();
        }
    }
}