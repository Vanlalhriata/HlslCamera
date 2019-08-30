using HlslCamera.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;

namespace HlslCamera
{
    public class MainViewModel : ObservableObject
    {
        public string HlslText { get; set; }
        public string FxcPath { get; set; }

        public ImageSource ImageSource
        {
            get { return GetPropertyValue<ImageSource>(); }
            set { SetPropertyValue(value); }
        }

        public ShaderEffect ImageEffect
        {
            get { return GetPropertyValue<ShaderEffect>(); }
            set { SetPropertyValue(value); }
        }

        public ICommand ExecuteCommand => new RelayCommand(execute);
        public ICommand OpenCameraCommand => new RelayCommand(openCamera);

        private CameraManager cameraManager;
        private Process process;
        private string fxPath = "Shader.fx";
        private string psPath = "Shader.ps";

        public MainViewModel()
        {
            cameraManager = new CameraManager();
            cameraManager.OnNewFrame += cameraManager_OnNewFrame;
            cameraManager.InitializeVideo();
        }
        
        public void SetImage(string fileName)
        {
            cameraManager.OnNewFrame -= cameraManager_OnNewFrame;
            cameraManager.StopVideo();

            // Wait for NewFrame events
            System.Threading.Tasks.Task.Delay(200).ContinueWith(t =>
                Application.Current.Dispatcher.Invoke(() => ImageSource = new BitmapImage(new Uri(fileName)))
            );
        }

        private void execute(object parameter)
        {
            try
            {
                File.WriteAllText(fxPath, HlslText);

                var fxcArguments = $"/O0 /Fc /Zi /T  ps_2_0 /Fo {psPath} {fxPath}";
                ProcessStartInfo startInfo = new ProcessStartInfo(FxcPath, fxcArguments);
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardError = true;
                startInfo.CreateNoWindow = true;

                disposeProcess();
                process = Process.Start(startInfo);
                process.EnableRaisingEvents = true;
                process.Exited += process_Exited;
            }
            catch (Exception exception)
            {
                disposeProcess();
                ErrorManager.NotifyError(exception.Message);
            }
        }

        private void openCamera(object parameter)
        {
            cameraManager.OnNewFrame += cameraManager_OnNewFrame;
            cameraManager.InitializeVideo();
        }

        private void cameraManager_OnNewFrame(object sender, System.EventArgs e)
        {
            try
            {
                var bitmapSource = Utils.ToBitmapSource(cameraManager.CurrentBitmap);
                bitmapSource.Freeze(); // avoid cross thread operations and prevents leaks
                Application.Current.Dispatcher.Invoke(() => ImageSource = bitmapSource);
            }
            catch (Exception exception)
            {
                ErrorManager.NotifyError(exception.Message);
            }
        }

        private void process_Exited(object sender, System.EventArgs e)
        {
            var error = process.StandardError.ReadToEnd();
            if (!string.IsNullOrEmpty(error))
            {
                ErrorManager.NotifyError(error);
            }
            else
            {
                var psUri = new Uri(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, psPath), UriKind.Absolute);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var shaderEffect = new CustomShaderEffect(psUri);
                    ImageEffect = shaderEffect;
                });
            }

            disposeProcess();
        }

        private void disposeProcess()
        {
            if (null != process)
            {
                process.Exited -= process_Exited;
                process.Close();
                process.Dispose();
                process = null;
            }
        }
    }
}
