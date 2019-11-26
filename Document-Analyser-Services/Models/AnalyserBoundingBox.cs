using Amazon.Textract.Model;

namespace Document_Analyser_Services.Models
{
    public class AnalyserBoundingBox : BoundingBox
    {
        public AnalyserBoundingBox(float width, float height, float left, float top) : base()
        {
            Width = width;
            Height = height;
            Left = left;
            Top = top;
        }

        public override string ToString()
        {
            return string.Format("width: {0}, height: {1}, left: {2}, top: {3}", Width, Height, Left, Top);
        }
    }
}
