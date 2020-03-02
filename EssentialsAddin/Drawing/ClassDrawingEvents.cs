using System;
using Foundation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebKit;

namespace Diagram.Drawing
{
	delegate void NodeEventDelegate(NodeEventArgs e);

	class VisJsNodeDraggedEventListener : WKScriptMessageHandler
	{
		public event NodeEventDelegate NodeDragged;
		public event NodeEventDelegate NodeContextClicked;

		public override void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
		{
			var body = (message.Body as NSString).ToString();
			var args = JsonConvert.DeserializeObject<NodeEventArgs>(body);

			switch (args.type)
			{
				case NodeEvent.Dragged:
					NodeDragged?.Invoke(args);
					break;
				case NodeEvent.ContextClicked:
					NodeContextClicked?.Invoke(args);
					break;
			}
		}
	}

	public class NodeEventArgs : EventArgs
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public NodeEvent type { get; set; }
		public int x { get; set; }
		public int y { get; set; }
		public string nodeId { get; set; }
	}

	public enum NodeEvent
	{
		Dragged,
		ContextClicked
	}
}