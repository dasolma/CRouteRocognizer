using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Util;
using Emgu.CV;

namespace Vision.Features.Detector
{
    
    public class Features
    {
        public VectorOfKeyPoint keyPoints;
        private Matrix<byte> descriptors;
        private Matrix<float> floatDescriptors;
        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Matrix<byte> ByteDescriptors
        {
            get { return (Matrix<byte>)descriptors; }
            set { descriptors = value; }
        }

        public Matrix<float> FloatDescriptors
        {
            get { return (Matrix<float>)floatDescriptors; }
            set { floatDescriptors = value; }
        }
    }
    
}
