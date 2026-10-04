using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public class VerticalGuideMarkerViewModel(double x, Brush brush, double thickness) : GuideMarkerViewModel(brush, thickness)
    {
        public double X { get; } = x;
    }
}
