using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

public class LineaReserva
{
    [Key]
    public int Id { get; set; }

    [Required]
    [System.ComponentModel.DataAnnotations.Display(Name = "Tiempo de reserva")]
    public TiempoReserva TiempoReserva { get; set; }

    [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    [System.ComponentModel.DataAnnotations.Display(Name = "Precio subtotal")]
    [Precision(10, 2)]
    public decimal PrecioSubtotal { get; set; }

    public int ImpresoraId { get; set; }

    [Required]
    [ForeignKey(nameof(ImpresoraId))]
    public Impresora3D Impresora { get; set; } = null!;

    public int ReservaImpresoraId { get; set; }

    public LineaReserva() { }

    public LineaReserva(int id, TiempoReserva tiempoReserva, decimal precioSubtotal, int impresoraId, int reservaImpresoraId)
    {
        Id = id;
        TiempoReserva = tiempoReserva;
        PrecioSubtotal = precioSubtotal;
        ImpresoraId = impresoraId;
        ReservaImpresoraId = reservaImpresoraId;
    }
}
