using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace EpcDashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void APP_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            char[] a = e.Exception.StackTrace.ToCharArray();
            byte[] b = Encoding.GetEncoding("UTF-8").GetBytes(a);
            string tempDirectory = Path.GetTempPath();
            string fileName = "epcDump";
            string path = Path.Combine(tempDirectory, fileName);
            if (!Directory.Exists(tempDirectory))
            {
                Directory.CreateDirectory(tempDirectory);
            }
            using (FileStream fs = File.Create(path))
            {
                fs.Write(b, 0, b.Length);
            }
            e.Handled = true;
        }
    }
}
