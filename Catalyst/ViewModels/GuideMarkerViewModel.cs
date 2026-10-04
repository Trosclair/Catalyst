using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public abstract class GuideMarkerViewModel
    {
        public Brush Brush { get; }
        public double Thickness { get; }

        public GuideMarkerViewModel(Brush brush, double thickness)
        {
            Brush = brush;
            Thickness = thickness;
        }
    }
}
