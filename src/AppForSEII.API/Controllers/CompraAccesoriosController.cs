using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.DTOs.AccesorioDTOs;
using AppForSEII.API.Models;
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
        //Get
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
        //Post
        [HttpPost]
        public async Task<ActionResult<CompraAccesorios>> PostCompraAccesorio(CompraAccesorioCreateDTO compraDTO)
        {
            var nuevaCompra = new CompraAccesorios
            {
                FechaCompra = DateTime.Now,
                NombreCliente = compraDTO.NombreCliente,
                ApellidosCliente = compraDTO.ApellidosCliente,
                DireccionEnvio = compraDTO.DireccionEnvio,
                NumeroTelefono = compraDTO.NumeroTelefono,
                MetodoPago = compraDTO.MetodoPago,
                Cliente = null!,
                Lineas = new List<LineaCompraAccesorio>()
            };

            decimal precioTotal = 0;

            foreach (var lineaDTO in compraDTO.LineasCompra)
            {
                var accesorio = await _context.Accesorios.FindAsync(lineaDTO.AccesorioId);
                
                if (accesorio == null)
                {
                    return NotFound($"El accesorio con ID {lineaDTO.AccesorioId} no existe.");
                }

                if (accesorio.CantidadDisponible < lineaDTO.Cantidad)
                {
                    return BadRequest($"No hay stock suficiente para el accesorio: {accesorio.Nombre}");
                }

                var linea = new LineaCompraAccesorio
                {
                    AccesorioId = accesorio.Id,
                    Cantidad = lineaDTO.Cantidad,
                    PrecioUnitario = accesorio.Precio,
                    Accesorio = null!,
                    Compra = null!     
                };

                precioTotal += (linea.Cantidad * linea.PrecioUnitario);
                accesorio.CantidadDisponible -= linea.Cantidad;

                nuevaCompra.Lineas.Add(linea);
            }

            nuevaCompra.PrecioTotal = precioTotal;

            _context.ComprasAccesorios.Add(nuevaCompra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompraAccesorio), new { id = nuevaCompra.Id }, nuevaCompra);
        }
    }
} 