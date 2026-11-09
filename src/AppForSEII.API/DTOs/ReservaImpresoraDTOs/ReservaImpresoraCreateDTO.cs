using AppForSEII.API.Models;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.ReservaImpresoraDTOs;

public class ReservaImpresoraCreateDTO
{
    public ReservaImpresoraCreateDTO()
    {
        NombreCliente = string.Empty;
        ApellidosCliente = string.Empty;
        DireccionFacturacion = string.Empty;
        NumeroTelefono = string.Empty;
        MetodoPago = MetodoPago.NoDefinido;
        LineasReserva = new List<LineaReservaCreateDTO>();
    }

    public ReservaImpresoraCreateDTO(string nombreCliente, string apellidosCliente, string direccionFacturacion, string numeroTelefono, MetodoPago metodoPago, List<LineaReservaCreateDTO> lineasReserva)
    {
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        DireccionFacturacion = direccionFacturacion;
        NumeroTelefono = numeroTelefono;
        MetodoPago = metodoPago;
        LineasReserva = lineasReserva;
    }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [StringLength(50)]
    public string NombreCliente { get; set; }

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(100)]
    public string ApellidosCliente { get; set; }

    [Required(ErrorMessage = "La dirección de facturación es obligatoria")]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; }

    [Required(ErrorMessage = "El número de teléfono es obligatorio")]
    [Phone]
    public string NumeroTelefono { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required(ErrorMessage = "Debe haber al menos una impresora a reservar")]
    public List<LineaReservaCreateDTO> LineasReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ReservaImpresoraCreateDTO otro &&
               NombreCliente == otro.NombreCliente &&
               ApellidosCliente == otro.ApellidosCliente &&
               DireccionFacturacion == otro.DireccionFacturacion &&
               NumeroTelefono == otro.NumeroTelefono &&
               MetodoPago == otro.MetodoPago &&
               LineasReserva.SequenceEqual(otro.LineasReserva);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreCliente, ApellidosCliente, DireccionFacturacion, MetodoPago);
    }
}
