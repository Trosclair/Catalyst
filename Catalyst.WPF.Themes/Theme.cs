using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Catalyst.WPF.Themes.ViewModels;

namespace Catalyst.WPF.Themes
{
    public static class Theme
    {
        private static bool areControlTemplatesLoaded = false;
        private static readonly Lock controlTemplatesLock = new();

        public static void LoadControlTemplates()
        {
            lock (controlTemplatesLock)
            {
                if (!areControlTemplatesLoaded)
                {
                    ResourceDictionary controlDictionary = new()
                    {
                        Source = new Uri("pack://application:,,,/Catalyst.WPF.Themes;component/Controls/Controls.xaml", UriKind.RelativeOrAbsolute)
                    };

                    Application.Current.Resources.MergedDictionaries.Add(controlDictionary);

                    if (GetThemes().FirstOrDefault(x => x.Name == "Dark") is ThemeViewModel theme)
                    {
                        theme.Load();
                    }

                    areControlTemplatesLoaded = true;
                }
            }
        }

        public static IEnumerable<ThemeViewModel> GetThemes()
        {
            return
            [
                new("Dark", new() { Source = new Uri("pack://application:,,,/Catalyst.WPF.Themes;component/Themes/Dark.xaml", UriKind.RelativeOrAbsolute) }),
                new("Light", new() { Source = new Uri("pack://application:,,,/Catalyst.WPF.Themes;component/Themes/Light.xaml", UriKind.RelativeOrAbsolute) }),
            ];
        }
    }
}
