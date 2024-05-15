namespace GeneratePdfApi.Services
{
    public interface IGeneratePdfService
    {
        public byte[] FormatXmlToPDF(byte[] xml, byte[] xsltFo);
    }
}
