using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Vision.Features.Detector
{
    public interface IFeaturesExtractor
    {
        Features GetFeatures(Emgu.CV.Image<Gray, byte> image);
        Features GetFeatures(Emgu.CV.Image<Gray, byte> image, Emgu.CV.Image<Gray, byte> mask);
    }
}
