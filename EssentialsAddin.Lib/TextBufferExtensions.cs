using System.Text;
using Gtk;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("EssentialsAddin.Unitttests")]

namespace EssentialsAddin.Lib
{
    public static class TextBufferExtensions
    {
        public static StringBuilder GetDebugTextFromBuffer(this TextBuffer buffer)
        {
            TextTag debugTag = new TextTag("TheDebugTagToBe");
            buffer.TagTable.Foreach(t => { if (t.Name == "debug") { debugTag = t; } });

            return GetTextFromBuffer(buffer, debugTag);
        }

        internal static StringBuilder GetTextFromBuffer(TextBuffer buffer, TextTag textTag)
        {
            var sb = new StringBuilder();
            var tagIter = buffer.StartIter;
            while (true)
            {
                if (!tagIter.IsStart || !tagIter.BeginsTag(textTag))
                {
                    if (!tagIter.ForwardToTagToggle(textTag))
                        break;
                }

                var startIter = tagIter;
                var stopIter = buffer.EndIter;
                if (tagIter.ForwardToTagToggle(textTag))
                    stopIter = tagIter;

                sb.AppendLine(buffer.GetText(startIter, stopIter, false));

                if (tagIter.IsEnd)
                    break;
            }
            return sb;
        }
    }
}
