using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;
using Vision.Features;

namespace FeatureDetectorTest
{
    public partial class frmModel : Form
    {
        private Image<Bgr, Byte> _model;

        public frmModel(Image<Bgr, Byte> model)
        {
            _model = model;
            InitializeComponent();
        }

        private void frmModel_Load(object sender, EventArgs e)
        {
            model.Image = _model;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (!Core.ModelHandler().AddModel(_model.Convert<Gray, Byte>(), txtName.Text))
                MessageBox.Show("Modelo no válido");

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
