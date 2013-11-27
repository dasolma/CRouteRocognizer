//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.GPU;

namespace Vision.Features.Matching
{
   public class DrawMatches 
   {
       public bool FindFast(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime, out  VectorOfKeyPoint observedKeyPoints, out VectorOfKeyPoint modelKeyPoints, out HomographyMatrix homography)
       {
           Stopwatch watch;
           homography = null;

           FastDetector fastCPU = new FastDetector(10, true);
           //VectorOfKeyPoint modelKeyPoints;
           //VectorOfKeyPoint observedKeyPoints;
           Matrix<int> indices;

           BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

           Matrix<byte> mask;

           watch = Stopwatch.StartNew();

           int k = 2;
           double uniquenessThreshold = 0.8;

           //extract features from the object image
           modelKeyPoints = fastCPU.DetectKeyPointsRaw(modelImage, null);
           if (modelKeyPoints.Size == 0) {
               matchTime = 0;
               observedKeyPoints = null;
               return false;
           }

           Matrix<Byte> modelDescriptors = descriptor.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);
           if (modelKeyPoints.Size == 0)
           {
               matchTime = 0;
               observedKeyPoints = null;
               return false;
           }

           // extract features from the observed image
           observedKeyPoints = fastCPU.DetectKeyPointsRaw(observedImage, null);
           Matrix<Byte> observedDescriptors = descriptor.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
           
           BruteForceMatcher<Byte> matcher = new BruteForceMatcher<Byte>(DistanceType.L2);
           matcher.Add(modelDescriptors);
           

           indices = new Matrix<int>(observedDescriptors.Rows, k);
           using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
           {
               matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
               mask = new Matrix<byte>(dist.Rows, 1);
               mask.SetValue(255);
               Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
           }

           int nonZeroCount = CvInvoke.cvCountNonZero(mask);
           if (nonZeroCount >= 4)
           {
               nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
               if (nonZeroCount >= 4)
                   homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(
                   modelKeyPoints, observedKeyPoints, indices, mask, 2);
                
           }

         //  //Draw the matched keypoints
         //  Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
         //     indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

         //  #region draw the projected region on the image
         //  if (homography != null)
         //  {  //draw a rectangle along the projected model
         //      Rectangle rect = modelImage.ROI;
         //      PointF[] pts = new PointF[] { 
         //new PointF(rect.Left, rect.Bottom),
         //new PointF(rect.Right, rect.Bottom),
         //new PointF(rect.Right, rect.Top),
         //new PointF(rect.Left, rect.Top)};
         //      homography.ProjectPoints(pts);

         //      result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
         //  }
         //  #endregion

           //return result;
           
           watch.Stop();
           matchTime = watch.ElapsedMilliseconds;

           return true;
       }

      public  bool FindMatch(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, out Matrix<int> indices, out Matrix<byte> mask, out HomographyMatrix homography)
      {
         int k = 2;
         double uniquenessThreshold = 0.8;
         SURFDetector surfCPU = new SURFDetector(500, false);
         
         Stopwatch watch;
         homography = null;

         if (GpuInvoke.HasCuda)
         {
            GpuSURFDetector surfGPU = new GpuSURFDetector(surfCPU.SURFParams, 0.01f);
            using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(modelImage))
            //extract features from the object image
            using (GpuMat<float> gpuModelKeyPoints = surfGPU.DetectKeyPointsRaw(gpuModelImage, null))
            using (GpuMat<float> gpuModelDescriptors = surfGPU.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
            using (GpuBruteForceMatcher<float> matcher = new GpuBruteForceMatcher<float>(DistanceType.L2))
            {
               modelKeyPoints = new VectorOfKeyPoint();
               surfGPU.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
               watch = Stopwatch.StartNew();

               // extract features from the observed image
               using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray, byte>(observedImage))
               using (GpuMat<float> gpuObservedKeyPoints = surfGPU.DetectKeyPointsRaw(gpuObservedImage, null))
               using (GpuMat<float> gpuObservedDescriptors = surfGPU.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
               using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
               using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
               using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
               using (Stream stream = new Stream())
               {
                  matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                  indices = new Matrix<int>(gpuMatchIndices.Size);
                  mask = new Matrix<byte>(gpuMask.Size);

                  //gpu implementation of voteForUniquess
                  using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                  using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                  {
                     GpuInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                     GpuInvoke.Compare(col0, col1, gpuMask, CMP_TYPE.CV_CMP_LE, stream);
                  }

                  observedKeyPoints = new VectorOfKeyPoint();
                  surfGPU.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                  //wait for the stream to complete its tasks
                  //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                  stream.WaitForCompletion();

                  gpuMask.Download(mask);
                  gpuMatchIndices.Download(indices);

                  if (GpuInvoke.CountNonZero(gpuMask) >= 4)
                  {
                     int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                     if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                  }

                  watch.Stop();
               }
            }
         }
         else
         {
            //extract features from the object image
            modelKeyPoints = new VectorOfKeyPoint();
            
            Matrix<float> modelDescriptors = surfCPU.DetectAndCompute(modelImage, null, modelKeyPoints);
            if (modelKeyPoints.Size == 0)
            {
                matchTime = 0;
                observedKeyPoints = null;
                indices = null;
                mask = null;
                return false;
            }

            watch = Stopwatch.StartNew();

            // extract features from the observed image
            observedKeyPoints = new VectorOfKeyPoint();
            Matrix<float> observedDescriptors = surfCPU.DetectAndCompute(observedImage, null, observedKeyPoints);
            if (observedDescriptors == null) observedDescriptors = new Matrix<float>(10,10);
           
            BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
            matcher.Add(modelDescriptors);

            indices = new Matrix<int>(observedDescriptors.Rows, k);
            using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
            {
               matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
               mask = new Matrix<byte>(dist.Rows, 1);
               mask.SetValue(255);
               Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
                
            }

            int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= 4)
            {
               nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
               if (nonZeroCount >= 4)
                  homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
            }

            watch.Stop();
         }
         matchTime = watch.ElapsedMilliseconds;

         return true;
      }

      /// <summary>
      /// Draw the model image and observed image, the matched features and homography projection.
      /// </summary>
      /// <param name="modelImage">The model image</param>
      /// <param name="observedImage">The observed image</param>
      /// <param name="matchTime">The output total time for computing the homography matrix.</param>
      /// <returns>The model image and observed image, the matched features and homography projection.</returns>
      public  Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
      {
         HomographyMatrix homography;
         VectorOfKeyPoint modelKeyPoints;
         VectorOfKeyPoint observedKeyPoints;
         matchTime = 0;
         

         int w = modelImage.ROI.Width;
         int h = modelImage.ROI.Height;
         Image<Bgr, Byte> result = null;
         int offset = 2;
       
         
         {
             for (int j = 0; j + h / offset <= h; j += h / offset)
             {
                 for (int i = 0; i + w / offset <= w; i += w / offset)
                 {
                     Image<Gray, Byte> crop = new Image<Gray, byte>(w / offset, h / offset);
                     modelImage.ROI = new Rectangle(i, j, w / offset, h / offset);
                     //CvInvoke.cvCopy(modelImage, crop, IntPtr.Zero);
                     
                     //if (FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography))
                     if (FindFast(modelImage, observedImage, out matchTime, out observedKeyPoints, out modelKeyPoints, out homography))
                     {

                         //Draw the matched keypoints
                         //Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
                         //   indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);
                         Image<Bgr, Byte> aux = Features2DToolbox.DrawKeypoints(observedImage, observedKeyPoints, new Bgr(255, 255, 255), Features2DToolbox.KeypointDrawType.DEFAULT);
                         if (result == null) result = aux;

                         #region draw the projected region on the image
                         if (homography != null)
                         {  //draw a rectangle along the projected model
                             Rectangle rect = modelImage.ROI;
                             PointF[] pts = new PointF[] { 
                       new PointF(rect.Left - i, rect.Bottom - j),
                       new PointF(rect.Right -i , rect.Bottom- j),
                       new PointF(rect.Right - i, rect.Top- j),
                       new PointF(rect.Left - i, rect.Top- j)};
                             homography.ProjectPoints(pts);

                             result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);
                         }
                         #endregion

                     }

                     
                 }

                 
             }
             //FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography);


         }
        
         modelImage.ROI = Rectangle.Empty;
         return result;
      }

      /// <summary>
      /// Draw the model image and observed image, the matched features and homography projection.
      /// </summary>
      /// <param name="modelImage">The model image</param>
      /// <param name="observedImage">The observed image</param>
      /// <param name="matchTime">The output total time for computing the homography matrix.</param>
      /// <returns>The model image and observed image, the matched features and homography projection.</returns>
      public  Image<Bgr, Byte> Draw(Image<Bgr, byte> observedImage, bool drawfeatures, out long matchTime)
      {
          HomographyMatrix homography;
          HomographyMatrix homographyCand;
          int notZeroCountCand = 0;
          Vision.Features.Detector.Features modelCand = null;
          matchTime = 0;
          int notZeroCount;
          Image<Bgr, Byte> result = null;


          Image<Gray, Byte> gray = observedImage.Convert<Gray, Byte>();
          Vision.Features.Detector.Features observedFeatures = Core.FeaturesExtactor().GetFeatures(gray);

          float percent = 0;
          foreach (List<Vision.Features.Detector.Features> list in Core.ModelHandler().Values)
          {
              homographyCand = null;
              notZeroCountCand = 0;
              foreach (Vision.Features.Detector.Features modelFeatures in list)
              {

                  if (Core.Matcher().Match(observedFeatures, modelFeatures, out matchTime, out homography, out notZeroCount))
                  {
                      //Draw the matched keypoints
                      Image<Bgr, Byte> aux = null;
                      if (drawfeatures)
                          aux = Features2DToolbox.DrawKeypoints(observedImage, observedFeatures.keyPoints, new Bgr(255, 255, 255), Features2DToolbox.KeypointDrawType.DEFAULT);
                      else
                          aux = observedImage.Clone();

                      if (result == null) result = aux;

                      #region draw the projected region on the image
                      if (homography != null &&  notZeroCount > notZeroCountCand)
                      {
                          Rectangle rect = new Rectangle(0, 0, modelFeatures.Width, modelFeatures.Height);
                          PointF[] pts = new PointF[] { 
                            new PointF(rect.Left, rect.Bottom),
                            new PointF(rect.Right , rect.Bottom),
                            new PointF(rect.Right, rect.Top),
                            new PointF(rect.Left, rect.Top)};
                          homography.ProjectPoints(pts);

                          double el1 = (double)modelFeatures.Width / modelFeatures.Height;
                          double el = Elongation(pts[0], pts[1], pts[2], pts[3]);
                          double f = el1 / el;
                          double m = MinAngle(pts[0], pts[1], pts[2], pts[3]);
                          percent = (100f / modelFeatures.keyPoints.Size) * notZeroCount;

                          //if ( m > 1.4d && m < 1.6d && f > 0.8 && f < 1.2d)
                          if( percent > 2 )
                          {
                              homographyCand = homography.Clone();
                              modelCand = modelFeatures;
                              notZeroCountCand = notZeroCount;
                          }
                      }

                      //if (homography != null)
                      //{  //draw a rectangle along the projected model
                      //    Rectangle rect = new Rectangle(0, 0, modelFeatures.With, modelFeatures.Heigth);
                      //    PointF[] pts = new PointF[] { 
                      //              new PointF(rect.Left, rect.Bottom),
                      //              new PointF(rect.Right , rect.Bottom),
                      //              new PointF(rect.Right, rect.Top),
                      //              new PointF(rect.Left, rect.Top)};
                      //    homography.ProjectPoints(pts);

                      //    double l1 = Math.Sqrt(Math.Pow(pts[1].X - pts[0].X, 2) + Math.Pow(pts[1].Y - pts[0].Y, 2));
                      //    double l2 = Math.Sqrt(Math.Pow(pts[3].X - pts[2].X, 2) + Math.Pow(pts[3].Y - pts[2].Y, 2));
                      //    double l3 = Math.Sqrt(Math.Pow(pts[2].X - pts[1].X, 2) + Math.Pow(pts[2].Y - pts[1].Y, 2));
                      //    double l4 = Math.Sqrt(Math.Pow(pts[0].X - pts[3].X, 3) + Math.Pow(pts[0].Y - pts[3].Y, 3));
                      //    double f1 = l1 / l2;
                      //    double f2 = l3 / l4;
                      //    if (f1 > 0.7d && f1 < 1.3d /* && f2 > 0.7d && f2 < 1.3d*/ )
                      //    result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);

                      //}
                      #endregion

                  }
              }

              if (homographyCand != null)
              {  //draw a rectangle along the projected model
                  Rectangle rect = new Rectangle(0, 0, modelCand.Width, modelCand.Height);
                  PointF[] pts = new PointF[] { 
                            new PointF(rect.Left, rect.Bottom),
                            new PointF(rect.Right , rect.Bottom),
                            new PointF(rect.Right, rect.Top),
                            new PointF(rect.Left, rect.Top)};
                  homographyCand.ProjectPoints(pts);

                  

                  //double l1 = Math.Sqrt(Math.Pow(pts[1].X - pts[0].X, 2) + Math.Pow(pts[1].Y - pts[0].Y, 2));
                  //double l2 = Math.Sqrt(Math.Pow(pts[3].X - pts[2].X, 2) + Math.Pow(pts[3].Y - pts[2].Y, 2));
                  //double l3 = Math.Sqrt(Math.Pow(pts[2].X - pts[1].X, 2) + Math.Pow(pts[2].Y - pts[1].Y, 2));
                  //double l4 = Math.Sqrt(Math.Pow(pts[0].X - pts[3].X, 3) + Math.Pow(pts[0].Y - pts[3].Y, 3));
                  //double f1 = l1 / l2;
                  //double f2 = l3 / l4;
                  //if (f1 > 0.7d && f1 < 1.3d /* && f2 > 0.7d && f2 < 1.3d*/ )
                 
                  result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 2);
                  float x = (pts[0].X + (pts[1].X - pts[0].X) / 2 + pts[3].X + (pts[2].X - pts[3].X) / 2) / 2;
                  float y = (pts[1].Y + (pts[2].Y - pts[1].Y) / 2 + pts[0].Y + (pts[3].Y - pts[0].Y) / 2) / 2;
                  PointF center = new PointF(x, y);
                  result.Draw( new CircleF(center, 5), new Bgr(Color.Red), 5);
                  MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_PLAIN, 2, 2);
                  Point p = new Point((int)x, (int)y);
                  result.Draw( modelCand.Name, ref font, p, new Bgr(Color.Red));

                  
                  p = new Point(10, 20);
                  font = new MCvFont(FONT.CV_FONT_HERSHEY_PLAIN, 1, 1 );
                  result.Draw("Matching percent: " + percent.ToString() + "%", ref font, p, new Bgr(Color.Red));


                  double m = MinAngle(pts[0], pts[1], pts[2], pts[3]) * 180 / Math.PI;
                  p = new Point(10, 40);
                  result.Draw("Min angle: " + m.ToString(), ref font, p, new Bgr(Color.Red));

                  double el1 = (double)modelCand.Width / modelCand.Height;
                  double el = Elongation(pts[0], pts[1], pts[2], pts[3]);
                   p = new Point(10, 60);
                  result.Draw("Elongation factor: " + el1 / el , ref font, p, new Bgr(Color.Red));

              }

          }

          
          return result;
      }

      private double Elongation(PointF p1, PointF p2, PointF p3, PointF p4)
      {
          double w = Math.Sqrt(Math.Pow(p1.X - p2.X,2) + Math.Pow(p1.Y - p2.Y,2));
          double h = Math.Sqrt(Math.Pow(p2.X - p3.X, 2) + Math.Pow(p2.Y - p3.Y, 2));

          return w / h;

      }

      private double Angle(PointF p1l1, PointF p2l1, PointF p1l2, PointF p2l2)
      {
          double m1 = (p2l1.Y - p1l1.Y) / (p2l1.X - p1l1.X);
          double m2 = (p2l2.Y - p1l2.Y) / (p2l2.X - p1l2.X);

          return Math.Atan(Math.Abs(m2 - m1 / (1 + m2 * m1)));
      }

      private double MinAngle(PointF p1, PointF p2, PointF p3, PointF p4)
      {
          double a1 = Angle(p1, p2, p2, p3);
          double a2 = Angle(p2, p3, p3, p4);
          double a3 = Angle(p3, p4, p4, p1);
          double a4 = Angle(p4, p1, p1, p2);

          double min = Math.Min(a1, a2);
          min = Math.Min(min, a3);
          min = Math.Min(min, a4);

          return min;
      }

   }
}
