using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.Models
{
    public class Accesorio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; }

        [Required]
        public CategoriaAccesorio Categoria { get; set; }

        [StringLength(200)]
        public string Compatibilidad { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa")]
        public int CantidadDisponible { get; set; }

        [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
        [Microsoft.EntityFrameworkCore.Precision(5, 2)]
        public decimal Precio { get; set; }

        // Constructor vacío (obligatorio para Entity Framework)
        public Accesorio()
        {
        }

        // Constructor con parámetros generado
        public Accesorio(int id, string nombre, CategoriaAccesorio categoria, string compatibilidad, int cantidadDisponible, decimal precio)
        {
            Id = id;
            Nombre = nombre;
            Categoria = categoria;
            Compatibilidad = compatibilidad;
            CantidadDisponible = cantidadDisponible;
            Precio = precio;
        }
    }
}