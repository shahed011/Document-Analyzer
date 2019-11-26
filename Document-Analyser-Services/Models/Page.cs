using Amazon.Textract;
using Amazon.Textract.Model;
using System;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class Page
    {
        public List<Block> Blocks { get; set; }
        public string Text { get; set; }
        public List<Line> Lines { get; set; }
        public Form Form { get; set; }
        public List<Table> Tables { get; set; }
        public List<dynamic> Content { get; set; }
        public Geometry Geometry { get; set; }
        public string Id { get; set; }

        public Page(List<Block> blocks, List<Block> blockMap)
        {
            Blocks = blocks;
            Text = string.Empty;
            Lines = new List<Line>();
            Form = new Form();
            Tables = new List<Table>();
            Content = new List<dynamic>();
            Geometry = new Geometry();
            Id = string.Empty;

            foreach (var block in blocks)
            {
                if (block.BlockType == BlockType.PAGE)
                {
                    Geometry = new AnalyserGeometry(block.Geometry);
                    Id = block.Id;
                }
                else if (block.BlockType == BlockType.LINE)
                {
                    var line = new Line(block, blockMap);
                    Lines.Add(line);
                    Content.Add(line);
                    Text = this.Text + line.Text + Environment.NewLine;
                }
                else if (block.BlockType == BlockType.TABLE)
                {
                    var table = new Table(block, blockMap);
                    Tables.Add(table);
                    Content.Add(table);
                }
                else if (block.BlockType == BlockType.KEY_VALUE_SET)
                {
                    if (block.EntityTypes.Contains("KEY"))
                    {
                        var field = new Field(block, blockMap);
                        if (field.Key != null)
                        {
                            Form.AddField(field);
                            Content.Add(field);
                        }
                    }
                }
            }

        }

        private List<IndexedText> GetLinesInReadingOrder()
        {
            var lines = new List<IndexedText>();
            var columns = new List<Column>();

            foreach (var line in Lines)
            {
                var columnFound = false;
                for (var index = 0; index < columns.Count; index++)
                {
                    var column = columns[index];
                    var bb = line.Geometry.BoundingBox;
                    var bbLeft = bb.Left;
                    var bbRight = bb.Left + bb.Width;
                    var bbCentre = bb.Left + (bb.Width / 2);
                    var columnCentre = column.Left + (column.Right / 2);

                    if ((bbCentre > column.Left && bbCentre < column.Right) || (columnCentre > bbLeft && columnCentre < bbRight))
                    {
                        lines.Add(new IndexedText { ColumnIndex = index, Text = line.Text });
                        columnFound = true;
                        break;
                    }
                }
                if (!columnFound)
                {
                    var bb = line.Geometry.BoundingBox;
                    columns.Add(new Column { Left = bb.Left, Right = bb.Left + bb.Width });
                    lines.Add(new IndexedText { ColumnIndex = columns.Count - 1, Text = line.Text });
                }
            }

            //lines.FindAll(line => line.ColumnIndex == 0).ForEach(line => Console.WriteLine(line));

            return lines;
        }

        public string GetTextInReadingOrder()
        {
            var lines = GetLinesInReadingOrder();
            var text = string.Empty;

            foreach (var line in lines)
            {
                text = text + line.Text + "\n";
            }

            return text;
        }

        public override string ToString()
        {
            var result = new List<string>
            {
                string.Format("Page{0}===={0}", Environment.NewLine)
            };

            foreach (var content in Content)
            {
                result.Add(string.Format("{1}{0}", Environment.NewLine, content));
            }

            return string.Join("", result);
        }

        public class Column
        {
            public float Left { get; set; }
            public float Right { get; set; }

            public override string ToString()
            {
                return string.Format("Left: {0}, Right :{1}", Left, Right);
            }
        }

        public class IndexedText
        {
            public int ColumnIndex { get; set; }
            public string? Text { get; set; }

            public override string ToString()
            {
                return string.Format("[{0}] {1}", ColumnIndex, Text);
            }
        }
    }
}
