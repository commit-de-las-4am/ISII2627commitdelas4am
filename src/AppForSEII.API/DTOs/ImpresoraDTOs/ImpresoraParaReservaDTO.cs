using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs.ImpresoraDTOs;

public class ImpresoraParaReservaDTO
{
    public ImpresoraParaReservaDTO(int id, string nombre, string modelo, TipoImpresora tipo, string descripcion, decimal precioKilovatioHora, decimal precioReserva)
    {
        Id = id;
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    }

    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Modelo { get; set; }
    public TipoImpresora Tipo { get; set; }
    public string Descripcion { get; set; }
    public decimal PrecioKilovatioHora { get; set; }
    public decimal PrecioReserva { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is ImpresoraParaReservaDTO otro &&
               Id == otro.Id &&
               Nombre == otro.Nombre &&
               Modelo == otro.Modelo &&
               Tipo == otro.Tipo &&
               Descripcion == otro.Descripcion &&
               PrecioKilovatioHora == otro.PrecioKilovatioHora &&
               PrecioReserva == otro.PrecioReserva;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Nombre, Modelo, Tipo, PrecioKilovatioHora, PrecioReserva);
    }
}
