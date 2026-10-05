public class LineaCompraModelo
{
    public LineaCompraModelo()
    {
    }

    public LineaCompraModelo(int ID, int cantidadLicencias, decimal precioUnidad, decimal subtotal)
    {
        Id = ID;
        CantidadLicencias = cantidadLicencias;
        PrecioUnidad = precioUnidad;
        Subtotal = subtotal;
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