using Core.Application.Interfaces.Infrastructure.File;
using QRCoder;
using System.Drawing;

namespace Infrastructure.Integration.QR {
    public class QRLib : IQRGenerator {
        public byte[] QRImageAsPNGBytes(string str) {
            var imgType = Base64QRCode.ImageType.Png;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20, Color.Black, Color.White, true, imgType);
            return Convert.FromBase64String(qrCodeImageAsBase64);
        }

        public string QRImageAsSVG(string str) {
            var imgType = Base64QRCode.ImageType.Png;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(str, QRCodeGenerator.ECCLevel.Q);
            SvgQRCode qrCode = new SvgQRCode(qrCodeData);
            string qrCodeImageAsSVG = qrCode.GetGraphic(20);
            return qrCodeImageAsSVG;
        }
    }
}
