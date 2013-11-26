using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;

namespace Vision.Features.Detector
{
    public interface IFeaturesMatcher
    {
        bool Match(Features observedFeatures, Features modelFeatures, out long matchTime, out HomographyMatrix homography, out int notZeroCount);
    }
}
