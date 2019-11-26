using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    public class Row
    {
        public List<Cell> Cells { get; set; }

        public Row()
        {
            Cells = new List<Cell>();
        }

        public override string ToString()
        {
            var result = new List<string>();

            foreach (var cell in Cells)
            {
                result.Add(string.Format("[{0}]", cell));
            }

            return string.Join("", result);
        }
    }
}
