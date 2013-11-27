using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RouteRecognition.Data
{
    public class FeatureRegion : Vision.Features.Detector.Features,IRegion
    {
        private Point _x;

        public Point X
        {
            get { return _x; }
            set { _x = value; }
        }

        private Point _y;

        public Point Y
        {
            get { return _x; }
            set { _y = value; }
        }


    }
}
