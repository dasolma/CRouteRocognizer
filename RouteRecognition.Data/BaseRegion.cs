using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RouteRecognition.Data
{
    public class BaseRegion : IRegion
    {
        private Point _x;
        private Point _y;
        private int _width;
        private int _height;

        public System.Drawing.Point X
        {
            get { return _x; }
            set { _x = value; }
        }

        public System.Drawing.Point Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
    }
}
