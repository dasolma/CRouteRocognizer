using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;

namespace Vision.Features.Detector
{
    public class BRISKFeatureExtrator : IFeaturesExtractor
    {
        private static Brisk fastCPU = new Brisk(30, 3, 1);
        
        private static BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

        public Vision.Features.Detector.Features GetFeatures(Emgu.CV.Image<Gray, byte> image)
        {
            return GetFeatures(image, null);
        }

        public Vision.Features.Detector.Features GetFeatures(Emgu.CV.Image<Gray, byte> image, Emgu.CV.Image<Gray, byte> mask)
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
