using EpcDashboard.MVVMHelpers;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EpcDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = ContainerHelper.Container.Resolve<IMainWindowViewModel>();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        //protected override void OnStateChanged(EventArgs e)
        //{
        //    if (WindowState == WindowState.Maximized)
        //    {
        //        MinWidth = 0;
        //        MinHeight = 0;
        //        MaxWidth = int.MaxValue;
        //        MaxHeight = int.MaxValue;

        //        if (!m_isDuringMaximizing)
        //        {
        //            m_isDuringMaximizing = true;
        //            WindowState = WindowState.Normal;
        //            WindowState = WindowState.Maximized;
        //            m_isDuringMaximizing = false;
        //        }
        //    }
        //    else if (!m_isDuringMaximizing)
        //    {
        //        MinWidth = 1024;
        //        MinHeight = 768;
        //        MaxWidth = 1024;
        //        MaxHeight = 768;
        //    }

        //    base.OnStateChanged(e);
        //}
    }
}
