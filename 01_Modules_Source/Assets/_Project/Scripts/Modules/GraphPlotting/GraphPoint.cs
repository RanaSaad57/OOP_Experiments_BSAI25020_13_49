using System;

namespace OOPLab.Modules.GraphPlotting
{
    [Serializable]
    public class GraphPoint
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public GraphPoint(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
