namespace Core.Application.Interfaces.Infrastructure.File {
    public interface IQRGenerator {
        byte[] QRImageAsPNGBytes(string str);
        string QRImageAsSVG(string str);
    }
}
