using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RouteRecognition.Data.Measuring
{
    public class MeasuringRoute : RouteRecognition.Data.Route
    {
        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { _imagePath = value; }
        }

          /// <summary>
        /// Constructor de la clase Route
        /// </summary>
        /// <param name="id">Identificador de la ruta</param>
        /// <param name="points">Lista de puntos que componen la ruta</param>
        /// <param name="regions">Lista de regiones que contiene la información para el reconocimiento</param>
        public MeasuringRoute(int id, int width, int height, List<Point> points, List<IRegion> regions) : 
            base(id, width, height, points, regions)
        {
        }
    }
}
