using EpcDashboard.CustomValidations;
using EpcDashboard.MVVMHelpers;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Settings
{
    //Short lifetime object for data validation and holding user setting edits until Save button is clicked. 
    public class EditableSetting : ValidatableBindableBase
    {
        private string _userId;
        private string _lastName;
        private string _firstName;
        private bool _winStart;
        private string _sourcePath;

        [Required]
        //Custom validation
        [UserIdValidation("se_aaabbb",
        ErrorMessage = "{0} field validation failed.")]       
        public string UserId
        {
            get { return _userId; }
            set { SetProperty(ref _userId, value); }
        }
        [Required]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
        [Required]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        [Required]
        public string SourcePath
        {
            get { return _sourcePath; }
            set { SetProperty(ref _sourcePath, value); }
        }
        public bool WinStart
        {
            get { return _winStart; }
            set { SetProperty(ref _winStart, value); }
        }
    }
}
