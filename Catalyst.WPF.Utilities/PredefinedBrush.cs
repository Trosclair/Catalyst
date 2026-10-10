using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Catalyst.WPF.Utilities
{
    public class PredefinedBrush
    {
        public string Name { get; }
        public Brush Brush { get; }

        internal PredefinedBrush(string name, Brush brush)
        {
            Name = name;
            Brush = brush;
        }
    }
}
