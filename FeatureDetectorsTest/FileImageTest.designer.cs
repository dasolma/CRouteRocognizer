namespace FeatureDetectorTest
{
    partial class FileImageTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.model = new Emgu.CV.UI.ImageBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnPre = new System.Windows.Forms.Button();
            this.btnClean = new System.Windows.Forms.Button();
            this.cmbFeatureType = new System.Windows.Forms.ComboBox();
            this.chkShowFeatures = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.model)).BeginInit();
            this.SuspendLayout();
            // 
            // model
            // 
            this.model.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.model.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.model.Location = new System.Drawing.Point(0, 0);
            this.model.Name = "model";
            this.model.Size = new System.Drawing.Size(742, 444);
            this.model.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.model.TabIndex = 3;
            this.model.TabStop = false;
            this.model.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
            this.model.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            this.model.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseUp);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(693, 459);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(39, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnPre
            // 
            this.btnPre.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPre.Location = new System.Drawing.Point(647, 459);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(40, 23);
            this.btnPre.TabIndex = 5;
            this.btnPre.Text = "<";
            this.btnPre.UseVisualStyleBackColor = true;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // btnClean
            // 
            this.btnClean.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClean.Location = new System.Drawing.Point(12, 459);
            this.btnClean.Name = "btnClean";
            this.btnClean.Size = new System.Drawing.Size(49, 23);
            this.btnClean.TabIndex = 6;
            this.btnClean.Text = "Clear";
            this.btnClean.UseVisualStyleBackColor = true;
            this.btnClean.Click += new System.EventHandler(this.btnClean_Click);
            // 
            // cmbFeatureType
            // 
            this.cmbFeatureType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbFeatureType.FormattingEnabled = true;
            this.cmbFeatureType.Items.AddRange(new object[] {
            "Fast",
            "Freak",
            "SURF",
            "BRISK"});
            this.cmbFeatureType.Location = new System.Drawing.Point(106, 461);
            this.cmbFeatureType.Name = "cmbFeatureType";
            this.cmbFeatureType.Size = new System.Drawing.Size(121, 21);
            this.cmbFeatureType.TabIndex = 7;
            this.cmbFeatureType.SelectedIndexChanged += new System.EventHandler(this.cmbFeatureType_SelectedIndexChanged);
            // 
            // chkShowFeatures
            // 
            this.chkShowFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkShowFeatures.AutoSize = true;
            this.chkShowFeatures.Location = new System.Drawing.Point(281, 465);
            this.chkShowFeatures.Name = "chkShowFeatures";
            this.chkShowFeatures.Size = new System.Drawing.Size(92, 17);
            this.chkShowFeatures.TabIndex = 8;
            this.chkShowFeatures.Text = "show features";
            this.chkShowFeatures.UseVisualStyleBackColor = true;
            // 
            // FileImageTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 506);
            this.Controls.Add(this.chkShowFeatures);
            this.Controls.Add(this.cmbFeatureType);
            this.Controls.Add(this.btnClean);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.model);
            this.Name = "FileImageTest";
            this.Text = "FileImageTestcs";
            this.Load += new System.EventHandler(this.FileImageTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.model)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private Emgu.CV.UI.ImageBox model;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.Button btnClean;
        private System.Windows.Forms.ComboBox cmbFeatureType;
        private System.Windows.Forms.CheckBox chkShowFeatures;
    }
}