using BackendCRUD.ApiService.Services.Interfaces;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BackendCRUD.ApiService.Services.Implementations
{
    public class PdfTextExtractor : IPdfTextExtractor
    {
        public async Task<string> ExtractText(byte[] pdfData)
        {
            return await Task.Run(() =>
            {
                using var stream = new MemoryStream(pdfData);
                using var reader = new PdfReader(stream);
                var text = new StringBuilder();
                var strategy = new SimpleTextExtractionStrategy();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    
                    text.Append(iTextSharp.text.pdf.parser.PdfTextExtractor.GetTextFromPage(reader, i, strategy));
                }

                return text.ToString();
            });
        }
    }
}