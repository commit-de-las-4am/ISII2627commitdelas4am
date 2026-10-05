using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

public class Impresora3D
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "El nombre no puede superar 50 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(50, ErrorMessage = "El modelo no puede superar 50 caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    public TipoImpresora Tipo { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
    public string Descripcion { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    [Display(Name = "Precio kilovatio/hora")]
    [Precision(6, 2)]
    public decimal PrecioKilovatioHora { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Precio de reserva")]
    [Precision(8, 2)]
    public decimal PrecioReserva { get; set; }

    // Constructor vacío para Entity Framework
    public Impresora3D()
    {
    }

    // Constructor con parámetros para instanciar fácilmente
    public Impresora3D(string nombre, string modelo, TipoImpresora tipo, string descripcion, decimal precioKilovatioHora, decimal precioReserva)
    {
        Nombre = nombre;
        Modelo = modelo;
        Tipo = tipo;
        Descripcion = descripcion;
        PrecioKilovatioHora = precioKilovatioHora;
        PrecioReserva = precioReserva;
    }
}