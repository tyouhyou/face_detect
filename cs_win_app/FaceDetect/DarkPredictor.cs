using OpenCvSharp;
using System;
using System.Runtime.InteropServices;

namespace zb.FaceDetect
{
    class DarkPredictor : IDisposable
    {
        public delegate void PredictResultHanlder(predict_result[] results);

        private IntPtr _Predictor { set; get; }

        /// <summary>
        /// (x, y) is the top left point coordinate
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct predict_result
        {
            [MarshalAs(UnmanagedType.I4)]
            public int class_id;
            [MarshalAs(UnmanagedType.R4)]
            public float x;
            [MarshalAs(UnmanagedType.R4)]
            public float y;
            [MarshalAs(UnmanagedType.R4)]
            public float w;
            [MarshalAs(UnmanagedType.R4)]
            public float h;
            [MarshalAs(UnmanagedType.R4)]
            public float probability;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void predict_result_handler(
            [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
            predict_result[] results, 
            [MarshalAs(UnmanagedType.I4)]
            int result_num
            );

        [DllImport("DarkPredictor.dll", CallingConvention=CallingConvention.Cdecl)]
        private extern static IntPtr create_predictor();

        [DllImport("DarkPredictor.dll", CallingConvention=CallingConvention.Cdecl)]
        private extern static void set_log(IntPtr predictor, string log_file);

        [DllImport("DarkPredictor.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void load(IntPtr predictor, string config_file, string weights_file);

        [DllImport("DarkPredictor.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void destroy_predictor(IntPtr predictor);

        [DllImport("DarkPredictor.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void predict_image_file(IntPtr predictor, string image_file, predict_result_handler handler);

        [DllImport("DarkPredictor.dll", CallingConvention = CallingConvention.Cdecl)]
        private extern static void predict_image(
            IntPtr predictor, 
            byte[] image_data,
            int image_weight,
            int image_height,
            int channels,
            predict_result_handler handler);

        public DarkPredictor()
        {
            _Predictor = create_predictor();
        }

        public DarkPredictor SetLog(string logFile)
        {
            set_log(_Predictor, logFile);
            return this;
        }

        public DarkPredictor Load(string cfg, string weights)
        {
            load(_Predictor, cfg, weights);
            return this;
        }

        public void Predict(string imageFile, PredictResultHanlder handler)
        {
            predict_image_file(_Predictor, imageFile, (r, n) => handler(r));
        }

        public void Predict(Mat mat, PredictResultHanlder handler)
        {
            Mat img = new Mat();
            if (3 == mat.Channels()) Cv2.CvtColor(mat, img, ColorConversionCodes.BGR2RGB);
            var len = mat.Cols* mat.Rows* mat.Channels();
            var imgData = new byte[len];
            Marshal.Copy(mat.Data, imgData, 0, len);
            predict_image(_Predictor, imgData, mat.Cols, mat.Rows, mat.Channels(), (r, n) => handler(r));
            mat.Dispose();
        }

        #region dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO:
                }

                if (IntPtr.Zero != _Predictor)
                {
                    destroy_predictor(_Predictor);
                    _Predictor = IntPtr.Zero;
                }

                disposedValue = true;
            }
        }

        ~DarkPredictor()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    #endregion

}
