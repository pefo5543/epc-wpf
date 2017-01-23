using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EpcDashboard.ViewModelBases
{
    public class BindableBase : INotifyPropertyChanged
    {

        protected virtual void SetProperty<T>(ref T member, T val,
            [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string _NotificationMessage;

        public string NotificationMessage
        {
            get { return _NotificationMessage; }
            set
            {
                _NotificationMessage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("NotificationMessage"));
            }
        }
    }
}

//    #region Debugging Aides

//    /// <summary>
//    /// Warns the developer if this object does not have
//    /// a public property with the specified name. This
//    /// method does not exist in a Release build.
//    /// </summary>
//    [Conditional("DEBUG")]
//    [DebuggerStepThrough]
//    public virtual void VerifyPropertyName(string propertyName)
//    {
//        // Verify that the property name matches a real,
//        // public, instance property on this object.
//        if (TypeDescriptor.GetProperties(this)[propertyName] == null)
//        {
//            string msg = "Invalid property name: " + propertyName;

//            if (this.ThrowOnInvalidPropertyName)
//                throw new Exception(msg);
//            else
//                Debug.Fail(msg);
//        }
//    }

//    /// <summary>
//    /// Returns whether an exception is thrown, or if a Debug.Fail() is used
//    /// when an invalid property name is passed to the VerifyPropertyName method.
//    /// The default value is false, but subclasses used by unit tests might
//    /// override this property's getter to return true.
//    /// </summary>
//    protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

//    #endregion // Debugging Aides
//}
