using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyzer_Services.Models
{
    public class TextractDocument
    {
        public List<Page> Pages { get; set; }

        public TextractDocument(GetDocumentAnalysisResponse response)
        {
            Pages = new List<Page>();

            var allPageBlocks = ParseDocumentPagesAndBlockMap(response);
            Parse(allPageBlocks, response.Blocks);
        }

        private List<List<Block>> ParseDocumentPagesAndBlockMap(GetDocumentAnalysisResponse response)
        {
            var allPageBlocks = new List<List<Block>>();
            var pageBlocks = new List<Block>();

            foreach (var block in response.Blocks)
            {
                if (block.BlockType == BlockType.PAGE)
                {
                    if (pageBlocks.Count > 0)
                    {
                        allPageBlocks.Add(pageBlocks);
                    }

                    pageBlocks = new List<Block>{ block };
                }
                else
                {
                    pageBlocks.Add(block);
                }
            }

            if (pageBlocks != null)
            {
                allPageBlocks.Add(pageBlocks);
            }

            return allPageBlocks;
        }

        private void Parse(List<List<Block>> allPageBlocks, List<Block> allBlocks)
        {
            foreach (var pageBlocks in allPageBlocks)
            {
                var page = new Page(pageBlocks, allBlocks);
                Pages.Add(page);
            }
        }
    }
}