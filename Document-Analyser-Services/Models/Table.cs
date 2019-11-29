using Amazon.Textract.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Document_Analyzer_Services.Models
{
    public class Table
    {
        public List<Row> Rows { get; set; }
        public Block Block { get; set; }
        public float Confidence { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }

        public Table(Block block, List<Block> blocks)
        {
            Block = block;
            Confidence = block.Confidence;
            Geometry = block.Geometry;
            Id = block.Id;
            Rows = new List<Row>();

            var rowIndex = 1;
            var row = new Row();

            var relationships = block.Relationships;
            if (relationships != null && relationships.Count > 0)
            {
                relationships.ForEach(r => {
                    if (r.Type == "CHILD")
                    {
                        r.Ids.ForEach(id => {
                            var cell = new Cell(blocks.Find(b => b.Id == id) ?? new Block(), blocks);

                            if (cell.RowIndex > rowIndex)
                            {
                                Rows.Add(row);
                                row = new Row();
                                rowIndex = cell.RowIndex;
                            }

                            row.Cells.Add(cell);
                        });
                        if (row != null && row.Cells.Count > 0)
                            Rows.Add(row);
                    }
                });
            }
        }
        public override string ToString()
        {
            var result = new List<string>();
            result.Add(string.Format("Table{0}===={0}", Environment.NewLine));

            foreach (var row in Rows)
            {
                result.Add(string.Format("Row{0}===={0}{1}{0}", Environment.NewLine, row));
            }

            return string.Join("", result);
        }

    }
}
