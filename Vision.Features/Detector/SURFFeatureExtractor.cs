using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV;

namespace Vision.Features.Detector
{
    public class SURFFeatureExtractor : IFeaturesExtractor
    {
        private static SURFDetector fastCPU = new SURFDetector(500, true);
        private static BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();
        //private static Freak descriptor = new Freak(true, true, 22.0f, 4);

        public Features GetFeatures(Emgu.CV.Image<Gray, byte> image)
        {
            return GetFeatures(image, null);
        }

        public Features GetFeatures(Emgu.CV.Image<Gray, byte> image, Emgu.CV.Image<Gray, byte> mask)
        {
            Features features = new Features();

            //extract features from the object image
            features.keyPoints = fastCPU.DetectKeyPointsRaw(image, mask);
            //features.FloatDescriptors = fastCPU.DetectAndCompute(image, null, features.keyPoints);
            features.ByteDescriptors = descriptor.ComputeDescriptorsRaw(image, null, features.keyPoints);
            features.Width = image.Width;
            features.Height = image.Height;

            return features;
        }

       
    }
}
