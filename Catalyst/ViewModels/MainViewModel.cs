using Catalyst.Enum;
using Catalyst.WPF.Utilities;
using Catalyst.WPF.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Catalyst.Extensions;
using System.Windows.Media;
using System.Windows.Input;

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
        private PredefinedBrush? majorMarkerBrush = PredefinedBrushCollector.Instance.GetPredefinedBrush("Black");
        private PredefinedBrush? minorMarkerBrush = PredefinedBrushCollector.Instance.GetPredefinedBrush("DarkGray");
        private double majorMarkerThickness = 2D;
        private double minorMarkerThickness = 1D;
        private double zoom = 1D;
        private const double AnchorPointHitRadiusInPixels = 7D;
        private Tool tool = Tool.Pen;
        private Point? lastMousePosition = null;
        private bool isEndingPiece = false;
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

        public PredefinedBrush? MajorMarkerBrush
        {
            get => majorMarkerBrush;
            set
            {
                majorMarkerBrush = value;
                ResetGuideLines();
                OnPropertyChanged(nameof(MajorMarkerBrush));
            }
        }

        public PredefinedBrush? MinorMarkerBrush
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

        public Tool Tool
        {
            get => tool;
            set
            {
                tool = value;
                OnPropertyChanged(nameof(Tool));
            }
        }

        public double Zoom
        {
            get => zoom;
            set
            {
                zoom = value;
                OnPropertyChanged(nameof(Zoom));
            }
        }

        public ObservableCollection<HorizontalGuideMarkerViewModel> HorizontalGuideMarkers { get; } = [];
        public ObservableCollection<VerticalGuideMarkerViewModel> VerticalGuideMarkers { get; } = [];
        public ObservableCollection<GuideMarkerType> GuideMarkerTypes { get; } = [];
        public ObservableCollection<SnapType> SnapTypes { get; } = [];
        public ObservableCollection<PatternPieceViewModel> PatternPieces { get; } = [];
        #endregion

        public MainViewModel()
        {
            GuideMarkerTypes.AddRange(System.Enum.GetValues<GuideMarkerType>());
            SnapTypes.AddRange(System.Enum.GetValues<SnapType>());
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
                    VerticalGuideMarkers.Add(new VerticalGuideMarkerViewModel(x, MajorMarkerBrush?.Brush ?? Brushes.Black, MajorMarkerThickness));
                }
                else
                {
                    VerticalGuideMarkers.Add(new VerticalGuideMarkerViewModel(x, MinorMarkerBrush?.Brush ?? Brushes.DarkGray, MinorMarkerThickness));
                }
                i++;
            }

            i = 1;

            for (double y = pixelsPerUnitY; y < 2000; y += pixelsPerUnitY)
            {
                if (i % 5 == 0)
                {
                    HorizontalGuideMarkers.Add(new HorizontalGuideMarkerViewModel(y, MajorMarkerBrush?.Brush ?? Brushes.Black, 2D));
                }
                else
                {
                    HorizontalGuideMarkers.Add(new HorizontalGuideMarkerViewModel(y, MinorMarkerBrush?.Brush ?? Brushes.DarkGray, 1d));
                }
                i++;
            }
        }

        internal void PatternCanvas_MouseLeftButtonDown(Point p)
        {
            switch (Tool)
            {
                case Tool.Select:
                    break;
                case Tool.FineSelect:
                    break;
                case Tool.Pen:
                    PenLeftButtonDown(p);
                    break;
            }
        }

        internal void PatternCanvas_MouseLeftButtonUp(Point p)
        {
            switch (Tool)
            {
                case Tool.Select:
                    break;
                case Tool.FineSelect:
                    break;
                case Tool.Pen:
                    PenLeftButtonUp(p);
                    break;
            }
        }

        private void PenLeftButtonUp(Point p)
        {
            if (isEndingPiece)
            {
                if (PatternPieces.SingleOrDefault(x => x.IsSelected && !x.IsClosed) is PatternPieceViewModel selectedPiece)
                {
                    selectedPiece.IsClosed = true;
                    foreach (var patternPiece in PatternPieces)
                    {
                        patternPiece.IsSelected = false;
                    }
                    isEndingPiece = false;
                }
            }
        }

        private void PenLeftButtonDown(Point p)
        {
            if (PatternPieces.SingleOrDefault(x => x.IsSelected && !x.IsClosed) is PatternPieceViewModel selectedPiece)
            {
                if (selectedPiece.AnchorPoints.Count > 1 && 
                    selectedPiece.AnchorPoints[0].Point.DistanceToPoint(p) < AnchorPointHitRadiusInPixels)
                {
                    isEndingPiece = true;
                    return;
                }

                AnchorPointViewModel lastAnchorPoint = selectedPiece.AnchorPoints[^1];
                if (selectedPiece.AnchorPoints.Count > 0 &&
                    lastAnchorPoint.Point.DistanceToPoint(p) < AnchorPointHitRadiusInPixels)
                {
                    lastAnchorPoint.IsHandle2Enabled = false;
                    return;
                }

                AnchorPointViewModel newAnchorPoint = new AnchorPointViewModel(p)
                {
                    Handle1 = p,
                    Handle2 = p,
                    IsHandle1Enabled = false,
                    IsHandle2Enabled = false
                };

                selectedPiece.AnchorPoints.Add(newAnchorPoint);
            }
            else
            {
                foreach (var patternPiece in PatternPieces)
                {
                    patternPiece.IsSelected = false;
                }

                PatternPieceViewModel newPatternPiece = new()
                {
                    Name = $"Pattern Piece {PatternPieces.Count + 1}",
                    IsSelected = true
                };

                AnchorPointViewModel newAnchorPoint = new(p)
                {
                    Handle1 = p,
                    Handle2 = p,
                    IsHandle1Enabled = false,
                    IsHandle2Enabled = false
                };

                PatternPieces.Add(newPatternPiece);
                newPatternPiece.AnchorPoints.Add(newAnchorPoint);
            }
        }

        internal void PatternCanvas_MouseMove(Point p, MouseEventArgs e)
        {
            switch (Tool)
            {
                case Tool.Select:
                    break;
                case Tool.FineSelect:
                    break;
                case Tool.Pen:
                    PenMouseMove(p, e);
                    break;
            }

            lastMousePosition = p;
        }

        private void PenMouseMove(Point p, MouseEventArgs e)
        {
            if (lastMousePosition is Point oldPos) 
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (PatternPieces.SingleOrDefault(x => x.IsSelected && !x.IsClosed) is PatternPieceViewModel selectedPiece &&
                        selectedPiece.AnchorPoints.Count > 1)
                    {
                        AnchorPointViewModel lastAnchorPoint = isEndingPiece ? selectedPiece.AnchorPoints[0] : selectedPiece.AnchorPoints[^1];
                        lastAnchorPoint.IsHandle1Enabled = true;
                        lastAnchorPoint.Handle1 = new(lastAnchorPoint.Handle1.X - p.X + oldPos.X, lastAnchorPoint.Handle1.Y - p.Y + oldPos.Y);
                    } 
                }
            }
        }
    }
}
