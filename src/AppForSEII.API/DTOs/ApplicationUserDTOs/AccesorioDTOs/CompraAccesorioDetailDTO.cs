namespace AppForSEII.API.DTOs.AccesorioDTOs
{
    public class CompraAccesorioDetailDTO
    {
        public CompraAccesorioDetailDTO(int id, DateTime fechaCompra, decimal precioTotal, string direccionEnvio, IList<CompraAccesorioItemDTO> lineasCompra)
        {
            Id = id;
            FechaCompra = fechaCompra;
            PrecioTotal = precioTotal;
            DireccionEnvio = direccionEnvio;
            LineasCompra = lineasCompra;
        }
        public int Id { get; set; }
        public DateTime FechaCompra { get; set; }
        public decimal PrecioTotal { get; set; }
        public string DireccionEnvio { get; set; }
        public IList<CompraAccesorioItemDTO> LineasCompra { get; set; }

    }
}