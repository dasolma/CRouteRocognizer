using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.Features2D;
using Emgu.CV;
using System.Diagnostics;
using Vision.Features.Detector;

namespace Vision.Features.Matching
{
    public class BasicMatcher : IFeaturesMatcher
    {

        public bool Match(Vision.Features.Detector.Features observedFeatures, Vision.Features.Detector.Features modelFeatures, out long matchTime, out Emgu.CV.HomographyMatrix homography, out int nonZeroCount)
        {
            Stopwatch watch;
            Matrix<byte> mask;
            int k = 2;
            double uniquenessThreshold = 0.8;
            homography = null;


            watch = Stopwatch.StartNew();

            if( observedFeatures.ByteDescriptors == null || modelFeatures.ByteDescriptors == null )
            {
                watch.Stop();
                matchTime = watch.ElapsedMilliseconds;
                nonZeroCount = 0;
                return false;
            }

             

            Matrix<int> indices;
            if( modelFeatures.FloatDescriptors != null )
                FloatMatch(observedFeatures, modelFeatures, k, uniquenessThreshold, out mask, out indices);
            else
                ByteMatch(observedFeatures, modelFeatures, k, uniquenessThreshold, out mask, out indices);

            nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= 4)
            {
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelFeatures.keyPoints, observedFeatures.keyPoints, indices, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelFeatures.keyPoints, observedFeatures.keyPoints, indices, mask, 2);
            }

            watch.Stop();
            matchTime = watch.ElapsedMilliseconds;

            return true;
        }

        private static void ByteMatch(Vision.Features.Detector.Features observedFeatures, Vision.Features.Detector.Features modelFeatures, int k, double uniquenessThreshold, out Matrix<byte> mask, out Matrix<int> indices)
        {
            BruteForceMatcher<byte> matcher = new BruteForceMatcher<byte>(DistanceType.L2);
            matcher.Add(modelFeatures.ByteDescriptors);

            indices = new Matrix<int>(observedFeatures.ByteDescriptors.Rows, k);
            using (Matrix<float> dist = new Matrix<float>(observedFeatures.ByteDescriptors.Rows, k))
            {
                matcher.KnnMatch(observedFeatures.ByteDescriptors, indices, dist, k, null);
                mask = new Matrix<byte>(dist.Rows, 1);
                mask.SetValue(255);
                Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);

            }
        }

        private static void FloatMatch(Vision.Features.Detector.Features observedFeatures, Vision.Features.Detector.Features modelFeatures, int k, double uniquenessThreshold, out Matrix<byte> mask, out Matrix<int> indices)
        {
            BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
            matcher.Add(modelFeatures.FloatDescriptors);

            indices = new Matrix<int>(observedFeatures.ByteDescriptors.Rows, k);
            using (Matrix<float> dist = new Matrix<float>(observedFeatures.ByteDescriptors.Rows, k))
            {
                matcher.KnnMatch(observedFeatures.FloatDescriptors, indices, dist, k, null);
                mask = new Matrix<byte>(dist.Rows, 1);
                mask.SetValue(255);
                Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);

            }
        }
    }
}
