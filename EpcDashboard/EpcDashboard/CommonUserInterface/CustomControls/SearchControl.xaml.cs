using System.Windows.Controls;

namespace EpcDashboard.CommonUserInterface.CustomControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
