using AppForSEII.API.DTOs.Modelo3DDTOs;
namespace AppForSEII.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompraModelo3DDetailsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CompraModelo3DDetailsController> _logger;

    public CompraModelo3DDetailsController(ApplicationDbContext context, ILogger<CompraModelo3DDetailsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CompraModelo3DDetailDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<ActionResult> GetCompraModelo3D(int id)
    {
        var compra = await _context.ComprasModelo3D
            .Include(c => c.LineasCompraModelo)
                .ThenInclude(l => l.Modelo3D)
                    .ThenInclude(m => m.Licencias)
            .Where(c => c.id == id)
            .FirstOrDefaultAsync();

        if (compra == null)
        {
            _logger.LogError($"{DateTime.Now} Error: la compra con id {id} no existe.");
            return NotFound();
        }

        return Ok(CompraModelo3DDetailDTO.FromCompra(compra));
    }
}
