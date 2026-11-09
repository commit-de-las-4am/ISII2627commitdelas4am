namespace AppForSEII.API.DTOs.Modelo3DDTOs;
public class CompraModelo3DDetailDTO
{
    public CompraModelo3DDetailDTO(int id, DateTime fechaCompra, string nombreCliente, string apellidosCliente, string correoElectronico, string direccionFacturacion, MetodoPago metodoPago, string? descripcion, decimal precioTotal, List<LineaCompraModelo3DDetailDTO> lineasCompra)
    {
        Id = id;
        FechaCompra = fechaCompra;
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        CorreoElectronico = correoElectronico;
        DireccionFacturacion = direccionFacturacion;
        MetodoPago = metodoPago;
        Descripcion = descripcion;
        PrecioTotal = precioTotal;
        LineasCompra = lineasCompra;
    }

    public int Id { get; set; }
    public DateTime FechaCompra { get; set; }
    public string NombreCliente { get; set; }
    public string ApellidosCliente { get; set; }
    public string CorreoElectronico { get; set; }
    public string DireccionFacturacion { get; set; }
    public MetodoPago MetodoPago { get; set; }
    public string? Descripcion { get; set; }
    public decimal PrecioTotal { get; set; }
    public List<LineaCompraModelo3DDetailDTO> LineasCompra { get; set; }

    public static CompraModelo3DDetailDTO FromCompra(CompraModelo3D compra)
    {
        return new CompraModelo3DDetailDTO(
            compra.id,
            compra.FechaCompra,
            compra.NombreCliente,
            compra.ApellidosCliente,
            compra.CorreoElectronico,
            compra.DireccionFacturacion,
            compra.MetodoPago,
            compra.Descripcion,
            compra.PrecioTotal,
            compra.LineasCompraModelo.Select(l => new LineaCompraModelo3DDetailDTO(
                l.Modelo3D.Nombre,
                l.Modelo3D.Categoria,
                l.Modelo3D.Formato,
                l.PrecioUnidad,
                l.CantidadLicencias,
                l.Subtotal,
                LicenciaModelo3DDTO.VigentesDe(l.Modelo3D)
            )).ToList()
        );
    }

    public override bool Equals(object? obj)
    {
        return obj is CompraModelo3DDetailDTO otro &&
               Id == otro.Id &&
               FechaCompra.Date == otro.FechaCompra.Date &&
               NombreCliente == otro.NombreCliente &&
               ApellidosCliente == otro.ApellidosCliente &&
               CorreoElectronico == otro.CorreoElectronico &&
               DireccionFacturacion == otro.DireccionFacturacion &&
               MetodoPago == otro.MetodoPago &&
               Descripcion == otro.Descripcion &&
               PrecioTotal == otro.PrecioTotal &&
               LineasCompra.SequenceEqual(otro.LineasCompra);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, NombreCliente, ApellidosCliente, CorreoElectronico, PrecioTotal);
    }
}
