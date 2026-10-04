using Catalyst.Enum;
using Catalyst.WPF.Utilities;
using Catalyst.WPF.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        #region Constants
        private const double CentimetersPerInch = 2.54D;
        #endregion

        #region Fields
        private GuideMarkerType guideMarkerType = GuideMarkerType.None;
        private bool isSnapOn = false;
        private SnapType snapType = SnapType.Inch;
        private double pixelTolerance = 96D;
        private double inchTolerance = 1D;
        private double centimeterTolerance = 1D;
        private Brush majorMarkerBrush = Brushes.Black;
        private Brush minorMarkerBrush = Brushes.DarkGray;
        private double majorMarkerThickness = 2D;
        private double minorMarkerThickness = 1D;
        #endregion

        #region Properties
        public GuideMarkerType GuideMarkerType
        { 
            get => guideMarkerType;
            set 
            {
                guideMarkerType = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(GuideMarkerType));
            }
        }

        public bool IsSnapOn
        {
            get => isSnapOn;
            set
            {
                isSnapOn = value;
                OnPropertyChanged(nameof(IsSnapOn));
            }
        }

        public SnapType SnapType
        {
            get => snapType;
            set
            {
                snapType = value;
                OnPropertyChanged(nameof(SnapType));
            }
        }

        public double PixelTolerance
        {
            get => pixelTolerance;
            set
            {
                pixelTolerance = value;
                OnPropertyChanged(nameof(PixelTolerance));
            }
        }

        public double InchTolerance
        {
            get => inchTolerance;
            set
            {
                inchTolerance = value;
                OnPropertyChanged(nameof(InchTolerance));
            }
        }

        public double CentimeterTolerance
        {
            get => centimeterTolerance;
            set
            {
                centimeterTolerance = value;
                OnPropertyChanged(nameof(CentimeterTolerance));
            }
        }

        public Brush MajorMarkerBrush
        {
            get => majorMarkerBrush;
            set
            {
                majorMarkerBrush = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(MajorMarkerBrush));
            }
        }

        public Brush MinorMarkerBrush
        {
            get => minorMarkerBrush;
            set
            {
                minorMarkerBrush = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(MinorMarkerBrush));
            }
        }

        public double MajorMarkerThickness
        {
            get => majorMarkerThickness;
            set
            {
                majorMarkerThickness = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(MajorMarkerThickness));
            }
        }

        public double MinorMarkerThickness
        {
            get => minorMarkerThickness;
            set
            {
                minorMarkerThickness = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(MinorMarkerThickness));
            }
        }

        public ObservableCollection<HorizontalGuideMarkerViewModel> HorizontalGuideMarkers { get; } = [];
        public ObservableCollection<VerticalGuideMarkerViewModel> VerticalGuideMarkers { get; } = [];
        public ObservableCollection<GuideMarkerType> GuideMarkerTypes { get; } = [];
        #endregion

        public MainViewModel()
        {
            GuideMarkerTypes.AddRange(System.Enum.GetValues<GuideMarkerType>());
            GuideMarkerType = GuideMarkerType.Imperial;
        }

        private void ResetGuideLines()
        {
            HorizontalGuideMarkers.Clear();
            VerticalGuideMarkers.Clear();

            double pixelsPerUnitX = 0.0D;
            double pixelsPerUnitY = 0.0D;
            DpiScale dpiInfo = VisualTreeHelper.GetDpi(App.Current.MainWindow);

            switch (GuideMarkerType)
            {
                case GuideMarkerType.Imperial:
                    pixelsPerUnitX = dpiInfo.PixelsPerInchX / 5;
                    pixelsPerUnitY = dpiInfo.PixelsPerInchY / 5;
                    break;
                case GuideMarkerType.Metric:
                    pixelsPerUnitX = dpiInfo.PixelsPerInchX / CentimetersPerInch / 5;
                    pixelsPerUnitY = dpiInfo.PixelsPerInchY / CentimetersPerInch / 5;
                    break;
                default:
                    return;
            }


            int i = 1;
            for (double x = pixelsPerUnitX; x < 2000; x += pixelsPerUnitX)
            {
                if (i % 5 == 0)
                {
                    VerticalGuideMarkers.Add(new VerticalGuideMarkerViewModel(x, MajorMarkerBrush, MajorMarkerThickness));
                }
                else
                {
                    VerticalGuideMarkers.Add(new VerticalGuideMarkerViewModel(x, MinorMarkerBrush, MinorMarkerThickness));
                }
                i++;
            }

            i = 1;

            for (double y = pixelsPerUnitY; y < 2000; y += pixelsPerUnitY)
            {
                if (i % 5 == 0)
                {
                    HorizontalGuideMarkers.Add(new HorizontalGuideMarkerViewModel(y, Brushes.Black, 2D));
                }
                else
                {
                    HorizontalGuideMarkers.Add(new HorizontalGuideMarkerViewModel(y, Brushes.DarkGray, 1d));
                }
                i++;
            }
        }

    }
}
