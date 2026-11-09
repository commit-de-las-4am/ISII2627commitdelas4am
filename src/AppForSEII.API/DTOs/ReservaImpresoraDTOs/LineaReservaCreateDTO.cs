using AppForSEII.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs;

public class LineaReservaCreateDTO
{
    public LineaReservaCreateDTO()
    {
        ImpresoraId = 0;
        TiempoReserva = TiempoReserva.NoDefinido;
    }

    public LineaReservaCreateDTO(int impresoraId, TiempoReserva tiempoReserva)
    {
        ImpresoraId = impresoraId;
        TiempoReserva = tiempoReserva;
    }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la impresora debe ser mayor a 0.")]
    public int ImpresoraId { get; set; }

    [Required]
    public TiempoReserva TiempoReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is LineaReservaCreateDTO otra &&
               ImpresoraId == otra.ImpresoraId &&
               TiempoReserva == otra.TiempoReserva;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ImpresoraId, TiempoReserva);
    }
}
