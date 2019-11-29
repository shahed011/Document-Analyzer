using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyzer_Services.Models
{
    public class Cell
    {
        public int RowIndex { get; set; }
        public int RowSpan { get; set; }
        public int ColumnIndex { get; set; }
        public int ColumnSpan { get; set; }
        public List<dynamic> Content { get; set; }
        public Block Block { get; set; }
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }

        public Cell(Block block, List<Block> blocks)
        {
            Block = block;
            ColumnIndex = block.ColumnIndex;
            ColumnSpan = block.ColumnSpan;
            Confidence = block.Confidence;
            Content = new List<dynamic>();
            Geometry = block.Geometry;
            Id = block.Id;
            RowIndex = block.RowIndex;
            RowSpan = block.RowSpan;
            Text = string.Empty;

            var relationships = block.Relationships;
            if (relationships != null && relationships.Count > 0)
            {
                foreach (var relationship in relationships)
                {
                    if (relationship.Type == RelationshipType.CHILD)
                    {
                        foreach (var id in relationship.Ids)
                        {
                            var matchedBlock = blocks.Find(b => b.Id == id);

                            if (matchedBlock?.BlockType == BlockType.WORD)
                            {
                                var word = new Word(matchedBlock);
                                Content.Add(word);
                                Text = Text + word.Text + " ";
                            }
                            else if (matchedBlock?.BlockType == BlockType.SELECTION_ELEMENT)
                            {
                                var selectionElement = new SelectionElement(matchedBlock);
                                Content.Add(selectionElement);
                                Text = Text + selectionElement.SelectionStatus + ", ";
                            }
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            return Text;
        }

    }
}
