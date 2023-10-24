using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace WebFormsAPIWebbtest
{
    public partial class _Default : Page
    {
        private static readonly HttpClient client = new HttpClient();

        protected async void btnConvertUrl_Click(object sender, EventArgs e)
        {
            await ConvertUrlToPdf(txtUrl.Text.Trim());
        }

        protected async void btnConvertHtml_Click(object sender, EventArgs e)
        {
            await ConvertHtmlToPdf(htmlFileUpload.FileName.Trim());
        }

        private async Task ConvertUrlToPdf(string url)
        {
            try
            {
                var response = await client.PostAsJsonAsync("https://localhost:7099/api/pdf/convertUrl", new { Url = url });

                if (response.IsSuccessStatusCode)
                {
                    byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                    SaveAndDownloadPdf(pdfBytes);
                }
                else
                {
                    litMessage.Text = "Error converting URL to PDF.";
                }
            }
            catch (HttpRequestException ex)
            {
                litMessage.Text = $"Error: {ex.Message} {(ex.InnerException != null ? "- " + ex.InnerException.Message : string.Empty)}";

            }
        }

        private async Task ConvertHtmlToPdf(string htmlFileName)
        {
            if (htmlFileUpload.HasFile)
                try
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        var htmlContentStream = htmlFileUpload.FileContent;

                        content.Add(new StreamContent(htmlContentStream), "htmlFile", htmlFileName);

                        var response = await client.PostAsync("https://localhost:7099/api/pdf/convertHtmlFile", content);

                        if (response.IsSuccessStatusCode)
                        {
                            byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                            SaveAndDownloadPdf(pdfBytes);
                        }
                        else
                        {
                            litMessage.Text = "Error converting HTML to PDF.";
                        }
                        if (File.Exists(htmlFileName))
                            File.Delete(htmlFileName);
                    }
                }
                catch (HttpRequestException ex)
                {
                    litMessage.Text = $"Error: {ex.Message} {(ex.InnerException != null ? "- " + ex.InnerException.Message : string.Empty)}";

                }
            else
            {
                litMessage.Text = "Please upload an HTML file.";
            }
        }


        private void SaveAndDownloadPdf(byte[] pdfBytes)
        {
            if (pdfBytes == null || pdfBytes.Length == 0)
            {
                litMessage.Text = "Error: Received empty content";
                return;
            }

            // Set the headers for PDF download directly to the client's browser
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=converted.pdf");
            Response.BinaryWrite(pdfBytes);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}