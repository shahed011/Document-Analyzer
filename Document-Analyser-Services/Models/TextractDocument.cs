using Amazon.Textract;
using Amazon.Textract.Model;
using System.Collections.Generic;

namespace Document_Analyser_Services.Models
{
    internal class TextractDocument
    {
        private List<Block> _blockMap;
        private List<List<Block>> _documentPages;

        public List<GetDocumentAnalysisResponse> ResponsePages { get; set; }
        public List<Page> Pages { get; set; }
        public List<List<Block>> PageBlocksReturn => _documentPages;

        public TextractDocument(GetDocumentAnalysisResponse response)
        {
            _blockMap = new List<Block>();
            _documentPages = new List<List<Block>>();

            Pages = new List<Page>();
            ResponsePages = new List<GetDocumentAnalysisResponse>{ response };

            ParseDocumentPagesAndBlockMap();
            Parse();
        }

        private void ParseDocumentPagesAndBlockMap()
        {
            List<Block>? documentPage = null;

            foreach (var page in ResponsePages)
            {
                foreach (var block in page.Blocks)
                {
                    _blockMap.Add(block);

                    if (block.BlockType == BlockType.PAGE)
                    {
                        if (documentPage != null)
                        {
                            _documentPages.Add(documentPage);
                        }

                        documentPage = new List<Block>{ block };
                    }
                    else
                    {
                        if (documentPage ==  null)
                        {
                            documentPage = new List<Block>();
                        }

                        documentPage.Add(block);
                    }
                }
            }

            if (documentPage != null)
            {
                _documentPages.Add(documentPage);
            }
        }

        private void Parse()
        {
            foreach (var documentPage in _documentPages)
            {
                var page = new Page(documentPage, _blockMap);
                Pages.Add(page);
            }
        }
    }
}