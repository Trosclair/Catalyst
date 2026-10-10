using Catalyst.WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Catalyst.ViewModels
{
    public class AnchorPointViewModel : ObservableObject
    {
        private Point point = new();
        private Point handle1 = new();
        private Point handle2 = new();
        private bool isHandle1Enabled = false;
        private bool isHandle2Enabled = false;

        PathSegment? inPathSegment;
        PathSegment? outPathSegment;

        public Point Point
        {
            get => point;
            set
            {
                point = value;
                OnPropertyChanged(nameof(Point));
            }
        }

        public Point Handle1
        {
            get => handle1;
            set
            {
                handle1 = value;
                OnPropertyChanged(nameof(Handle1));
            }
        }

        public Point Handle2
        {
            get => handle2;
            set
            {
                handle2 = value;
                OnPropertyChanged(nameof(Handle2));
            }
        }

        public bool IsHandle1Enabled
        {
            get => isHandle1Enabled;
            set
            {
                isHandle1Enabled = value;
                OnPropertyChanged(nameof(IsHandle1Enabled));
            }
        }

        public bool IsHandle2Enabled
        {
            get => isHandle2Enabled;
            set
            {
                isHandle2Enabled = value;
                OnPropertyChanged(nameof(IsHandle2Enabled));
            }
        }

        public PathSegment? InPathSegment
        {
            get => inPathSegment;
            set
            {
                inPathSegment = value;
                OnPropertyChanged(nameof(InPathSegment));
            }
        }

        public PathSegment? OutPathSegment
        {
            get => outPathSegment;
            set
            {
                outPathSegment = value;
                OnPropertyChanged(nameof(OutPathSegment));
            }
        }

        public AnchorPointViewModel(Point point)
        {
            Point = point;
        }

        public AnchorPointViewModel Clone()
        {
            return new AnchorPointViewModel(Point)
            {
                Handle1 = Handle1,
                Handle2 = Handle2,
                IsHandle1Enabled = IsHandle1Enabled,
                IsHandle2Enabled = IsHandle2Enabled
            };
        }
    }
}
