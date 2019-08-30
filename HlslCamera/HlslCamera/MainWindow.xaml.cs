using Microsoft.Win32;
using System.Windows;

namespace HlslCamera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void openImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png | All file (*.*) | *.*";
            bool? result = dialog.ShowDialog();
            if (true == result)
            {
                ((MainViewModel)DataContext).SetImage(dialog.FileName);
                cameraButton.Visibility = Visibility.Visible;
            }
        }

        private void openCamera_Click(object sender, RoutedEventArgs e)
        {
            cameraButton.Visibility = Visibility.Collapsed;
        }
    }
}
