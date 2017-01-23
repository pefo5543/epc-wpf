using EpcDashboard.CommonUserInterface.Converters;
using System.ComponentModel;

namespace EpcDashboard.Actions
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ActionOption
    {
        [Description("Start Empiri X3")]
        StartX3,
        [Description("Open Folder")]
        OpenFolder,
        [Description("Run Remote Desktop")]
        RunRDP,
        [Description("Run VNC")]
        RunVNC,
        [Description("Open EBMS")]
        OpenEBMS,
        [Description("Start .EXE")]
        StartExe,
    };
}
