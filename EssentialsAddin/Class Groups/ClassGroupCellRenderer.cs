using Diagram.Drawing.Data;
using Gdk;
using Gtk;

namespace Diagram.ClassGroups
{
	public class ClassGroupCellRenderer : CellRenderer
	{
		public ClassGroup ClassGroup { get; set; }

		public override void GetSize(Widget widget, ref Rectangle cell_area, out int x_offset, out int y_offset, out int width, out int height)
		{
			x_offset = cell_area.X;
			y_offset = cell_area.Y;
			width = ClassGroup == null ? 0 : 30;
			height = ClassGroup == null ? 0 : 20;
		}

		protected override void Render(Drawable window, Widget widget, Rectangle background_area, Rectangle cell_area, Rectangle expose_area, CellRendererState flags)
		{
			if (ClassGroup == null) return;

			using (var ctx = CairoHelper.Create(window))
			{
				Xwt.Drawing.Color gtkColor;
				Xwt.Drawing.Color.TryParse(ClassGroup.color, out gtkColor);
				var color = new Cairo.Color(gtkColor.Red, gtkColor.Green, gtkColor.Blue);

				DrawBackground(ctx, color, cell_area);
				DrawBorder(ctx, color, cell_area);
			}
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
