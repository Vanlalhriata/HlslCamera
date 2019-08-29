using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace HlslCamera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CameraManager cameraManager;

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
            this.Loaded += mainWindow_Loaded;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            cameraManager = new CameraManager();
            cameraManager.OnNewFrame += cameraManager_OnNewFrame;
            cameraManager.InitializeVideo();
        }

        private void cameraManager_OnNewFrame(object sender, System.EventArgs e)
        {
            try
            {
                BitmapSource bitmapSource;
                using (var bitmap = (Bitmap)cameraManager.CurrentBitmap.Clone())
                {
                    bitmapSource = Utils.ToBitmapSource(bitmap);
                }
                bitmapSource.Freeze(); // avoid cross thread operations and prevents leaks
                Application.Current.Dispatcher.BeginInvoke(new ThreadStart(delegate { image.Source = bitmapSource; }));
            }
            catch (Exception exception)
            {
                // TODO: Error
            }
        }

    }
}
