using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;

namespace QRCodeHttpEcho.Controllers
{
    [ApiController]
    [Route("/")]
    public class RootEcho : ControllerBase
    {
        protected static string HtmlColorToHex(string inputColor, string fallback)
        {
            var c = System.Drawing.Color.FromName(inputColor);
            if (c.IsKnownColor)
            {
                return $"#{c.R:X2}{c.G:X2}{c.B:X2}";
            }

            if (Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                return inputColor;

            return fallback;
        }

        public IActionResult Get(string text = "", string color = "black", string backgroundColor = "white", int blockSize = 10, string ECC = "M")
        {
            QRCodeGenerator.ECCLevel ecc = QRCodeGenerator.ECCLevel.M;
            if (!string.IsNullOrWhiteSpace(ECC))
            {
                //allow for ECC to be queried as L, l, Lowest, low...
                var c = ECC.ToUpper()[0];
                switch (c)
                {
                    case 'L':
                        ecc = QRCodeGenerator.ECCLevel.L;
                        break;
                    case 'M':
                        ecc = QRCodeGenerator.ECCLevel.M;
                        break;
                    case 'Q':
                        ecc = QRCodeGenerator.ECCLevel.Q;
                        break;
                    case 'H':
                        ecc = QRCodeGenerator.ECCLevel.H;
                        break;
                }
            }
            var data = BitmapByteQRCodeHelper.GetQRCode(text,  blockSize, HtmlColorToHex(color, "#000000"), HtmlColorToHex(backgroundColor, "#FFFFFF"), ecc);

            return File(data, "image/bmp");
        }
    }
}
