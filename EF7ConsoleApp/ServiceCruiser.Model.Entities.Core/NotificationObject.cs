using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace ServiceCruiser.Model.Entities.Core
{
    public abstract class NotificationObject : INotifyPropertyChanged
    {
        #region state
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region behavior
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null) return;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null) return;

            OnPropertyChanged(body.Member.Name);
        }

        protected virtual void OnPropertyChanged<T>(params Expression<Func<T>>[] propertyExpressions)
        {
            if (propertyExpressions == null) { throw new ArgumentNullException("propertyExpressions"); }

            foreach (var propertyExpression in propertyExpressions)
            {
                OnPropertyChanged(propertyExpression);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
