using System.ComponentModel.DataAnnotations;
using AppForSEII.API.Models;

namespace AppForSEII.API.DTOs
{
    public class CompraAccesorioCreateDTO
    {
        [Required]
        public string NombreCliente { get; set; } = string.Empty;

        [Required]
        public string ApellidosCliente { get; set; } = string.Empty;

        [Required]
        public string DireccionEnvio { get; set; } = string.Empty;

        [Required]
        public string NumeroTelefono { get; set; } = string.Empty;

        [Required]
        public MetodoPago MetodoPago { get; set; }

        [Required]
        public List<LineaCompraAccesorioCreateDTO> LineasCompra { get; set; } = new List<LineaCompraAccesorioCreateDTO>();
    }

    public class LineaCompraAccesorioCreateDTO
    {
        [Required]
        public int AccesorioId { get; set; }

        [Required]
        public int Cantidad { get; set; }
    }
}