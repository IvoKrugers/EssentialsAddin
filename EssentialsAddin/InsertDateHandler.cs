using MonoDevelop.Components.Commands;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui;
using System;

namespace EssentialsAddin
{
	class InsertDateHandler : CommandHandler
	{
		[CommandHandler(AddinCommands.InsertDate)]
		protected void InsertDate()
		{
			var editor = IdeApp.Workbench.ActiveDocument.Editor;
			var date = $"//{DateTime.Now.ToShortDateString()}";
			editor.InsertAtCaret(date);
		}

		protected override void Update(CommandInfo info)
		{
			info.Enabled = IdeApp.Workbench.ActiveDocument?.Editor != null;
		}
	}
}
