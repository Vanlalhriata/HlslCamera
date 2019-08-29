using AForge.Video.DirectShow;
using System;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace HlslCamera
{
    public class CameraManager
    {
        public event EventHandler OnNewFrame;

        public Bitmap CurrentBitmap { get; private set; }

        private VideoCaptureDevice videoSource;

        public CameraManager()
        {
            Application.Current.Exit += application_Exit;
        }

        public void InitializeVideo()
        {
            var videoDevicesList = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (videoDevicesList.Count < 1)
            {
                ErrorManager.NotifyError("No video capture devices found.");
            }
            else
            {
                videoSource = new VideoCaptureDevice(videoDevicesList[videoDevicesList.Count - 1].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
        }

        private void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            CurrentBitmap = eventArgs.Frame;
            OnNewFrame?.Invoke(this, null);
        }

        private void application_Exit(object sender, ExitEventArgs e)
        {
            videoSource?.Stop();

            if (null != OnNewFrame)
            {
                var handlers = OnNewFrame.GetInvocationList();
                foreach (EventHandler handler in handlers)
                {
                    OnNewFrame -= handler;
                }
            }
        }
    }
}
