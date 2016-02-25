using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ServiceCruiser.Model.Entities.Core.Extensibility
{
    public static class EnumerableExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }

        public static ObservableCollection<T> ToObservableCollection<T>(this ICollection<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
    }
}
