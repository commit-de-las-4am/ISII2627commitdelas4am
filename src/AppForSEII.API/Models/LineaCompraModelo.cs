public class LineaCompraModelo
{
    public LineaCompraModelo()
    {
        CantidadLicencias = 0;
        PrecioUnidad = 0.0m;
        Subtotal = 0.0m;
        CompraModelo3D = new CompraModelo3D();
        Modelo3D = new Modelo3D();
    }

    public LineaCompraModelo(int ID, int cantidadLicencias, decimal precioUnidad, decimal subtotal)
    {
        Id = ID;
        CantidadLicencias = cantidadLicencias;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
        CompraModelo3D = new CompraModelo3D();
        Modelo3D = new Modelo3D();
    }
    
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CantidadLicencias { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public decimal PrecioUnidad { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public decimal Subtotal { get; set; }

    [Required]
    public int CompraModelo3DId { get; set; }

    [ForeignKey("CompraModelo3DId")]
    public CompraModelo3D CompraModelo3D { get; set; }

    [Required]
    public int Modelo3DId { get; set; }

    [ForeignKey("Modelo3DId")]
    public Modelo3D Modelo3D { get; set; }
}