using Amazon.Textract;
using Amazon.Textract.Model;
using System;
using System.Collections.Generic;

namespace Document_Analyzer_Services.Models
{
    public class Field
    {
        public FieldKey Key { get; set; }
        public FieldValue Value { get; set; }

        public Field(Block block, List<Block> blocks)
        {
            Key = new FieldKey(new Block(), new List<string>(), new List<Block>());
            Value = new FieldValue(new Block(), new List<string>(), new List<Block>());

            var relationships = block.Relationships;
            if (relationships != null && relationships.Count > 0)
            {
                foreach (var relationship in relationships)
                {
                    if (relationship.Type == RelationshipType.CHILD)
                    {
                        Key = new FieldKey(block, relationship.Ids, blocks);
                    }
                    else if (relationship.Type == RelationshipType.VALUE)
                    {
                        foreach (var id in relationship.Ids)
                        {
                            var matchedBlock = blocks.Find(b => b.Id == id);

                            if (matchedBlock != null && matchedBlock.EntityTypes.Contains("VALUE"))
                            {
                                var blockRelationships = matchedBlock.Relationships;
                                if (blockRelationships != null && blockRelationships.Count > 0)
                                {
                                    foreach (var blockRelationship in blockRelationships)
                                    {
                                        if (blockRelationship.Type == RelationshipType.CHILD)
                                        {
                                            Value = new FieldValue(matchedBlock, blockRelationship.Ids, blocks);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public override string ToString()
        {
            var k = Key == null ? string.Empty : Key.ToString();
            var v = Value == null ? string.Empty : Value.ToString();
            return string.Format(@"
                {0}Field{0}===={0}
                Key: {1}, Value: {2}
            ", Environment.NewLine, k, v);
        }
    }
}
