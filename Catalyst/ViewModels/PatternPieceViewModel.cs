using Catalyst.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public class PatternPieceViewModel : ObservableObject
    {
        private string name = "<Unnamed>";
        private Brush stroke = Brushes.SteelBlue;
        private Brush fill = new SolidColorBrush(Colors.LightBlue) { Opacity = 0.3 };
        private PathFigure pathFigure = new PathFigure();
        private bool isSelected = false;

        public ObservableCollection<AnchorPointViewModel> AnchorPoints { get; } = [];

        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsClosed
        {
            get => pathFigure.IsClosed;
            set
            {
                pathFigure.IsClosed = value;
                OnPropertyChanged(nameof(IsClosed));
            }
        }

        public Brush Stroke
        {
            get => stroke;
            set
            {
                stroke = value;
                OnPropertyChanged(nameof(Stroke));
            }
        }

        public Brush Fill
        {
            get => fill;
            set
            {
                fill = value;
                OnPropertyChanged(nameof(Fill));
            }
        }

        public PathFigure PathFigure
        {
            get => pathFigure;
            set
            {
                pathFigure = value;
                OnPropertyChanged(nameof(PathFigure));
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
