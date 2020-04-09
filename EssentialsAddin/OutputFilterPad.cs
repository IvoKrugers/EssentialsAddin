using MonoDevelop.Components;
using MonoDevelop.Ide.Gui;

namespace EssentialsAddin
{
    public class OutputFilterPad : PadContent
    {
        private OutputFilterWidget control;
        public override Control Control => control ?? (control = new OutputFilterWidget());

        public override string Id => Constants.OutputFilterPadId;

        protected override void Initialize(IPadWindow window)
        {
            base.Initialize(window);
            //this.Window.Title = $"Output Filter ({Constants.Version})";
        }
    }
}
