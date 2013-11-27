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

namespace RouteDesigner
{
    public partial class frmPrincipal : Form
    {
        private string directory;
        private string[] files;
        private int fileIndex = -1;
        private Image<Bgr, Byte> _currentImage;

        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void frmPrincipal_Load(object sender, EventArgs e)
        {
            this.Width = 800;
            this.Height = 600;
            //select work directory
            SelectImagesDirectory();

            //get all files names
            try
            {
                files = System.IO.Directory.GetFiles(directory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error a buscar las imagenes");
                Application.Exit();
                return;
            }

            //show
            ShowNextImage();
        }

        private void SelectImagesDirectory()
        {
            if (System.IO.Directory.Exists("../../doc/images"))
                directory = "../../doc/images";
            else
            {
                //select manually
                folderBrowserDialog1.ShowDialog();
                directory = folderBrowserDialog1.SelectedPath;
            }
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

        private void ShowNewImage(Image<Bgr, Byte> img1)
        {
            //free memory
            if (_currentImage != null)
                _currentImage.Dispose();

            //free memory
            if (this.routeDesigner.Image != null)
                routeDesigner.Image.Dispose();


            //resize the image
            _currentImage = img1.Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, true);

            //show imagen
            routeDesigner.Image = _currentImage;

        }
    }
}
