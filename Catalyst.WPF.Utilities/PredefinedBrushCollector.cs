using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text;
using System.Windows.Media;

namespace Catalyst.WPF.Utilities
{
    public class PredefinedBrushCollector
    {
        private static PredefinedBrushCollector? instance = null;
        public static PredefinedBrushCollector Instance => instance ??= new();

        private Dictionary<string, PredefinedBrush> brushDictionary = [];

        public ObservableCollection<PredefinedBrush> Brushes { get; } = [];

        private PredefinedBrushCollector()
        {
            PropertyInfo[] properties = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);

            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(null, null) is SolidColorBrush brush)
                {
                    PredefinedBrush predefinedBrush = new(property.Name, brush);
                    Brushes.Add(predefinedBrush);
                    brushDictionary.Add(property.Name, predefinedBrush);
                }
            }
        }

        public PredefinedBrush? GetPredefinedBrush(string name) => brushDictionary.GetValueOrDefault(name);
    }
}
