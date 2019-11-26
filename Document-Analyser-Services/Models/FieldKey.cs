using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class FieldKey
    {
        public List<dynamic> Content { get; set; }
        public Block Block { get; set; }
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }

        public FieldKey(Block block, List<string> children, List<Block> blocks)
        {
            Block = block;
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            Text = string.Empty;
            Content = new List<dynamic>();

            var words = new List<string>();

            if (children != null && children.Count > 0)
            {
                foreach (var child in children)
                {
                    var wordBlock = blocks.Find(b => b.Id == child);

                    if (wordBlock?.BlockType == BlockType.WORD)
                    {
                        var word = new Word(wordBlock);
                        Content.Add(word);
                        words.Add(word.Text);
                    }
                }
            }

            if (words.Count > 0)
            {
                Text = string.Join(" ", words);
            }
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
