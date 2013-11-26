﻿using System;
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
        public int With;
        public int Heigth;
        public string name;

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
