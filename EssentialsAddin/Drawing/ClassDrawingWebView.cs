using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Diagram.Drawing.Data;
using Gtk;
using Newtonsoft.Json;
using WebKit;
using Xwt.GtkBackend;
using Foundation;
using CoreGraphics;
using Diagram.Storage;
using System.Diagnostics;

namespace Diagram.Drawing
{
	[System.ComponentModel.ToolboxItem(true)]
	class ClassDrawingWebView : WKWebView
	{
		const string BASE_HTML_RESOURCE = "Diagram.Drawing.WebResources.drawing_area.html";
		const string VIS_JS_LIBRARY_RESOURCE = "Diagram.Drawing.WebResources.vis-network.min.js";
		const string VIS_JS_DUMMY_TEXT = "REPLACE_ME_WITH_VIS_JS";
		const string CONVEX_HULL_JS_LIBRARY_RESOURCE = "Diagram.Drawing.WebResources.convex-hull.js";
		const string CONVEX_HULL_JS_DUMMY_TEXT = "REPLACE_ME_WITH_CONVEX_HULL_JS";

		const string OPTIONS_DUMMY_TEXT = "REPLACE_ME_WITH_OPTIONS";
		const string DUMMY_URL = "https://classes";

		static string baseHtml = MakeBaseHtml();

		public event NodeEventDelegate NodeDragged;
		public event NodeEventDelegate NodeContextClicked;

		public Widget GtkWidget => gtkWidget ?? (gtkWidget = GtkMacInterop.NSViewToGtkWidget(this));
		Widget gtkWidget;

		public bool DrawClassGroups
		{
			get => drawClassGroups;
			set
			{
				if (drawClassGroups == value) return;

				drawClassGroups = value;
				EvaluateJavaScript($"network.{(value ? "on" : "off")}('beforeDrawing', drawGroupBorders);", (r, e) => DrawNetwork());
			}
		}
		bool drawClassGroups;

		public ClassDrawingWebView() : base(new CGRect(), new WKWebViewConfiguration())
		{
			InitializeNetwork();
			SetupEvents();
		}

		void InitializeNetwork()
		{
			var options = JsonConvert.SerializeObject(new Options());
			var html = baseHtml.Replace(OPTIONS_DUMMY_TEXT, options);
			LoadHtmlString(html, new NSUrl(DUMMY_URL));
		}

		void SetupEvents()
		{
			var eventListener = new VisJsNodeDraggedEventListener();
			eventListener.NodeDragged += (e) => NodeDragged.Invoke(e);
			eventListener.NodeContextClicked += (e) => NodeContextClicked.Invoke(e);
			Configuration.UserContentController.AddScriptMessageHandler(eventListener, "eventListener");
		}

		public void Draw(List<ClassDefinition> classes, StoredDiagramDetails storedDetails)
		{
			var classGroups = JsonConvert.SerializeObject(storedDetails.classGroups);
			SetClassGroups(classGroups);
			var data = VisJsDataBuilder.Instance.MakeData(classes, storedDetails.nodes);
			SetNetworkData(data);
			DrawNetwork();
		}

		public void Clear()
		{
			SetNetworkData("{}");
			SetClassGroups("{}");
			DrawNetwork();
		}

		void SetNetworkData(string data) => EvaluateJavaScript($"data = {data}; network.setData(data);", null);
		
		void SetClassGroups(string classGroups) => EvaluateJavaScript($"classGroups = {classGroups};", null);

		void DrawNetwork() => EvaluateJavaScript($"network.redraw();", (r, e) => {
			Debug.WriteLine(r + "\n" + e);
		});

		static string MakeBaseHtml()
		{
			return LoadStringFromResource(BASE_HTML_RESOURCE)
				.Replace(VIS_JS_DUMMY_TEXT, LoadStringFromResource(VIS_JS_LIBRARY_RESOURCE))
				.Replace(CONVEX_HULL_JS_DUMMY_TEXT, LoadStringFromResource(CONVEX_HULL_JS_LIBRARY_RESOURCE));
		}

		static string LoadStringFromResource(string id)
		{
			var assembly = Assembly.GetExecutingAssembly();
			using (var stream = assembly.GetManifestResourceStream(id))
			using (StreamReader reader = new StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}
}