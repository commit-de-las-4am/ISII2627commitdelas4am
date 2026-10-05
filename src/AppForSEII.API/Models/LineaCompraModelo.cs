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
    

    public int Id { get; set; }
    public int CantidadLicencias { get; set; }
    public decimal PrecioUnidad { get; set; }
    public decimal Subtotal { get; set; }

    public int CompraModelo3DId { get; set; }
    public CompraModelo3D CompraModelo3D { get; set; }

    public int Modelo3DId { get; set; }
    public Modelo3D Modelo3D { get; set; }
}