namespace GeneratePdfApi.Services
{
    public interface IGeneratePdfService
    {
        byte[] FormatXmlToPDF(byte[] xmlPath, byte[] xsltFo, string outputPath);
        byte[] GenerateXSLFO(byte[] xml, byte[] xsltFo);
        byte[] GeneratePDF(byte[] inputFo, string fopConfigFilePath);
    }
}
