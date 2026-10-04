using System;
using System.Collections.Generic;
using System.Text;

namespace Catalyst.WPF.Utilities.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this System.Collections.ObjectModel.ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        public static void RemoveRange<T>(this System.Collections.ObjectModel.ObservableCollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                collection.Remove(item);
            }
        }
    }
}
