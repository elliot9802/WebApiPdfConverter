using System;
using System.Net.Http;
using System.IO;
using System.Web.UI;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Web;

namespace WebFormsAPIWebbtest
{
    public partial class _Default : Page
    {
        protected async void btnConvertUrl_Click(object sender, EventArgs e)
        {
            await ConvertUrlToPdf(txtUrl.Text.Trim());
        }

        private async Task ConvertUrlToPdf(string url)
        {
            using (HttpClient client = new HttpClient())
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
                    var errorMessage = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage += " - " + ex.InnerException.Message;
                    }
                    litMessage.Text = errorMessage;
                }
            }
        }

        private void SaveAndDownloadPdf(byte[] pdfBytes)
        {
            // Save the file to the server
            string relativePath = $"~/{Guid.NewGuid()}.pdf";
            string pdfPath = Server.MapPath(relativePath);
            File.WriteAllBytes(pdfPath, pdfBytes);

            // Prompt user to download the file
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=converted.pdf");
            Response.TransmitFile(pdfPath);

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}