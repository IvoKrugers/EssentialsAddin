using Gdk;
using Gtk;

namespace Diagram.ClassGroups
{
	public class ClassGroupColorWidget : DrawingArea
	{
		public string Color { get; set; }

		protected override bool OnExposeEvent(EventExpose evnt)
		{
			if (string.IsNullOrEmpty(Color)) return false;

			using (var ctx = CairoHelper.Create(evnt.Window))
			{
				Xwt.Drawing.Color.TryParse(Color, out Xwt.Drawing.Color gtkColor);
				var color = new Cairo.Color(gtkColor.Red, gtkColor.Green, gtkColor.Blue);

				DrawBackground(ctx, color, evnt.Area);
				DrawBorder(ctx, color, evnt.Area);
			}

			return true;
		}

		void DrawBackground(Cairo.Context ctx, Cairo.Color color, Rectangle cell_area)
		{
			color.A = 0.5;
			ctx.SetSourceColor(color);
			ctx.Rectangle(cell_area.X, cell_area.Y, cell_area.Width, cell_area.Height);
			ctx.Fill();
		}

		void DrawBorder(Cairo.Context ctx, Cairo.Color color, Rectangle cell_area)
		{
			color.A = 1d;
			ctx.SetSourceColor(color);
			ctx.LineWidth = 3d;
			ctx.Rectangle(cell_area.X, cell_area.Y, cell_area.Width, cell_area.Height);
			ctx.Stroke();
		}
	}
}
