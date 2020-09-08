using MonoDevelop.Components.Commands;

namespace EssentialsAddin.CommandHandlers
{
    public class CurrentVersionMenuHandler : CommandHandler
    {
        //protected override void Run()
        //{
        //    var pad = IdeApp.Workbench.GetPad<OutputFilterPad>();
        //    if (pad != null)
        //    {
        //        pad.Visible = true;
        //        pad.IsOpenedAutomatically = true;
        //        pad.BringToFront(true);
        //    }
        //}

        protected override void Update(CommandInfo info)
        {
            info.Enabled = false;//IdeApp.Workbench.ActiveDocument?.Editor != null;
            info.Text = $"Essentials Addin ({Constants.Version})";
            //info.Icon = MonoDevelop.Ide.Gui.Stock.MonoDevelop;
        }
    }

}
