using System;
using Gtk;

namespace Diagram
{
    [System.ComponentModel.ToolboxItem(true)]
    public class DrawingProgressBar : ProgressBar
    {
        public Progress<TreeBuilderProgress> Progress
        {
            get => progress;
            set
            {
                if (progress == value) return;

                progress = value;
                progress.ProgressChanged += OnProgressChanged;
            }
        }
        Progress<TreeBuilderProgress> progress;

        void OnProgressChanged(object sender, TreeBuilderProgress p)
        {
            double progressValue = (double)p.doneClasses / p.totalClasses;
            Fraction = progressValue;
            Text = progressValue.Equals(1d) ? "Done" : "Generating diagram...";
        }

        public void Clear()
        {
            Text = string.Empty;
            Fraction = 0;
        }
    }
}
