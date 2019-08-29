using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HlslCamera
{
    public static class ErrorManager
    {
        public static void NotifyError(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
                MessageBox.Show(Application.Current.MainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            );
        }
    }
}
