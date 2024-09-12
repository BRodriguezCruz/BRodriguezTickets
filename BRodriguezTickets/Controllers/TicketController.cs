using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SLTickets.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly string _path = @"C:\Users\Alien 18\Documents\BRodriguezReasignacion\Tickets\archivos";

        public TicketController() { }

        [HttpGet("oldest")]
        public IActionResult GetFile()
        {
            try
            {
                if (!Directory.Exists(_path))
                {
                    return NotFound("No se encontraron archivos en la carpeta de origen");
                }

                var oldestFile = Directory.GetFiles(_path)
                          .Select(f => new FileInfo(f))
                          .OrderBy(fi => fi.CreationTime) //ordena por el campo fecha de creacion que proporciona la clase fileIfo
                          .FirstOrDefault();

                if (oldestFile == null)
                {
                    return NotFound("Archivos no encontrados");
                }

                string file = System.IO.File.ReadAllText(oldestFile.FullName); //fullname es la prop q tiene la ruta completa del archivo para encontrarlo y leerlo
                ML.Result result = BL.Ticket.ReadData(file, oldestFile);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
