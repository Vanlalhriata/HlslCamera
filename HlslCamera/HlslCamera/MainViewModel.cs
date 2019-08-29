using HlslCamera.Infrastructure;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace HlslCamera
{
    public class MainViewModel : ObservableObject
    {
        public string HlslText { get; set; }
        public string FxcPath { get; set; }

        public ShaderEffect ImageEffect
        {
            get { return GetPropertyValue<ShaderEffect>(); }
            set { SetPropertyValue(value); }
        }

        public ICommand ExecuteCommand => new RelayCommand(execute);

        private Process process;
        private string fxPath = "Shader.fx";
        private string psPath = "Shader.ps";

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
