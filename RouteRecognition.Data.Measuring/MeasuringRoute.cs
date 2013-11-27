using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RouteRecognition.Data.Measuring
{
    public class MeasuringRoute : RouteRecognition.Data.Route
    {
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }
    }
}
