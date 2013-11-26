namespace RouteDesigner
{
    partial class frmPrincipal
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.routeDesigner = new UI.Controls.RouteDesigner();
            ((System.ComponentModel.ISupportInitialize)(this.routeDesigner)).BeginInit();
            this.SuspendLayout();
            // 
            // routeDesigner
            // 
            this.routeDesigner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.routeDesigner.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.routeDesigner.Location = new System.Drawing.Point(12, 39);
            this.routeDesigner.Name = "routeDesigner";
            this.routeDesigner.Size = new System.Drawing.Size(566, 279);
            this.routeDesigner.TabIndex = 2;
            this.routeDesigner.TabStop = false;
            // 
            // frmPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 351);
            this.Controls.Add(this.routeDesigner);
            this.Name = "frmPrincipal";
            this.Text = "Route designer";
            this.Load += new System.EventHandler(this.frmPrincipal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.routeDesigner)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.RouteDesigner routeDesigner;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

