using Catalyst.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Catalyst.Controls
{
    /// <summary>
    /// Interaction logic for PatternPieceCanvas.xaml
    /// </summary>
    public partial class PatternPieceCanvas : UserControl, INotifyPropertyChanged
    {
        private PathGeometry geometry;

        public static readonly DependencyProperty PatternPiecesProperty =
            DependencyProperty.Register(
                name: "PatternPieces",                                                                                  // The name of the property
                propertyType: typeof(IEnumerable<PatternPieceViewModel>),                                               // The type of the property
                ownerType: typeof(PatternPieceCanvas),                                                                  // The class that owns it
                typeMetadata: new PropertyMetadata(Enumerable.Empty<PatternPieceViewModel>(), OnPatternPiecesChanged)   // Optional default value & callback
            );

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<PatternPieceViewModel> PatternPieces
        {
            get => (IEnumerable<PatternPieceViewModel>)GetValue(PatternPiecesProperty);
            set => SetValue(PatternPiecesProperty, value);
        }

        public PathGeometry Geometry
        {
            get => geometry;
            set
            {
                geometry = value;
                OnPropertyChanged(nameof(Geometry));
            }
        }

        public PatternPieceCanvas()
        {
            geometry = new PathGeometry() { FillRule = FillRule.Nonzero };

            InitializeComponent();
        }

        private static void OnPatternPiecesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PatternPieceCanvas canvas)
            {
                if (e.OldValue is ObservableCollection<PatternPieceViewModel> oldPieces)
                {
                    foreach (var piece in oldPieces)
                    {
                        piece.PropertyChanged -= canvas.PatternPiece_PropertyChanged;
                        piece.AnchorPoints.CollectionChanged -= canvas.AnchorPoints_CollectionChanged;
                        canvas.Geometry.Figures.Remove(piece.PathFigure);
                        foreach (var anchorPoint in piece.AnchorPoints)
                        {
                            anchorPoint.PropertyChanged -= canvas.AnchorPoints_PropertyChanged;
                        }
                    }
                    oldPieces.CollectionChanged -= canvas.PatternPieces_CollectionChanged;
                }

                if (e.NewValue is ObservableCollection<PatternPieceViewModel> newPieces)
                {
                    foreach (var piece in newPieces)
                    {
                        piece.PropertyChanged += canvas.PatternPiece_PropertyChanged;
                        piece.AnchorPoints.CollectionChanged += canvas.AnchorPoints_CollectionChanged;
                        canvas.Geometry.Figures.Add(piece.PathFigure);
                        foreach (var anchorPoint in piece.AnchorPoints)
                        {
                            anchorPoint.PropertyChanged += canvas.AnchorPoints_PropertyChanged;
                        }
                    }
                    newPieces.CollectionChanged += canvas.PatternPieces_CollectionChanged;
                }
            }
        }

        private void PatternPieces_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var piece in e.NewItems)
                {
                    if (piece is PatternPieceViewModel patternPiece)
                    {
                        patternPiece.PropertyChanged += PatternPiece_PropertyChanged;
                        patternPiece.AnchorPoints.CollectionChanged += AnchorPoints_CollectionChanged;
                        Geometry.Figures.Add(patternPiece.PathFigure);
                        foreach (var anchorPoint in patternPiece.AnchorPoints)
                        {
                            anchorPoint.PropertyChanged += AnchorPoints_PropertyChanged;
                        }

                        if (patternPiece.AnchorPoints.FirstOrDefault() is AnchorPointViewModel firstAnchorPoint)
                        {
                            patternPiece.PathFigure.StartPoint = firstAnchorPoint.Point;
                        }
                    }
                }
            }

            if (e.OldItems is not null)
            {
                foreach (var piece in e.OldItems)
                {
                    if (piece is PatternPieceViewModel patternPiece)
                    {
                        patternPiece.PropertyChanged -= PatternPiece_PropertyChanged;
                        patternPiece.AnchorPoints.CollectionChanged -= AnchorPoints_CollectionChanged;
                        Geometry.Figures.Remove(patternPiece.PathFigure);
                        foreach (var anchorPoint in patternPiece.AnchorPoints)
                        {
                            anchorPoint.PropertyChanged -= AnchorPoints_PropertyChanged;
                        }
                    }
                }
            }
        }

        private void AnchorPoints_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PatternPieceViewModel? piece = PatternPieces.SingleOrDefault(x => x.IsSelected && !x.IsClosed);

            if (piece is not null)
            {
                if (e.OldItems is not null)
                {
                    foreach (AnchorPointViewModel anchorPoint in e.OldItems)
                    {
                        anchorPoint.PropertyChanged -= AnchorPoints_PropertyChanged;
                        RefreshPathFigureSegments(piece);
                    }

                    if (sender is ObservableCollection<AnchorPointViewModel> anchorPoints &&
                        anchorPoints.FirstOrDefault() is AnchorPointViewModel firstAnchorPoint)
                    {
                        piece.PathFigure.StartPoint = firstAnchorPoint.Point;
                    }
                }

                if (e.NewItems is not null)
                {
                    foreach (AnchorPointViewModel anchorPoint in e.NewItems)
                    {
                        anchorPoint.PropertyChanged += AnchorPoints_PropertyChanged;
                        RefreshPathFigureSegments(piece);
                    }

                    if (sender is ObservableCollection<AnchorPointViewModel> anchorPoints &&
                        anchorPoints.FirstOrDefault() is AnchorPointViewModel firstAnchorPoint)
                    {
                        piece.PathFigure.StartPoint = firstAnchorPoint.Point;
                    }
                }
            }
        }

        private void RefreshPathFigureSegments(PatternPieceViewModel piece)
        {
            //piece.PathFigure.Segments.Clear();
            //for (int i = 1; i <= piece.AnchorPoints.Count; i++)
            //{
            //    AnchorPointViewModel last = piece.AnchorPoints[i - 1];
            //    AnchorPointViewModel current = piece.AnchorPoints[i % piece.AnchorPoints.Count];

            //    if (last.IsHandle2Enabled || current.IsHandle1Enabled)
            //    {
            //        piece.PathFigure.Segments.Add(new LineSegment(current.Point, true));
            //    }
            //    else
            //    {
            //        piece.PathFigure.Segments.Add(new BezierSegment(last.Handle2, current.Handle1, current.Point, true));
            //    }
            //}
        }

        private void AnchorPoints_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            PatternPieceViewModel? piece = PatternPieces.SingleOrDefault(x => x.IsSelected && !x.IsClosed);

            if (sender is AnchorPointViewModel anchorPoint &&
                piece is not null)
            {
                if (e.PropertyName == nameof(AnchorPointViewModel.Handle1) || e.PropertyName == nameof(AnchorPointViewModel.IsHandle1Enabled))
                {
                    int indexOfPreviousAnchor = (piece.AnchorPoints.IndexOf(anchorPoint) - 1 + piece.AnchorPoints.Count) % piece.AnchorPoints.Count;
                    if (indexOfPreviousAnchor != -1)
                    {
                        AnchorPointViewModel previousAnchor = piece.AnchorPoints[indexOfPreviousAnchor];
                        if (!previousAnchor.IsHandle2Enabled && !anchorPoint.IsHandle1Enabled)
                        {
                            if (previousAnchor.OutPathSegment is null)
                            {
                                previousAnchor.OutPathSegment = anchorPoint.InPathSegment = new LineSegment(anchorPoint.Point, true);
                                piece.PathFigure.Segments.Add(previousAnchor.OutPathSegment);
                            }
                            if (previousAnchor.OutPathSegment is not LineSegment lineSegment)
                            {
                                int segmentIndex = piece.PathFigure.Segments.IndexOf(previousAnchor.OutPathSegment);
                                previousAnchor.OutPathSegment = anchorPoint.InPathSegment = new LineSegment(anchorPoint.Point, true);
                                piece.PathFigure.Segments.Insert(segmentIndex, previousAnchor.OutPathSegment);
                            }
                            else
                            {
                                lineSegment.SetCurrentValue(LineSegment.PointProperty, anchorPoint.Point);
                            }
                        }
                        else
                        {
                            if (previousAnchor.OutPathSegment is null)
                            {
                                previousAnchor.OutPathSegment = anchorPoint.InPathSegment = new BezierSegment(previousAnchor.Handle2, anchorPoint.Handle1, anchorPoint.Point, true);
                                piece.PathFigure.Segments.Add(previousAnchor.OutPathSegment);
                            }
                            else if (previousAnchor.OutPathSegment is not BezierSegment bezierSegment)
                            {
                                int segmentIndex = piece.PathFigure.Segments.IndexOf(previousAnchor.OutPathSegment);
                                previousAnchor.OutPathSegment = anchorPoint.InPathSegment = new BezierSegment(previousAnchor.Handle2, anchorPoint.Handle1, anchorPoint.Point, true);
                                piece.PathFigure.Segments.Insert(segmentIndex, previousAnchor.OutPathSegment);
                            }
                            else
                            {
                                bezierSegment.SetCurrentValue(BezierSegment.Point1Property, previousAnchor.Handle2);
                                bezierSegment.SetCurrentValue(BezierSegment.Point2Property, anchorPoint.Handle1);
                                bezierSegment.SetCurrentValue(BezierSegment.Point3Property, anchorPoint.Point);
                            }       // TODO: everytime an anchorpoint is added at the end. It needs to move the outPathSegment that connects to the first point...
                        }
                    }
                }
                else if (e.PropertyName == nameof(AnchorPointViewModel.Handle2) || e.PropertyName == nameof(AnchorPointViewModel.IsHandle2Enabled))
                {
                    int indexOfNextAnchor = (piece.AnchorPoints.IndexOf(anchorPoint) + 1) % piece.AnchorPoints.Count;
                    if (indexOfNextAnchor != -1)
                    {
                        AnchorPointViewModel nextAnchor = piece.AnchorPoints[indexOfNextAnchor];
                        if (!nextAnchor.IsHandle1Enabled && !anchorPoint.IsHandle2Enabled)
                        {
                            if (nextAnchor.InPathSegment is null)
                            {
                                nextAnchor.InPathSegment = anchorPoint.OutPathSegment = new LineSegment(nextAnchor.Point, true);
                                piece.PathFigure.Segments.Add(nextAnchor.InPathSegment);
                            }
                            else if (nextAnchor.InPathSegment is not LineSegment lineSegment)
                            {
                                int segmentIndex = piece.PathFigure.Segments.IndexOf(nextAnchor.InPathSegment);
                                nextAnchor.InPathSegment = anchorPoint.OutPathSegment = new LineSegment(nextAnchor.Point, true);
                                piece.PathFigure.Segments.Insert(segmentIndex, nextAnchor.InPathSegment);
                            }
                            else
                            {
                                lineSegment.SetCurrentValue(LineSegment.PointProperty, nextAnchor.Point);
                            }
                        }
                        else
                        {
                            if (nextAnchor.InPathSegment is null)
                            {
                                nextAnchor.InPathSegment = anchorPoint.OutPathSegment = new BezierSegment(anchorPoint.Handle2, nextAnchor.Handle1, nextAnchor.Point, true);
                                piece.PathFigure.Segments.Add(nextAnchor.InPathSegment);
                            }
                            else if (nextAnchor.InPathSegment is not BezierSegment bezierSegment)
                            {
                                int segmentIndex = piece.PathFigure.Segments.IndexOf(nextAnchor.InPathSegment);
                                nextAnchor.InPathSegment = anchorPoint.OutPathSegment = new BezierSegment(anchorPoint.Handle2, nextAnchor.Handle1, nextAnchor.Point, true);
                                piece.PathFigure.Segments.Insert(segmentIndex, nextAnchor.InPathSegment);
                            }
                            else
                            {
                                bezierSegment.SetCurrentValue(BezierSegment.Point1Property, anchorPoint.Handle2);
                                bezierSegment.SetCurrentValue(BezierSegment.Point2Property, nextAnchor.Handle1);
                                bezierSegment.SetCurrentValue(BezierSegment.Point3Property, nextAnchor.Point);
                            }
                        }
                    }
                }
            }
        }

        private void PatternPiece_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

    }
}