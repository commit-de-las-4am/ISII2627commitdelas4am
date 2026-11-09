using AppForSEII.API.DTOs.Modelo3DDTOs;
namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompraModelo3DController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CompraModelo3DController> _logger;

    public CompraModelo3DController(ApplicationDbContext context, ILogger<CompraModelo3DController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType(typeof(CompraModelo3DDetailDTO), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult> CreateCompraModelo3D([FromBody] CompraModelo3DDTO compraParaCrear)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogError($"{DateTime.Now} Error: los datos de la compra no son válidos");
            return BadRequest("Error: los datos de la compra no son válidos.");
        }

        if (compraParaCrear.LineasCompra is null || compraParaCrear.LineasCompra.Count == 0)
        {
            _logger.LogError($"{DateTime.Now} Error: la compra no tiene modelos");
            return BadRequest("Error: debes comprar al menos un modelo 3D.");
        }

        if (compraParaCrear.MetodoPago != MetodoPago.TarjetaCredito && compraParaCrear.MetodoPago != MetodoPago.PayPal)
        {
            _logger.LogError($"{DateTime.Now} Error: método de pago no válido");
            return BadRequest("Error: el método de pago debe ser tarjeta de crédito o PayPal.");
        }

        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Email == compraParaCrear.CorreoElectronico);
        if (cliente == null)
        {
            _logger.LogError($"{DateTime.Now} Error: no existe un cliente con el correo {compraParaCrear.CorreoElectronico}");
            return BadRequest("Error: el cliente no está registrado.");
        }

        if (compraParaCrear.LineasCompra.Any(l => l.CantidadLicencias < 1))
        {
            _logger.LogError($"{DateTime.Now} Error: cantidad de licencias no válida");
            return BadRequest("Error: la cantidad de licencias debe ser al menos 1.");
        }

        var repetido = compraParaCrear.LineasCompra.GroupBy(l => l.Modelo3DId).FirstOrDefault(g => g.Count() > 1);
        if (repetido != null)
        {
            _logger.LogError($"{DateTime.Now} Error: el modelo {repetido.Key} está repetido");
            return BadRequest($"Error: el modelo 3D con id {repetido.Key} aparece repetido en la compra.");
        }

        var idsModelos = compraParaCrear.LineasCompra.Select(l => l.Modelo3DId).ToList();
        var modelos = await _context.Modelos3D.Include(m => m.Licencias).Where(m => idsModelos.Contains(m.Id)).ToDictionaryAsync(m => m.Id);

        var ahora = DateTime.Now;
        var compra = new CompraModelo3D
        {
            FechaCompra = ahora,
            NombreCliente = compraParaCrear.NombreCliente,
            ApellidosCliente = compraParaCrear.ApellidosCliente,
            CorreoElectronico = compraParaCrear.CorreoElectronico,
            DireccionFacturacion = compraParaCrear.DireccionFacturacion,
            Descripcion = compraParaCrear.Descripcion,
            MetodoPago = compraParaCrear.MetodoPago,
            ClienteId = cliente.Id,
            Cliente = cliente,
            LineasCompraModelo = new List<LineaCompraModelo>()
        };

        foreach (var lineaDto in compraParaCrear.LineasCompra)
        {
            if (!modelos.TryGetValue(lineaDto.Modelo3DId, out var modelo))
            {
                _logger.LogError($"{DateTime.Now} Error: el modelo 3D {lineaDto.Modelo3DId} no existe.");
                return BadRequest("El modelo 3D seleccionado no existe.");
            }

            if (modelo.Licencias == null || !modelo.Licencias.Any(l => l.FechaExpiracion > ahora))
            {
                _logger.LogError($"{DateTime.Now} Error: el modelo 3D {modelo.Id} no tiene licencias vigentes.");
                return BadRequest($"Error: el modelo 3D '{modelo.Nombre}' no tiene licencias vigentes.");
            }

            compra.LineasCompraModelo.Add(new LineaCompraModelo
            {
                CantidadLicencias = lineaDto.CantidadLicencias,
                PrecioUnidad = modelo.Precio,
                Subtotal = lineaDto.CantidadLicencias * modelo.Precio,
                Modelo3DId = modelo.Id,
                Modelo3D = modelo,
                CompraModelo3D = compra
            });
        }

        compra.PrecioTotal = compra.LineasCompraModelo.Sum(l => l.Subtotal);

        _context.ComprasModelo3D.Add(compra);
        await _context.SaveChangesAsync();

        var compraDetailDTO = CompraModelo3DDetailDTO.FromCompra(compra);

        return CreatedAtAction("GetCompraModelo3D", "CompraModelo3DDetails", new { id = compra.id }, compraDetailDTO);
    }
}
