using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HlslCamera.Infrastructure
{
    /// <summary>
    /// An object that can raise PropertyChanged
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {
        private readonly Dictionary<string, object> propertyBackingDictionary = new Dictionary<string, object>();

        protected T GetPropertyValue<T>([CallerMemberName]string propertyName = null)
        {
            if (null == propertyName)
                throw new ArgumentNullException(nameof(propertyName));

            object value;
            if (propertyBackingDictionary.TryGetValue(propertyName, out value))
                return (T)value;

            return default(T);
        }

        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName]string propertyName = null)
        {
            if (null == propertyName)
                throw new ArgumentNullException(nameof(propertyName));

            if (EqualityComparer<T>.Default.Equals(newValue, GetPropertyValue<T>(propertyName)))
                return false;

            propertyBackingDictionary[propertyName] = newValue;
            RaisePropertyChanged(propertyName);

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
