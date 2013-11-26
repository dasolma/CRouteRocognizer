//----------------------------------------------------------------------------
//  Copyright (C) 2004-2012 by EMGU. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.GPU;


namespace FeatureDetectorTest
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);

         //using (Image<Gray, Byte> modelImage = new Image<Gray, byte>("box.png"))
         //using (Image<Gray, Byte> observedImage = new Image<Gray, byte>("box_in_scene.png"))
         //{
         //    Image<Bgr, byte> result = DrawMatches.Draw(modelImage, observedImage, out matchTime);
         //    ImageViewer.Show(result, String.Format("Matched using {0} in {1} milliseconds", GpuInvoke.HasCuda ? "GPU" : "CPU", matchTime));
         //}
         //SURFDetector surfCPU = new SURFDetector(500, false);

         //VideoSurveilance v = new VideoSurveilance();
         //v.ShowDialog();

         FileImageTest ft = new FileImageTest();
         ft.ShowDialog();
      }
   }
}
