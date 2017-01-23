using EpcDashboard.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace EpcDashboard.Actions.ShorthandActions
{
    public class SimpleExe : EditableBaseAction
    {
        private string _fileName;

        private string _arguments;

        [FileValidation(new string[] { "exe" },
        ErrorMessage = "{0} must be of type .exe")]
        [FileSourceValidation(50000000, ErrorMessage = "{0} path does not exist or image is to big")] //Max size limit set to 50MB
        [Required]
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                SetProperty(ref _fileName, value);
            }
        }

        public string Arguments
        {
            get
            {
                return _arguments;
            }
            set
            {
                SetProperty(ref _arguments, value);
            }
        }
    }
}
