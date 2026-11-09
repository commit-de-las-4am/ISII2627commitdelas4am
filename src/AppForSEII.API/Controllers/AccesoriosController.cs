using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AccesorioDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccesoriosController : ControllerBase
    {
        private protected readonly ApplicationDbContext _context;

        public AccesoriosController(ApplicationDbContext context)
        {
            _context = context;
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

            return Ok(accesorios);
        }
    }
}