using Microsoft.AspNetCore.Http;

namespace Core.Shared {
    public class FileSignature {
        private static readonly Dictionary<List<string>, List<byte[]>> _fileSignatures
            = new Dictionary<List<string>, List<byte[]>>{
                {
                    new List<string>{ ".docx", ".zip", ".pptx" }, new List<byte[]>{
                        new byte[] { 0x50, 0x4B, 0x03, 0x04 },
                        new byte[] { 0x50, 0x4B, 0x4C, 0x49, 0x54, 0x45 },
                        new byte[] { 0x50, 0x4B, 0x53, 0x70, 0x58 },
                        new byte[] { 0x50, 0x4B, 0x05, 0x06 },
                        new byte[] { 0x50, 0x4B, 0x07, 0x08 },
                        new byte[] { 0x57, 0x69, 0x6E, 0x5A, 0x69, 0x70 },
                    }
                },
                {
                    new List<string>{ ".pdf" }, new List<byte[]>{ new byte[] { 0x25, 0x50, 0x44, 0x46 } }
                },
                {
                    new List<string>{ ".jpeg" }, new List<byte[]> {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE2 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE3 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
                    }
                },
                {
                    new List<string>{ ".jpg" }, new List<byte[]> {
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE1 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xE8 },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xEE },
                        new byte[] { 0xFF, 0xD8, 0xFF, 0xDB }
                    }
                },
                {
                    new List<string>{".txt"}, new List<byte[]> {
                        new byte[]{ 0xEF, 0xBB, 0xBF },
                        new byte[]{ 0xFF, 0xFE },
                        new byte[]{ 0xFE, 0xFF },
                        new byte[]{ 0XFF, 0xFE, 0x00, 0x00 },
                        new byte[]{ 0x00, 0x00, 0xFE, 0xFF },
                        new byte[]{ 0x0E, 0xFE, 0xFF }
                    }
                },
                {
                    new List<string>{ ".png" }, new List<byte[]> {
                        new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }
                    }
                },
                {
                    new List<string>{ ".dcm" }, new List<byte[]> {
                        new byte[]{0x44, 0x49, 0x43, 0x4D }
                    }
                }
            };

        public static bool IsFileValid(IFormFile file, string expectedFormat) {
            List<byte[]> signature;
            var k = _fileSignatures.Keys.ToList<List<string>>().Find(M=>M.Contains(expectedFormat));
            if(k == null) {
                throw new FormatException($"Invalid Format Provider for file {file.FileName}");
            }
            signature = _fileSignatures[k];
            using (var reader = new BinaryReader(file.OpenReadStream())) {
                var headerBytes = reader.ReadBytes(_fileSignatures.Max(m => m.Value.Max(n => n.Length)));
                bool result = signature.Any(signature => headerBytes.Take(signature.Length).SequenceEqual(signature));
                return result;
            }
        }

    }
}
