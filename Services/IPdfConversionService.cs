using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IPdfConversionService
    {

         bool IsValidUrl(string url);

        public byte[] ConvertHtmlContentToPdfBytes(string htmlContent);
        public byte[] ConvertUrlToPdfBytes(string url);
    }
}
