using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ImpresoraDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImpresorasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ImpresorasController> _logger;

    public ImpresorasController(ApplicationDbContext context, ILogger<ImpresorasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [Route("[action]")]
    [ProducesResponseType(typeof(IList<ImpresoraParaReservaDTO>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> GetImpresorasParaReserva(string? nombre = null, TipoImpresora? tipo = null)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"{DateTime.Now} Error: los filtros no son válidos");
            return BadRequest("Error: los filtros no son válidos.");
        }

        var query = _context.Impresoras3D.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nombre))
        {
            query = query.Where(i => i.Nombre.Contains(nombre));
        }

        if (tipo.HasValue && tipo.Value != TipoImpresora.NoDefinido)
        {
            query = query.Where(i => i.Tipo == tipo.Value);
        }

        var impresoras = await query
            .OrderBy(i => i.Nombre)
            .Select(i => new ImpresoraParaReservaDTO(
                i.Id,
                i.Nombre,
                i.Modelo,
                i.Tipo,
                i.Descripcion,
                i.PrecioKilovatioHora,
                i.PrecioReserva
            ))
            .ToListAsync();

        if (impresoras.Count == 0)
        {
            _logger.LogInformation($"{DateTime.Now} No se encontraron impresoras 3D con los filtros indicados");
            return NotFound("No se encontraron impresoras 3D disponibles con los criterios seleccionados.");
        }

        return Ok(impresoras);
    }
}
