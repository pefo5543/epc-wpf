using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EpcDashboard.MVVMHelpers
{
    public class ShowNotificationMessageBehaviour : Behavior<ContentControl>
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), 
                typeof(ShowNotificationMessageBehaviour), new PropertyMetadata(null, OnMessageChanged));

        private static void OnMessageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behaviour = ((ShowNotificationMessageBehaviour)d);
            behaviour.AssociatedObject.Content = e.NewValue;
            behaviour.AssociatedObject.Visibility = Visibility.Visible;
        }
        protected override void OnAttached()
        {
            AssociatedObject.MouseLeftButtonDown += (s, e) =>
            AssociatedObject.Visibility = Visibility.Collapsed;
            
        }
    }
}
