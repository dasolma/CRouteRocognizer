using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV;

namespace UI.Controls
{
    public partial class RouteDesigner : ImageBox
    {
        private List<Point> _points = new List<Point>();

        public List<Point> Points
        {
            get { return new List<Point> (_points); }
        }
        private IImage _imagen;


        public RouteDesigner()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            //clear the list
            _points.Clear();
        }

        

        public IImage Image
        {
            get { return base.Image; }
            set {
                //free memory
                if (_imagen != null)
                    _imagen.Dispose();

                _imagen = (IImage)value.Clone();

                base.Image = value;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            //add the new point
            _points.Add(new Point(e.X, e.Y));

            //force paint event
            this.Refresh();

            
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            foreach (Point p in _points)
            {
                pe.Graphics.FillEllipse(Brushes.OrangeRed, p.X, p.Y, 10, 10);
                pe.Graphics.DrawEllipse(Pens.Yellow, p.X, p.Y, 10, 10);
            }

            for (int i = 1; i < _points.Count; i++)
            {
                pe.Graphics.DrawLine(new Pen(Color.OrangeRed, 4), _points[i - 1].X + 5, _points[i - 1].Y + 5,
                    _points[i].X + 5, _points[i].Y + 5);
            }

            foreach (Point p in _points)
            {
                pe.Graphics.FillEllipse(Brushes.Yellow, p.X + 3, p.Y + 3, 4, 4);
            }

        }
    }
}
