using Amazon.Textract;
using Amazon.Textract.Model;
using System;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class Line
    {
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }
        public List<Word> Words { get; set; }
        public string Text { get; set; }
        public Block Block { get; set; }

        public Line(Block block, List<Block> blocks)
        {
            Block = block;
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            Text = block == null ? string.Empty : block.Text;
            Words = new List<Word>();

            var relationships = block?.Relationships;
            if (relationships != null && relationships.Count > 0)
            {
                foreach (var relationship in relationships)
                {
                    if (relationship.Type == RelationshipType.CHILD)
                    {
                        foreach (var id in relationship.Ids)
                        {
                            Words.Add(new Word(blocks?.Find(b => b.BlockType == BlockType.WORD && b.Id == id) ?? new Block()));
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format(@"
                Line{0}===={0}
                {1} {0}
                Words{0}----{0}
                {2}{0}
                ----
            ", Environment.NewLine, this.Text, string.Join(", ", this.Words));
        }
    }
}
