using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Vision.Features;

namespace FeatureDetectorTest
{
    public partial class FileImageTest : Form
    {
        private string directory;
        private string[] files;
        private int fileIndex = -1;
        private Image<Bgr, Byte> _currentImage;

        //selection vars
        private static Boolean _capture = false;
        private static Point _startPoint;
        private static Point _finishPoint;
        private static bool _drawing;
        private static Rectangle _rectModel;
        private static List<Image<Bgr, byte>> _models = new List<Image<Bgr,byte>>();


        public FileImageTest()
        {
            InitializeComponent();
        }

        private void FileImageTest_Load(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            directory = folderBrowserDialog1.SelectedPath;

            files = System.IO.Directory.GetFiles(directory);

            cmbFeatureType.SelectedIndex = 0;

            Core.FeatureType = Core.FeatureEnum.Fast;

            ShowNextImage();

           
        }

        private void ShowNextImage()
        {
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(files[++fileIndex]);
            ShowNewImage(img1);

            if (fileIndex > files.Length) fileIndex = 0;

        }

        private void ShowPreImage()
        {
            
            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(files[--fileIndex]);
            ShowNewImage(img1);
            if (fileIndex < 0) fileIndex = files.Length;

        }

        private void LoadCurrentImage()
        {

            Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(files[fileIndex]);
            ShowNewImage(img1);
        }

        private void ShowNewImage(Image<Bgr, Byte> img1)
        {
            if (_currentImage != null)
                _currentImage.Dispose();

            _currentImage = img1.Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);

            if (model.Image != null)
                model.Image.Dispose();

            model.Image = img1;

            if (Core.ModelHandler().Count > 0)
            {

                //filter out noises


                //Image<Gray, Byte> grayFrame = _currentImage.Convert<Gray, Byte>();

                ShowMatches();
            }
        }

        private void ShowMatches()
        {
            if (Core.ModelHandler().Count > 0)
            {
                Image<Bgr, byte> frame = _currentImage.Clone();
                long matchTime;
                Image<Bgr, byte> result = Core.DrawMatches().Draw(frame, this.chkShowFeatures.Checked, out matchTime);


                model.Image = result;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ShowNextImage();
        }

        private void btnPre_Click(object sender, EventArgs e)
        {
            ShowPreImage();
        }

        #region "Selection"
        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _capture = true;
            _startPoint = new Point(e.X, e.Y);

        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
            _capture = false;

            if (_startPoint != Point.Empty)
            {
                double fw = (double)_currentImage.Width / model.Width;
                double fh = (double)_currentImage.Height / model.Height;
                _rectModel = new Rectangle((int)(_startPoint.X * fw), (int)(_startPoint.Y * fh),
                                          (int)((_finishPoint.X - _startPoint.X) * fw),
                                          (int)((_finishPoint.Y - _startPoint.Y) * fh));

                //register the zone to track
                Image<Bgr, byte> m = _currentImage.Copy(_rectModel);
                
                frmModel frm = new frmModel(m);
                if( frm.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
                    _models.Add(m.Clone());

                _currentImage.ROI = Rectangle.Empty;
                return;

            }

            _startPoint = Point.Empty;
            _finishPoint = Point.Empty;

        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_drawing)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left && _startPoint != Point.Empty)
                {
                    _finishPoint = new Point(e.X, e.Y);
                }

                if (_capture)
                {
                    _drawing = true;
                    Image<Bgr, byte> aux = _currentImage.Clone();
                    DrawRectagleSelect(aux);
                    
                    model.Image = aux;
                    _drawing = false;
                }
            }
        }

        private void DrawRectagleSelect(Image<Bgr, byte> result)
        {
            if (_startPoint != Point.Empty)
            {
                double fw = (double)result.Width / model.Width;
                double fh = (double)result.Height / model.Height;
                result.Draw(new Rectangle((int)(_startPoint.X *fw), (int)(_startPoint.Y * fh),
                                          (int)((_finishPoint.X - _startPoint.X) * fw), 
                                          (int)((_finishPoint.Y - _startPoint.Y) * fh)),
                                          new Bgr(0, 0, 255), 5);
            }
        }
        #endregion

        private void btnClean_Click(object sender, EventArgs e)
        {
            Core.ModelHandler().Clear();
            _models.Clear();

            LoadCurrentImage();
        }

        private void cmbFeatureType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFeatureType.SelectedIndex)
            {
                case 0:
                    Core.FeatureType = Core.FeatureEnum.Fast;
                    break;
                case 1:
                    Core.FeatureType = Core.FeatureEnum.Freak;
                    break;
                case 2:
                    Core.FeatureType = Core.FeatureEnum.SURF;
                    break;
                case 3:
                    Core.FeatureType = Core.FeatureEnum.BRISK;
                    break;
            }

            Core.Refresh();


            int i = 0;
            foreach (Image<Bgr, byte> img in _models)
            {
                Core.ModelHandler().AddModel(img.Convert<Gray, Byte>(), "m" + i);
            }

            ShowMatches();
        }
    }
}
