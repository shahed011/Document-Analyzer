using Amazon.Textract.Model;

namespace Document_Analyzer_Services.Models
{
    public class Word
    {
        public Block Block { get; set; }
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }

        public Word(Block block)
        {
            Block = block;
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            Text = block == null ? string.Empty : block.Text;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
