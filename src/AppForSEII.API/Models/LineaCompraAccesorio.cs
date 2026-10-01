namespace AppForSEII.API.Models
{
    public class LineaCompraAccesorio
    {
        public int Id { get; set; }                               
        public int Cantidad { get; set; }                         
        public decimal PrecioUnidad { get; set; }                 
        public decimal Subtotal { get; set; }                     

        public Accesorio Accesorio { get; set; } = null!;
    }
}