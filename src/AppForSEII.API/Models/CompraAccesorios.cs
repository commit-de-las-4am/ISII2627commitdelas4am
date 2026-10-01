using System;
using System.Collections.Generic;

namespace AppForSEII.API.Models
{
    public class CompraAccesorios
    {
        public int Id { get; set; }                               
        public DateTime FechaCompra { get; set; }                 
        public string NombreCliente { get; set; }                 
        public string ApellidosCliente { get; set; }              
        public string DireccionEnvio { get; set; }               
        public string NumeroTelefono { get; set; }                
        public decimal PrecioTotal { get; set; }                  
        
        // public MetodoPago MetodoPago { get; set; }                

        public List<LineaCompraAccesorio> Lineas { get; set; } = new List<LineaCompraAccesorio>();
    }
}