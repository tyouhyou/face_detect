using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace zb.FaceDetect
{
    class CamCap : IDisposable
    {
        public delegate void CaptureFrameReceived(Bitmap image);

        private Tuple<int, VideoCaptureDevice>CurrentCaptureDevice { set; get; }

        private FilterInfoCollection CaptureDevices { set; get; }
        public List<string> CapDevices
        {
            get
            {
                var devices = new List<string>();

                CaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (0 < CaptureDevices.Count)
                {
                    foreach (FilterInfo dev in CaptureDevices)
                    {
                        devices.Add(dev.Name);
                    }
                }

                return devices;
            }
        }

        public void StartCapture(int devIdx, CaptureFrameReceived handler)
        {
            if (null == CaptureDevices || 0 >= CaptureDevices.Count || null == handler) return;

            StopCapture();

            var dev = new VideoCaptureDevice(CaptureDevices[devIdx].MonikerString);
            if (dev.SnapshotCapabilities.Length <= 0) throw new InvalidOperationException("Camera caturing is not supportted.");

            dev.NewFrame += (o, e) =>
            {
                handler((Bitmap)e.Frame.Clone());
                e.Frame.Dispose();
            };

            dev.Start();

            CurrentCaptureDevice = new Tuple<int, VideoCaptureDevice>(devIdx, dev);
        }

        public void StopCapture()
        {
            if (null != CurrentCaptureDevice &&
                CurrentCaptureDevice.Item2.IsRunning)
            {
                CurrentCaptureDevice.Item2.SignalToStop();
                CurrentCaptureDevice = null;
            }
        }

        #region IDisposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO:
                }

                StopCapture();
                disposedValue = true;
            }
        }

        ~CamCap()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}