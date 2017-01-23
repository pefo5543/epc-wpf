using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace EpcDashboard.Customers
{
    /// <summary>
    /// Interaction logic for AddEditCustomerView.xaml
    /// </summary>
    public partial class AddEditCustomerView : UserControl
    {
        public AddEditCustomerView()
        {
            InitializeComponent();
        }

        private void ClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color> e)
        {
            //ClrPcker_Background.SelectedColorText = "#" + ClrPcker_Background.SelectedColor.R.ToString() + ClrPcker_Background.SelectedColor.G.ToString() + ClrPcker_Background.SelectedColor.B.ToString();
        }
    }
}
