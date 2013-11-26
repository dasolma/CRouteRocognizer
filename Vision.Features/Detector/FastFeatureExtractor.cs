using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV;

namespace Vision.Features.Detector
{
    public class FastFeatureExtractor : IFeaturesExtractor
    {
        private static FastDetector fastCPU = new FastDetector(20, true);
        private static BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

        public Features GetFeatures(Emgu.CV.Image<Gray, byte> image)
        {
            return GetFeatures(image, null);
        }

        public Features GetFeatures(Emgu.CV.Image<Gray, byte> image, Emgu.CV.Image<Gray, byte> mask)
        {
            Features features = new Features();

            //extract features from the object image
            features.keyPoints = fastCPU.DetectKeyPointsRaw(image, mask);
            features.ByteDescriptors = descriptor.ComputeDescriptorsRaw(image, null, features.keyPoints);
            features.With = image.Width;
            features.Heigth = image.Height;

            return features;
        }

       
    }
}
