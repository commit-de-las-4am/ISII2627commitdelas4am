using AppForSEII.API.Data;
using AppForSEII.API.DTOs.AccesorioDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompraAccesoriosController : ControllerBase
    {
        private protected readonly ApplicationDbContext _context;

        public CompraAccesoriosController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        [Route("GetCompraAccesorio/{id}")]
        [ProducesResponseType(typeof(CompraAccesorioDetailDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetCompraAccesorio(int id)
        {
            var compra = await _context.ComprasAccesorios
                .Include(c => c.Lineas)
                    .ThenInclude(l => l.Accesorio) 
                .Where(c => c.Id == id)
                .Select(c => new CompraAccesorioDetailDTO(
                    c.Id,
                    c.FechaCompra,
                    c.PrecioTotal,
                    c.DireccionEnvio,
                    c.Lineas.Select(l => new CompraAccesorioItemDTO(
                        l.Accesorio.Nombre,
                        l.Cantidad,
                        l.PrecioUnitario
                    )).ToList()
                ))
                .FirstOrDefaultAsync();

            if (compra == null)
            {
                return NotFound();
            }

            return Ok(compra);
        }
    }
}