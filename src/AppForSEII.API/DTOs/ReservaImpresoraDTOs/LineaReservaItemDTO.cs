using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs;

public class LineaReservaItemDTO
{
    public LineaReservaItemDTO(string nombreImpresora, string modelo, TipoImpresora tipo, string descripcion, TiempoReserva tiempoReserva, decimal precioKilovatioHora, decimal precioReserva, decimal precioSubtotal)
    {
        NombreImpresora = nombreImpresora;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        TiempoReserva = tiempoReserva;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
        PrecioSubtotal = precioSubtotal;
    }

    public string NombreImpresora { get; set; }
    public string Modelo { get; set; }
    public TipoImpresora Tipo { get; set; }
    public string Descripcion { get; set; }
    public TiempoReserva TiempoReserva { get; set; }
    public decimal PrecioKilovatioHora { get; set; }
    public decimal PrecioReserva { get; set; }
    public decimal PrecioSubtotal { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is LineaReservaItemDTO otra &&
               NombreImpresora == otra.NombreImpresora &&
               Modelo == otra.Modelo &&
               Tipo == otra.Tipo &&
               Descripcion == otra.Descripcion &&
               TiempoReserva == otra.TiempoReserva &&
               PrecioKilovatioHora == otra.PrecioKilovatioHora &&
               PrecioReserva == otra.PrecioReserva &&
               PrecioSubtotal == otra.PrecioSubtotal;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreImpresora, Modelo, Tipo, TiempoReserva, PrecioSubtotal);
    }
}
