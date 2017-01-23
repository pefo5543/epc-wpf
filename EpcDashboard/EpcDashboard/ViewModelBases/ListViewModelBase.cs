using System.Windows;

namespace EpcDashboard.ViewModelBases
{
    /// <summary>
    /// Base class for all ListViewModels
    /// </summary>
    public class ListViewModelBase : ViewModelBase
    {
        private string _SearchInput;
        private bool _isActive;
        protected string _menuName;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set { SetProperty(ref _isActive, value); }
        }

        public string SearchInput
        {
            get { return _SearchInput; }
            set
            {
                SetProperty(ref _SearchInput, value);
                Filtering(_SearchInput);
            }
        }
        protected void OnClearSearch()
        {
            SearchInput = null;
        }

        public virtual void Filtering(string _SearchInput)
        {
        }

        /// <summary>
        /// <para>Returns a standard Yes/No confirmation dialogue for delete actions</para>
        /// </summary>
        internal MessageBoxResult DeleteConfirmationDialogue(string entityName)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure?", "Delete " + entityName, MessageBoxButton.YesNo,
                MessageBoxImage.Warning, MessageBoxResult.No);

            return messageBoxResult;
        }
    }
}
