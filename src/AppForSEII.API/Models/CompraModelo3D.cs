public class CompraModelo3D
{
    public CompraModelo3D()
    {
    }

    public CompraModelo3D( DateTime fechaCompra, string nombreCliente, string apellidosCliente, string correoElectronico, string direccionFacturacion, string descripcion, decimal precioTotal, MetodoPago metodoPago, string clienteId)
    {
        FechaCompra = fechaCompra;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        CorreoElectronico = correoElectronico;
        DireccionFacturacion = direccionFacturacion;
        Descripcion = descripcion;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        ClienteId = clienteId;
    }

    [Key]
    public int id { get; set; }
    
    [Required]
    public DateTime FechaCompra { get; set; }
    
    [StringLength(100)]
    public string NombreCliente { get; set; }
    
    [StringLength(100)]
    public string ApellidosCliente { get; set; }
    
    [StringLength(150)]
    public string CorreoElectronico { get; set; }
    
    [StringLength(200)]
    public string DireccionFacturacion { get; set; }
    
    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }
    
    [Required]
    public decimal PrecioTotal { get; set; }

    [Required]
    public MetodoPago MetodoPago { get; set; }

    [Required]
    public string ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente Cliente { get; set; } 

    public List<LineaCompraModelo> LineasCompraModelo { get; set; }
}