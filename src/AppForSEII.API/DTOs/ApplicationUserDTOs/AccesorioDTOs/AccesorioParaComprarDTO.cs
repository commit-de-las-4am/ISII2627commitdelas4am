using System.ComponentModel.DataAnnotations;

namespace AppForSEII.API.DTOs.AccesorioDTOs
{
    public class AccesorioParaComprarDTO
    {
        public AccesorioParaComprarDTO(int id, string nombre, string compatibilidad, decimal precio, int cantidadDisponible)
        {
            Id = id;
            Nombre = nombre;
            Compatibilidad = compatibilidad;
            Precio = precio;
            CantidadDisponible = cantidadDisponible;
        }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Compatibilidad { get; set; }
        public decimal Precio { get; set; }
        public int CantidadDisponible { get; set; }

    }
}