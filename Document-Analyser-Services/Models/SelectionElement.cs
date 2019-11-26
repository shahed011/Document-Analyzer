using Amazon.Textract.Model;

namespace Document_Analyser_Services.Models
{
    public class SelectionElement
    {
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public string SelectionStatus { get; set; }

        public SelectionElement(Block block)
        {
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            SelectionStatus = block.SelectionStatus;
        }
    }
}
