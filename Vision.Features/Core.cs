using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vision.Features.Detector;
using Vision.Features.Model;
using Vision.Features.Matching;

namespace Vision.Features
{
    public class Core
    {
        private static ModelHandler _modelHandler;
        private static IFeaturesExtractor _featuresExtrator;
        private static BasicMatcher _matcher;
        private static DrawMatches _drawer;
        private static FeatureEnum _typeFeature;

        public enum FeatureEnum
        {
            Fast,
            Freak,
            SURF,
            BRISK
        }

        public static FeatureEnum FeatureType
        {
            get { return _typeFeature; }
            set { _typeFeature = value; }
        }

        public static void Refresh()
        {
            if( _modelHandler != null )
                _modelHandler.Clear();

            _modelHandler = null;

            _featuresExtrator = null;
            CreateFeatureExtractor();
        }

        private static void CreateFeatureExtractor()
        {
            if (_featuresExtrator == null)
            {
                switch( _typeFeature )
                {
                    case FeatureEnum.Fast:
                        _featuresExtrator = (IFeaturesExtractor)(new FastFeatureExtractor());
                        break;
                    case FeatureEnum.Freak:
                        _featuresExtrator = (IFeaturesExtractor)(new FreakFeatureExtractor());
                        break;
                    case FeatureEnum.SURF:
                        _featuresExtrator = (IFeaturesExtractor)(new SURFFeatureExtractor());
                        break;
                    case FeatureEnum.BRISK:
                        _featuresExtrator = (IFeaturesExtractor)(new BRISKFeatureExtrator());
                        break;
                
                }
            }
        }

        public static IFeaturesExtractor FeaturesExtactor()
        {
            CreateFeatureExtractor();

            return _featuresExtrator;
        }
              

        public static ModelHandler ModelHandler()
        {
            CreateFeatureExtractor();

            if( _modelHandler == null )
                _modelHandler = new ModelHandler(_featuresExtrator);

            return _modelHandler;

        }

        public static BasicMatcher Matcher()
        {
            if (_matcher == null)
                _matcher = new BasicMatcher();

            return _matcher;
        }

        public static DrawMatches DrawMatches()
        {
            if (_drawer == null)
                _drawer = new DrawMatches();

            return _drawer;
        }

    }
}
