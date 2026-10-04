using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Catalyst.WPF.Utilities;

namespace Catalyst.WPF.Themes.ViewModels
{
    public class ThemeViewModel : ObservableObject
    {
        private readonly ResourceDictionary resourceDictionary;

        public string Name { get; set; }

        internal ThemeViewModel(string name, ResourceDictionary resourceDictionary)
        {
            this.resourceDictionary = resourceDictionary;
            Name = name;
        }

        public void Load()
        {
            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }
    }
}
