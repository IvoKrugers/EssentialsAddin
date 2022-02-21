
// This file has been generated by the GUI designer. Do not modify.
namespace EssentialsAddin
{
	public partial class SolutionFilterWidget
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.HBox hbox1;

		private global::Gtk.Label label1;

		private global::Gtk.Entry filterEntry;

		private global::Gtk.Button button1;

		private global::Gtk.HBox hbox2;

		private global::Gtk.CheckButton oneClickCheckbutton;

		private global::Gtk.Button newReleaseAvailableButton;

		private global::Gtk.HBox hbox3;

		private global::Gtk.Label label2;

		private global::Gtk.Entry collapseEntry;

		private global::Gtk.Button collapseButton;

		private global::Gtk.HBox hbox4;

		private global::Gtk.Button ResetPinnedButton;

		private global::Gtk.Button ReloadPropertiesButton;

		private global::Gtk.Button PinOpenDocumentsButton;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget EssentialsAddin.SolutionFilterWidget
			global::Stetic.BinContainer.Attach(this);
			this.HeightRequest = 110;
			this.CanFocus = true;
			this.Name = "EssentialsAddin.SolutionFilterWidget";
			// Container child EssentialsAddin.SolutionFilterWidget.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox1 = new global::Gtk.HBox();
			this.hbox1.Name = "hbox1";
			// Container child hbox1.Gtk.Box+BoxChild
			this.label1 = new global::Gtk.Label();
			this.label1.Name = "label1";
			this.label1.LabelProp = global::Mono.Unix.Catalog.GetString("Tree Filter");
			this.hbox1.Add(this.label1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.label1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			w1.Padding = ((uint)(6));
			// Container child hbox1.Gtk.Box+BoxChild
			this.filterEntry = new global::Gtk.Entry();
			this.filterEntry.TooltipMarkup = "Separate by space, colon, semicolon";
			this.filterEntry.CanFocus = true;
			this.filterEntry.Name = "filterEntry";
			this.filterEntry.IsEditable = true;
			this.filterEntry.HasFrame = false;
			this.filterEntry.InvisibleChar = '●';
			this.hbox1.Add(this.filterEntry);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.filterEntry]));
			w2.Position = 2;
			w2.Padding = ((uint)(6));
			// Container child hbox1.Gtk.Box+BoxChild
			this.button1 = new global::Gtk.Button();
			this.button1.CanFocus = true;
			this.button1.Name = "button1";
			this.button1.UseUnderline = true;
			this.button1.Relief = ((global::Gtk.ReliefStyle)(1));
			this.button1.Label = global::Mono.Unix.Catalog.GetString("Clear");
			this.hbox1.Add(this.button1);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1[this.button1]));
			w3.PackType = ((global::Gtk.PackType)(1));
			w3.Position = 3;
			w3.Expand = false;
			w3.Fill = false;
			w3.Padding = ((uint)(2));
			this.vbox1.Add(this.hbox1);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
			w4.Position = 0;
			w4.Expand = false;
			w4.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.oneClickCheckbutton = new global::Gtk.CheckButton();
			this.oneClickCheckbutton.CanFocus = true;
			this.oneClickCheckbutton.Name = "oneClickCheckbutton";
			this.oneClickCheckbutton.Label = global::Mono.Unix.Catalog.GetString("Use one click to show file");
			this.oneClickCheckbutton.Active = true;
			this.oneClickCheckbutton.DrawIndicator = true;
			this.oneClickCheckbutton.UseUnderline = true;
			this.hbox2.Add(this.oneClickCheckbutton);
			global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.oneClickCheckbutton]));
			w5.Position = 0;
			// Container child hbox2.Gtk.Box+BoxChild
			this.newReleaseAvailableButton = new global::Gtk.Button();
			this.newReleaseAvailableButton.CanFocus = true;
			this.newReleaseAvailableButton.Name = "newReleaseAvailableButton";
			this.newReleaseAvailableButton.UseUnderline = true;
			this.newReleaseAvailableButton.Relief = ((global::Gtk.ReliefStyle)(1));
			this.newReleaseAvailableButton.Label = global::Mono.Unix.Catalog.GetString("A new release is available");
			this.hbox2.Add(this.newReleaseAvailableButton);
			global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.newReleaseAvailableButton]));
			w6.Position = 2;
			w6.Expand = false;
			w6.Fill = false;
			w6.Padding = ((uint)(2));
			this.vbox1.Add(this.hbox2);
			global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
			w7.Position = 1;
			w7.Expand = false;
			w7.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox3 = new global::Gtk.HBox();
			this.hbox3.Name = "hbox3";
			this.hbox3.Spacing = 6;
			// Container child hbox3.Gtk.Box+BoxChild
			this.label2 = new global::Gtk.Label();
			this.label2.Name = "label2";
			this.label2.LabelProp = global::Mono.Unix.Catalog.GetString("Projects to Expand");
			this.hbox3.Add(this.label2);
			global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.label2]));
			w8.Position = 0;
			w8.Expand = false;
			w8.Fill = false;
			w8.Padding = ((uint)(6));
			// Container child hbox3.Gtk.Box+BoxChild
			this.collapseEntry = new global::Gtk.Entry();
			this.collapseEntry.TooltipMarkup = "Separate by space, colon, semicolon";
			this.collapseEntry.CanFocus = true;
			this.collapseEntry.Name = "collapseEntry";
			this.collapseEntry.IsEditable = true;
			this.collapseEntry.HasFrame = false;
			this.collapseEntry.InvisibleChar = '●';
			this.hbox3.Add(this.collapseEntry);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.collapseEntry]));
			w9.Position = 1;
			w9.Padding = ((uint)(6));
			// Container child hbox3.Gtk.Box+BoxChild
			this.collapseButton = new global::Gtk.Button();
			this.collapseButton.CanFocus = true;
			this.collapseButton.Name = "collapseButton";
			this.collapseButton.UseUnderline = true;
			this.collapseButton.FocusOnClick = false;
			this.collapseButton.Relief = ((global::Gtk.ReliefStyle)(1));
			this.collapseButton.Label = global::Mono.Unix.Catalog.GetString("Apply");
			this.hbox3.Add(this.collapseButton);
			global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox3[this.collapseButton]));
			w10.Position = 2;
			w10.Expand = false;
			w10.Fill = false;
			w10.Padding = ((uint)(2));
			this.vbox1.Add(this.hbox3);
			global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox3]));
			w11.Position = 3;
			w11.Expand = false;
			w11.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox4 = new global::Gtk.HBox();
			this.hbox4.Name = "hbox4";
			this.hbox4.Spacing = 6;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ResetPinnedButton = new global::Gtk.Button();
			this.ResetPinnedButton.CanFocus = true;
			this.ResetPinnedButton.Name = "ResetPinnedButton";
			this.ResetPinnedButton.UseUnderline = true;
			this.ResetPinnedButton.FocusOnClick = false;
			this.ResetPinnedButton.Label = global::Mono.Unix.Catalog.GetString("Reset Pinned Doc\'s");
			this.hbox4.Add(this.ResetPinnedButton);
			global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ResetPinnedButton]));
			w12.Position = 0;
			w12.Expand = false;
			w12.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.ReloadPropertiesButton = new global::Gtk.Button();
			this.ReloadPropertiesButton.CanFocus = true;
			this.ReloadPropertiesButton.Name = "ReloadPropertiesButton";
			this.ReloadPropertiesButton.UseUnderline = true;
			this.ReloadPropertiesButton.FocusOnClick = false;
			this.ReloadPropertiesButton.Label = global::Mono.Unix.Catalog.GetString("Reload Properties");
			this.hbox4.Add(this.ReloadPropertiesButton);
			global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.ReloadPropertiesButton]));
			w13.Position = 2;
			w13.Expand = false;
			w13.Fill = false;
			// Container child hbox4.Gtk.Box+BoxChild
			this.PinOpenDocumentsButton = new global::Gtk.Button();
			this.PinOpenDocumentsButton.TooltipMarkup = "Pin all open documents in the workbench";
			this.PinOpenDocumentsButton.CanFocus = true;
			this.PinOpenDocumentsButton.Name = "PinOpenDocumentsButton";
			this.PinOpenDocumentsButton.UseUnderline = true;
			this.PinOpenDocumentsButton.FocusOnClick = false;
			this.PinOpenDocumentsButton.Label = global::Mono.Unix.Catalog.GetString("Pin All Open Doc\'s");
			this.hbox4.Add(this.PinOpenDocumentsButton);
			global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox4[this.PinOpenDocumentsButton]));
			w14.Position = 4;
			w14.Expand = false;
			w14.Fill = false;
			this.vbox1.Add(this.hbox4);
			global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox4]));
			w15.Position = 4;
			w15.Expand = false;
			w15.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.newReleaseAvailableButton.Hide();
			this.Show();
			this.filterEntry.Changed += new global::System.EventHandler(this.OnFilterEntryChanged);
			this.button1.Clicked += new global::System.EventHandler(this.clearButton_Clicked);
			this.oneClickCheckbutton.Toggled += new global::System.EventHandler(this.oneClickCheckbutton_Toggled);
			this.newReleaseAvailableButton.Clicked += new global::System.EventHandler(this.NewReleaseAvailableButton_Clicked);
			this.collapseButton.Clicked += new global::System.EventHandler(this.collapseButton_Clicked);
			this.ResetPinnedButton.Clicked += new global::System.EventHandler(this.ResetPinnedDocuments_Clicked);
			this.ReloadPropertiesButton.Clicked += new global::System.EventHandler(this.ReloadPropertiesButton_Clicked);
			this.PinOpenDocumentsButton.Clicked += new global::System.EventHandler(this.PinOpenDocuments_Clicked);
		}
	}
}
