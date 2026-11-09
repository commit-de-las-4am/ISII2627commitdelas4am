namespace AppForSEII.API.DTOs.Modelo3DDTOs;
public class CompraModelo3DCreateDTO
{
    public CompraModelo3DCreateDTO()
    {
        NombreCliente = string.Empty;
        ApellidosCliente = string.Empty;
        CorreoElectronico = string.Empty;
        DireccionFacturacion = string.Empty;
        LineasCompra = new List<LineaCompraModelo3DCreateDTO>();
    }

    public CompraModelo3DCreateDTO(string nombreCliente, string apellidosCliente, string correoElectronico, string direccionFacturacion, string? descripcion, MetodoPago metodoPago, List<LineaCompraModelo3DCreateDTO> lineasCompra)
    {
        NombreCliente = nombreCliente;
        ApellidosCliente = apellidosCliente;
        CorreoElectronico = correoElectronico;
        DireccionFacturacion = direccionFacturacion;
        Descripcion = descripcion;
        MetodoPago = metodoPago;
        LineasCompra = lineasCompra;
    }

    [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
    [StringLength(100)]
    public string NombreCliente { get; set; }

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(100)]
    public string ApellidosCliente { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
    [StringLength(150)]
    public string CorreoElectronico { get; set; }

    [Required(ErrorMessage = "La dirección de facturación es obligatoria")]
    [StringLength(200)]
    public string DireccionFacturacion { get; set; }

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres.")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El método de pago es obligatorio")]
    public MetodoPago MetodoPago { get; set; }

    [Required(ErrorMessage = "Debe haber al menos un modelo 3D")]
    public List<LineaCompraModelo3DCreateDTO> LineasCompra { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is CompraModelo3DCreateDTO otro &&
               NombreCliente == otro.NombreCliente &&
               ApellidosCliente == otro.ApellidosCliente &&
               CorreoElectronico == otro.CorreoElectronico &&
               DireccionFacturacion == otro.DireccionFacturacion &&
               Descripcion == otro.Descripcion &&
               MetodoPago == otro.MetodoPago &&
               LineasCompra.SequenceEqual(otro.LineasCompra);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreCliente, ApellidosCliente, CorreoElectronico, DireccionFacturacion, MetodoPago);
    }
}
