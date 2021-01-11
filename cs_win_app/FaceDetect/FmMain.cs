using System;
using System.Drawing;
using System.Windows.Forms;
using OpenCvSharp;

namespace zb.FaceDetect
{
    public partial class FmMain : Form
    {
        private readonly string[] ClassName = new string[]
        {
            "face",
            "face_mask"
        };

        private delegate void InvokeSetImage(Image img);
        private InvokeSetImage invoker;

        private DarkPredictor predictor { set; get; }

        private CamCap capture { set; get; }

        private string BrowseFile()
        {
            var ret = string.Empty;
            using(var dia = new OpenFileDialog())
            {
                dia.Filter = "(*.jpg;*.png;*.gif)|*.jpg;*.png;*.gif";
                dia.Multiselect = false;
                dia.RestoreDirectory = true;
                if (DialogResult.OK == dia.ShowDialog(this))
                {
                    ret = dia.FileName;
                }
            }
            return ret;
        }

        private void SetImage(Image img)
        {
            if (null != pbMain.Image)
            {
                pbMain.Image.Dispose();
            }
            pbMain.Image = img;
        }

        private void DrawPredictResult(Image img, DarkPredictor.predict_result[] results)
        {
            if (null == results) return;

            var scale = (float)img.Width / pbMain.Width;

            using (var g = Graphics.FromImage(img))
            {
                var penFace = new Pen(Color.Red, 2.0f);
                var bruLabelBack = Brushes.LightGray;
                var bruLabelStr = Brushes.Black;
                for (int i = 0; i < results.Length; i++)
                {
                    var x = (int)(results[i].x * img.Width);
                    var y = (int)(results[i].y * img.Height);
                    var w = (int)(results[i].w * img.Width);
                    var h = (int)(results[i].h * img.Height);
                    var rectFace = new Rectangle
                    {
                        X = x,
                        Y = y,
                        Width = w,
                        Height = h
                    };
                    var rectLabel = new Rectangle
                    {
                        X = x,
                        Y = y - 2 - (int)(Font.Height * scale),
                        Width = w,
                        Height = (int)(Font.Height * scale)
                    };
                    g.FillRectangle(bruLabelBack, rectLabel);
                    g.DrawString($"{ClassName[results[i].class_id]} : {results[i].probability * 100:N1}%",
                        new Font(this.Font.Name, Font.Size * scale), 
                        bruLabelStr, 
                        new PointF(x, y - Font.Height * scale));
                    g.DrawRectangle(penFace, rectFace);
                }
            }
        }

        public FmMain()
        {
            InitializeComponent();
            invoker = new InvokeSetImage(SetImage);
        }

        private void FmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            capture.Dispose();
            predictor.Dispose();
            Close();
        }

        private void FmMain_Load(object sender, EventArgs e)
        {
            predictor = new DarkPredictor()
                .SetLog("log.txt")
                .Load(@"data\facex.cfg", @"data\facex.weights");

            capture = new CamCap();

            var devs = capture.CapDevices;
            if (devs.Count > 0)
            {
                cmbDeviceList.Items.AddRange(devs.ToArray());
                cmbDeviceList.SelectedIndex = 0;
            }
        }

        private void btnPicture_Click(object sender, EventArgs e)
        {
            var filepath = BrowseFile();
            if (string.IsNullOrWhiteSpace(filepath)) return;

            var mat = new Mat(filepath);
            Image image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(mat);
            predictor.Predict(mat, (results) =>
            {
                DrawPredictResult(image, results);
            });

            if (null != image) pbMain.Image = image;
        }

        private readonly int fps_mod = 20;
        private int fps_revd = 0;
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (null == capture) return;

            if (null == btnCamera.Tag || !(bool)btnCamera.Tag)
            {
                capture.StartCapture(cmbDeviceList.SelectedIndex, img =>
                {
                    fps_revd++;
                    fps_revd %= fps_mod;
                    if (0 != fps_revd)
                    {
                        if (null != img) img.Dispose();
                        return;
                    }

                    try
                    {
                        var mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(img);
                        predictor.Predict(mat, (results) =>
                        {
                            DrawPredictResult(img, results);
                            results = null;
                        });
                        mat.Dispose();
                        if (InvokeRequired)
                        {
                            BeginInvoke(invoker, new object[] { img });
                        }
                        else
                        {
                            SetImage(img);
                        }
                    }
                    catch (Exception)
                    {
                        // DO THING
                    }
                });
                btnCamera.Text = "Close Camera";
                btnCamera.Tag = true;
                btnClose.Enabled = false;
            }
            else
            {
                capture.StopCapture();
                btnCamera.Text = "Open Camera";
                btnCamera.Tag = false;
                btnClose.Enabled = true;
            }
        }
    }
}
