using Catalyst.WPF.Themes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Catalyst.WPF.Window
{
    public abstract class CatalystWindow : System.Windows.Window
    {
        private Button? noramlizeButton, maximizeButton, minimizeButton, exitButton;
        private Grid? windowGrid;

        private readonly DependencyProperty WindowIconProperty =
            DependencyProperty.Register(nameof(WindowIcon), typeof(object), typeof(CatalystWindow),
                new PropertyMetadata(null));

        private readonly DependencyProperty MenuProperty =
            DependencyProperty.Register(nameof(Menu), typeof(object), typeof(CatalystWindow),
                new PropertyMetadata(null));

        public object WindowIcon { get => GetValue(WindowIconProperty); set => SetValue(WindowIconProperty, value); }
        public object Menu { get => GetValue(MenuProperty); set => SetValue(MenuProperty, value); }

        public CatalystWindow()
        {
            Uri foo = new("pack://application:,,,/Catalyst.WPF.Window;component/CatalystWindow.xaml", UriKind.RelativeOrAbsolute);
            ResourceDictionary rd = new() { Source = foo };

            if (rd["CatalystWindowStyleKey"] is Style customWindowStyle)
            {
                Style = customWindowStyle;
                ApplyTemplate();
            }

            Theme.LoadControlTemplates();

            Loaded += CustomWindow_Loaded;
        }

        private void CustomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Button button in FindVisualChildren<Button>(this))
            {
                if (button.Name == "minimizeButton")
                {
                    minimizeButton = button;
                    minimizeButton.Click += MinimizeButton_Click;
                }
                else if (button.Name == "normalizeButton")
                {
                    noramlizeButton = button;
                    noramlizeButton.Click += NoramlizeButton_Click;
                }
                else if (button.Name == "maximizeButton")
                {
                    maximizeButton = button;
                    maximizeButton.Click += MaximizeButton_Click;
                }
                else if (button.Name == "exitButton")
                {
                    exitButton = button;
                    exitButton.Click += ExitButton_Click;
                }
            }

            windowGrid = FindVisualChildren<Grid>(this).FirstOrDefault(x => x.Name == "windowGrid");

            StateChanged += CatalystWindow_StateChanged;
            CatalystWindow_StateChanged(this, EventArgs.Empty);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }

        private void NoramlizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Normal;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CatalystWindow_StateChanged(object? sender, EventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    if (maximizeButton is not null)
                    {
                        maximizeButton.Visibility = Visibility.Visible;
                    }
                    if (noramlizeButton is not null)
                    {
                        noramlizeButton.Visibility = Visibility.Collapsed;
                    }
                    if (windowGrid is not null)
                    {
                        windowGrid.Margin = new(0);
                    }
                    break;
                case WindowState.Maximized:
                    if (noramlizeButton is not null)
                    {
                        noramlizeButton.Visibility = Visibility.Visible;
                    }
                    if (maximizeButton is not null)
                    {
                        maximizeButton.Visibility = Visibility.Collapsed;
                    }
                    if (windowGrid is not null)
                    {
                        windowGrid.Margin = new(8);
                    }
                    break;
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChildren<T>(ithChild)) yield return childOfChild;
            }
        }
    }
}
