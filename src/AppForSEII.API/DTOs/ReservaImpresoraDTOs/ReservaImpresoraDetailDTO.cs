using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs;

public class ReservaImpresoraDetailDTO
{
    public ReservaImpresoraDetailDTO(int id, DateTime fechaReserva, string nombreCliente, string apellidosCliente, string direccionFacturacion, string numeroTelefono, MetodoPago metodoPago, decimal precioTotal, IList<LineaReservaItemDTO> lineasReserva)
    {
        Id = id;
        FechaReserva = fechaReserva;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        NumeroTelefono = numeroTelefono;
        MetodoPago = metodoPago;
        PrecioTotal = precioTotal;
        LineasReserva = lineasReserva;
    }

    public int Id { get; set; }
    public DateTime FechaReserva { get; set; }
    public string NombreCliente { get; set; }
    public string ApellidosCliente { get; set; }
    public string DireccionFacturacion { get; set; }
    public string NumeroTelefono { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public decimal PrecioTotal { get; set; }
    public IList<LineaReservaItemDTO> LineasReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ReservaImpresoraDetailDTO otro &&
               Id == otro.Id &&
               NombreCliente == otro.NombreCliente &&
               ApellidosCliente == otro.ApellidosCliente &&
               DireccionFacturacion == otro.DireccionFacturacion &&
               NumeroTelefono == otro.NumeroTelefono &&
               MetodoPago == otro.MetodoPago &&
               PrecioTotal == otro.PrecioTotal &&
               LineasReserva.SequenceEqual(otro.LineasReserva);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, NombreCliente, ApellidosCliente, DireccionFacturacion, PrecioTotal);
    }
}
