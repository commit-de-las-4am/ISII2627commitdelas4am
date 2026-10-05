
namespace AppForSEII.API.Models;

public class ReservaImpresora
{
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Display(Name = "Fecha de reserva")]
    public DateTime FechaReserva { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Precio total")]
    [Precision(10, 2)]
    public decimal PrecioTotal { get; set; }

    [Required]
    [Display(Name = "Método de pago")]
    public MetodoPago MetodoPago { get; set; }

    // Clave foránea hacia el Cliente (Suele ser string si hereda de IdentityUser, cámbialo a int si en tu clase Cliente es int)
    public string ClienteId { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(ClienteId))]
    public Cliente Cliente { get; set; } = null!;

    // Relación de 1 a N con LineaReserva
    public IList<LineaReserva> LineasReserva { get; set; } = new List<LineaReserva>();

    // 1. Constructor vacío (Obligatorio para Entity Framework)
    public ReservaImpresora()
    {
    }

    // 2. Constructor con parámetros (Excluye colecciones y propiedades de navegación)
    public ReservaImpresora(int id, DateTime fechaReserva, decimal precioTotal, MetodoPago metodoPago, string clienteId)
    {
        Id = id;
        FechaReserva = fechaReserva;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        ClienteId = clienteId;
    }
}