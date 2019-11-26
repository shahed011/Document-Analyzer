using Amazon.Textract.Model;
using System;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class AnalyserGeometry : Geometry
    {
        public AnalyserGeometry(Geometry geometry) : base()
        {
            BoundingBox = geometry.BoundingBox;
            Polygon = geometry.Polygon;

            var bb = new AnalyserBoundingBox(BoundingBox.Width, BoundingBox.Height, BoundingBox.Left, BoundingBox.Top);
            var polygons = new List<Point>();

            foreach (var singlePoligon in Polygon)
            {
                polygons.Add(new Point
                {
                    X = singlePoligon.X,
                    Y = singlePoligon.Y
                });
            }

            BoundingBox = bb;
            Polygon = polygons;
        }

        public override string ToString()
        {
            return string.Format("BoundingBox: {0}{1}", BoundingBox, Environment.NewLine);
        }
    }
}
