using com.sun.org.apache.xalan.@internal.xsltc.compiler;
using java.io;
using System.Runtime.Intrinsics.X86;
using System.Xml;
using System.Xml.Xsl;

namespace GeneratePdfApi.Services
{
    public class GeneratePdfService : IGeneratePdfService
    {
        public byte[] FormatXmlToPDF(byte[] xml, byte[] xsltFo)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string foConfFilePath = Path.Combine(currentDirectory, "fop.xconf");

            //GENERATE FO
            byte[] xslFo = GenerateXSLFO(xml, xsltFo);

            //GENERATE PDF
            return GeneratePDF(xslFo, foConfFilePath);
        }

        private byte[] GenerateXSLFO(byte[] xml, byte[] xsltFo)
        {
            using (var msXml = new MemoryStream(xml))
            using (XmlReader xmlReader = XmlReader.Create(msXml))
            using (var msXslt = new MemoryStream(xsltFo))
            using (XmlReader xsltReader = XmlReader.Create(msXslt))
            using (var ms = new MemoryStream())
            {
                var xslt = new XslCompiledTransform(true);
                xslt.Load(xsltReader);
                xslt.Transform(xmlReader, null, ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        private byte[] GeneratePDF(byte[] inputFo, string fopConfigFilePath)
        {
            byte[] finalPDF;

            ByteArrayOutputStream os = new ByteArrayOutputStream();
            try
            {
                var fopFactory = (org.apache.fop.apps.FopFactory)org.apache.fop.apps.FopFactory.newInstance(new java.io.File(fopConfigFilePath));
                var fop = fopFactory.newFop("application/pdf", os);
                javax.xml.transform.TransformerFactory factory = javax.xml.transform.TransformerFactory.newInstance();
                javax.xml.transform.Transformer transformer = factory.newTransformer();

                InputStream input = new ByteArrayInputStream(inputFo);
                javax.xml.transform.Source src = new javax.xml.transform.stream.StreamSource(input);
                javax.xml.transform.Result res = new javax.xml.transform.sax.SAXResult(fop.getDefaultHandler());
                transformer.transform(src, res);

                finalPDF = os.toByteArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                os.close();
            }

            return finalPDF;
        }
    }
}
