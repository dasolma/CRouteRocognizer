using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
using Vision.Features.Detector;

namespace Vision.Features.Model
{
    public class ModelHandler : Dictionary<string, List<Vision.Features.Detector.Features>>
    {
        private IFeaturesExtractor extractor;

        public ModelHandler(IFeaturesExtractor extractor)
        {
            this.extractor = extractor;
        }

        public bool AddModel(Image<Gray, Byte> image, string name)
        {
            bool ok = false;
            Image<Gray, Byte> mask = new Image<Gray, Byte>(image.Width, image.Height, new Gray(255));
            for (int a = 0; a < 360; a += 360)
            {
                Vision.Features.Detector.Features features = extractor.GetFeatures(image.Rotate(a, new Gray(255)), mask.Rotate(a, new Gray(0)));
                features.name = name;

                if (features.keyPoints != null && features.keyPoints.Size > 0)
                {
                    List<Vision.Features.Detector.Features> f = new List<Vision.Features.Detector.Features>();
                    if (this.ContainsKey(name))
                        f = this[name];
                    else
                        this.Add(name, f);

                    f.Add(features);
                    ok = true;
                }
            }

            return ok;
        }
    }
}
