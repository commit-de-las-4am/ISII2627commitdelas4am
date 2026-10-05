using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Tiempo de reserva")]
    public TiempoReserva TiempoReserva { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Precio subtotal")]
    [Precision(10, 2)]
    public decimal PrecioSubtotal { get; set; }

    public int ImpresoraId { get; set; }

    [Required]
    [ForeignKey(nameof(ImpresoraId))]
    public Impresora3D Impresora { get; set; } = null!;

    // Clave foránea hacia ReservaImpresora (la relación se define desde ReservaImpresora)
    public int ReservaImpresoraId { get; set; }

    // 1. Constructor vacío (Obligatorio para Entity Framework)
    public LineaReserva()
    {
    }

    // 2. Constructor con parámetros (Excluye la propiedad de navegación 'Impresora')
    public LineaReserva(int id, TiempoReserva tiempoReserva, decimal precioSubtotal, int impresoraId, int reservaImpresoraId)
    {
        Id = id;
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        ImpresoraId = impresoraId;
        ReservaImpresoraId = reservaImpresoraId;
    }
}