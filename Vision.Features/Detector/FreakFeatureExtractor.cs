using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV;

namespace Vision.Features.Detector
{
    public class FreakFeatureExtractor : IFeaturesExtractor
    {
        private static FastDetector fastCPU = new FastDetector(30, true);
        //private static ORBDetector fastCPU = new ORBDetector(500);
        private static Freak descriptor = new Freak(true, true, 22.0f, 4);
        //private static BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

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
            features.Width = image.Width;
            features.Height = image.Height;

            return features;
        }

       
    }
}
