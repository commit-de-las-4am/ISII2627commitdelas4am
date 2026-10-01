namespace AppForSEII.API.Models
{
    public class Accesorio
    {
        public int Id { get; set; }                               
        public string Nombre { get; set; }                        
        public CategoriaAccesorio Categoria { get; set; }         
        public string Compatibilidad { get; set; }                
        public int CantidadDisponible { get; set; }               
        public decimal Precio { get; set; }                       
    }
}