using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RouteRecognition.Data
{
    public class Route
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _width;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        private int _height;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        private List<Point> _points;

        public List<Point> Points
        {
            get { return _points; }
            set { _points = value; }
        }
        private List<IRegion> _regions;

        public List<IRegion> Regions
        {
            get { return _regions; }
            set { _regions = value; }
        }

        /// <summary>
        /// Constructor de la clase Route
        /// </summary>
        /// <param name="id">Identificador de la ruta</param>
        /// <param name="points">Lista de puntos que componen la ruta</param>
        /// <param name="regions">Lista de regiones que contiene la información para el reconocimiento</param>
        public Route(int id, int width, int height, List<Point> points, List<IRegion> regions)
        {
            _id = id;
            _points = points;
            _regions = regions;
            _width = width;
            _height = height;
        }
    }
}
