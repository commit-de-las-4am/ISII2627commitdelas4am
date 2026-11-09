namespace AppForSEII.API.DTOs.AccesorioDTOs
{
    public class CompraAccesorioItemDTO
    {
        public CompraAccesorioItemDTO(string nombreAccesorio, int cantidad, decimal precioUnitario)
        {
            NombreAccesorio = nombreAccesorio;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
        }
        public string NombreAccesorio { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

    }
}