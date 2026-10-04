using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public class HorizontalGuideMarkerViewModel : GuideMarkerViewModel
    {
        public double Y { get; }

        public HorizontalGuideMarkerViewModel(double y, Brush brush, double thickness) : base(brush, thickness)
        {
            Y = y;
        }
    }
}
