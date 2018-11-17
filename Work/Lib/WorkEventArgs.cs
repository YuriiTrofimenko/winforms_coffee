using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Work.Lib
{
    public class WorkEventArgs : EventArgs
    {
        public Color BackColor { get; private set; }
        public WorkEventArgs() : this(SystemColors.Control) { }
        public WorkEventArgs(Color color)
        {
            BackColor = color;
        }
    }
}
