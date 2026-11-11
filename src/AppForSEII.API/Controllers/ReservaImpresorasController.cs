using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservaImpresorasController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ReservaImpresorasController> _logger;

    public ReservaImpresorasController(ApplicationDbContext context, ILogger<ReservaImpresorasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    [Route("GetReservaImpresora/{id}")]
    [ProducesResponseType(typeof(ReservaImpresoraDetailDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetReservaImpresora(int id)
    {
        var reserva = await _context.ReservasImpresoras
            .Include(r => r.Cliente)
            .Include(r => r.LineasReserva)
                .ThenInclude(l => l.Impresora)
            .Where(r => r.Id == id)
            .Select(r => new ReservaImpresoraDetailDTO(
                r.Id,
                r.FechaReserva,
                r.Cliente.Name ?? string.Empty,
                r.Cliente.Surname ?? string.Empty,
                r.Cliente.DireccionFacturacion,
                r.Cliente.PhoneNumber ?? string.Empty,
                r.MetodoPago,
                r.PrecioTotal,
                r.LineasReserva.Select(l => new LineaReservaItemDTO(
                    l.Impresora.Nombre,
                    l.Impresora.Modelo,
                    l.Impresora.Tipo,
                    l.Impresora.Descripcion,
                    l.TiempoReserva,
                    l.Impresora.PrecioKilovatioHora,
                    l.Impresora.PrecioReserva,
                    l.PrecioSubtotal
                )).ToList()
            ))
            .FirstOrDefaultAsync();

        if (reserva == null)
        {
            _logger.LogInformation($"{DateTime.Now} Reserva con ID {id} no encontrada");
            return NotFound($"La reserva con ID {id} no existe.");
        }

        return Ok(reserva);
    }

    [HttpPost]
    [Route("PostReservaImpresora")]
    [ProducesResponseType(typeof(ReservaImpresoraDetailDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
    public async Task<ActionResult<ReservaImpresoraDetailDTO>> PostReservaImpresora(ReservaImpresoraCreateDTO reservaDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"{DateTime.Now} Error: Datos de la reserva inválidos");
            return BadRequest("Los datos de la reserva no son válidos.");
        }

        if (reservaDTO.LineasReserva == null || reservaDTO.LineasReserva.Count == 0)
        {
            _logger.LogError($"{DateTime.Now} Error: Lista de líneas de reserva vacía");
            return BadRequest("Debe incluir al menos una impresora para ser reservada.");
        }

        // Buscar cliente existente o instanciar uno nuevo
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Name == reservaDTO.NombreCliente && c.Surname == reservaDTO.ApellidosCliente);
        if (cliente == null)
        {
            cliente = new Cliente(
                Guid.NewGuid().ToString(),
                reservaDTO.NombreCliente,
                reservaDTO.ApellidosCliente,
                $"{reservaDTO.NombreCliente.ToLower()}.{reservaDTO.ApellidosCliente.ToLower()}@uclm.es",
                reservaDTO.DireccionFacturacion
            )
            {
                PhoneNumber = reservaDTO.NumeroTelefono
            };
            _context.Clientes.Add(cliente);
        }

        var nuevaReserva = new ReservaImpresora(
            0,
            DateTime.Now,
            0m,
            reservaDTO.MetodoPago,
            cliente.Id
        )
        {
            Cliente = cliente
        };

        decimal precioTotal = 0;
        var lineasDetalleDTO = new List<LineaReservaItemDTO>();

        foreach (var lineaDTO in reservaDTO.LineasReserva)
        {
            var impresora = await _context.Impresoras3D.FindAsync(lineaDTO.ImpresoraId);
            if (impresora == null)
            {
                _logger.LogError($"{DateTime.Now} Impresora con ID {lineaDTO.ImpresoraId} no encontrada");
                return NotFound($"La impresora con ID {lineaDTO.ImpresoraId} no existe.");
            }

            if (lineaDTO.TiempoReserva == TiempoReserva.NoDefinido)
            {
                _logger.LogError($"{DateTime.Now} Tiempo de reserva no válido para la impresora {impresora.Nombre}");
                return BadRequest($"El tiempo de reserva especificado no es válido para la impresora {impresora.Nombre}.");
            }

            int horas = lineaDTO.TiempoReserva switch
            {
                TiempoReserva.UnaHora => 1,
                TiempoReserva.DosHoras => 2,
                TiempoReserva.TresHoras => 3,
                TiempoReserva.CuatroHoras => 4,
                TiempoReserva.CincoHoras => 5,
                TiempoReserva.UnDia => 24,
                TiempoReserva.DosDias => 48,
                _ => 1
            };

            decimal subtotal = impresora.PrecioReserva + (horas * impresora.PrecioKilovatioHora);

            var linea = new LineaReserva(
                0,
                lineaDTO.TiempoReserva,
                subtotal,
                impresora.Id,
                0
            )
            {
                Impresora = impresora
            };

            precioTotal += subtotal;
            nuevaReserva.LineasReserva.Add(linea);

            lineasDetalleDTO.Add(new LineaReservaItemDTO(
                impresora.Nombre,
                impresora.Modelo,
                impresora.Tipo,
                impresora.Descripcion,
                lineaDTO.TiempoReserva,
                impresora.PrecioKilovatioHora,
                impresora.PrecioReserva,
                subtotal
            ));
        }

        nuevaReserva.PrecioTotal = precioTotal;
        _context.ReservasImpresoras.Add(nuevaReserva);
        await _context.SaveChangesAsync();

        var reservaRetornoDTO = new ReservaImpresoraDetailDTO(
            nuevaReserva.Id,
            nuevaReserva.FechaReserva,
            cliente.Name ?? string.Empty,
            cliente.Surname ?? string.Empty,
            cliente.DireccionFacturacion,
            cliente.PhoneNumber ?? string.Empty,
            nuevaReserva.MetodoPago,
            nuevaReserva.PrecioTotal,
            lineasDetalleDTO
        );

        return CreatedAtAction(nameof(GetReservaImpresora), new { id = nuevaReserva.Id }, reservaRetornoDTO);
    }
}
