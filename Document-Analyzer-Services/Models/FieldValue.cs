using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyzer_Services.Models
{
    public class FieldValue
    {
        public List<dynamic> Content { get; set; }
        public Block Block { get; set; }
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }

        public FieldValue(Block block, List<string> ids, List<Block> blocks)
        {
            Block = block;
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            Text = string.Empty;
            Content = new List<dynamic>();

            var words = new List<string>();
            if (ids != null && ids.Count > 0)
            {
                foreach (var id in ids)
                {
                    var wordBlock = blocks.Find(b => b.Id == id);
                    if (wordBlock?.BlockType == BlockType.WORD)
                    {
                        var word = new Word(wordBlock);
                        Content.Add(word);
                        words.Add(word.Text);
                    }
                    else if (wordBlock?.BlockType == BlockType.SELECTION_ELEMENT)
                    {
                        var selection = new SelectionElement(wordBlock);
                        Content.Add(selection);
                        words.Add(selection.SelectionStatus);
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
