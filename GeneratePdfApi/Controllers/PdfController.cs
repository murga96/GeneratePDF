using GeneratePdfApi.Dtos;
using GeneratePdfApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeneratePdfApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PdfController : ControllerBase
    {
        private readonly IGeneratePdfService _generatePdfService;

        public PdfController(IGeneratePdfService generatePdfService)
        {
            _generatePdfService = generatePdfService;
        }

        [HttpPost("generate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GeneratePdf([FromBody] GeneratePdfDto dto)
        {
            try
            {
                byte[] xml = Convert.FromBase64String(dto.Xml);
                byte[] xslt = Convert.FromBase64String(dto.Xslt);
                byte[] generatedPdf = _generatePdfService.FormatXmlToPDF(xml, xslt);
                return Ok(new
                {
                    pdf = Convert.ToBase64String(generatedPdf),
                });
            }
            catch (FormatException ex)
            {

                return BadRequest("Los datos de entrada no tienen un formato válido en base64");
            }
            catch (Exception ex)
            {

                return Problem("Ocurrió un error al intentar generar el pdf");
            }

        }
    }
}
