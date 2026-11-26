using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AccesorioDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoriosController : ControllerBase
    {
        private protected readonly ApplicationDbContext _context;
        private readonly ILogger<AccesoriosController> _logger;

        public AccesoriosController(ApplicationDbContext context, ILogger<AccesoriosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetAccesoriosParaComprar")]
        [ProducesResponseType(typeof(IList<AccesorioParaComprarDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAccesoriosParaComprar()
        {
            var accesorios = await _context.Accesorios
                .Where(a => a.CantidadDisponible > 0)
                .Select(a => new AccesorioParaComprarDTO(
                    a.Id,
                    a.Nombre,
                    a.Compatibilidad,
                    a.Precio,
                    a.CantidadDisponible
                ))
                .ToListAsync();

            _logger.LogInformation("Consulta de accesorios recibida");
            return Ok(accesorios);
        }
    }
}