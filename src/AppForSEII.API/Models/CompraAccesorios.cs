using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class CompraAccesorios
    {
        public CompraAccesorios()
        {
            NombreCliente = string.Empty;
            ApellidosCliente = string.Empty;
            DireccionEnvio = string.Empty;
            NumeroTelefono = string.Empty;
            Cliente = null!;
        }

        public CompraAccesorios(int id, DateTime fechaCompra, string nombreCliente, string apellidosCliente, string direccionEnvio, string numeroTelefono, decimal precioTotal, MetodoPago metodoPago)
        {
            Id = id;
            FechaCompra = fechaCompra;
            NombreCliente = nombreCliente;
            ApellidosCliente = apellidosCliente;
            DireccionEnvio = direccionEnvio;
            NumeroTelefono = numeroTelefono;
            PrecioTotal = precioTotal;
            MetodoPago = metodoPago;
            Cliente = null!;
        }
        
        [Key]
        public int Id { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(50)]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(100)]
        public string ApellidosCliente { get; set; }

        [Required(ErrorMessage = "La dirección de envío es obligatoria")]
        [StringLength(200)]
        public string DireccionEnvio { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio")]
        [Phone]
        public string NumeroTelefono { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Microsoft.EntityFrameworkCore.Precision(8, 2)]
        public decimal PrecioTotal { get; set; }

        [Required]
        public MetodoPago MetodoPago { get; set; }

        public IList<LineaCompraAccesorio> Lineas { get; set; } = new List<LineaCompraAccesorio>();

        public string? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

    }
}