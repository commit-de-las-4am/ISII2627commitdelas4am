using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppForSEII.API.Models
{
    public class LineaCompraAccesorio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad mínima es 1")]
        public int Cantidad { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Microsoft.EntityFrameworkCore.Precision(8, 2)]
        public decimal PrecioUnitario { get; set; }

        // Relación con CompraAccesorios
        [Required]
        public int CompraAccesoriosId { get; set; }
        
        [ForeignKey("CompraAccesoriosId")]
        public CompraAccesorios Compra { get; set; }

        // Relación con Accesorio
        [Required]
        public int AccesorioId { get; set; }
        
        [ForeignKey("AccesorioId")]
        public Accesorio Accesorio { get; set; }

        // Constructor vacío 
        public LineaCompraAccesorio()
        {
        }

        // Constructor con parámetros
        public LineaCompraAccesorio(int id, int cantidad, decimal precioUnitario, CompraAccesorios compra, Accesorio accesorio)
        {
            Id = id;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Compra = compra;
            Accesorio = accesorio;
        }
    }
}