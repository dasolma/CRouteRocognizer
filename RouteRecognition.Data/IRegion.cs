using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RouteRecognition.Data
{
    public interface IRegion
    {
        Point X { get; }
        Point Y { get; }
        int Width { get; }
        int Height { get; }
    }
}
