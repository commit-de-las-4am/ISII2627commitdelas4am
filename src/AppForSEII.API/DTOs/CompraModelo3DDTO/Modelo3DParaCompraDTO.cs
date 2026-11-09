namespace AppForSEII.API.DTOs.Modelo3DDTOs;

public class Modelo3DParaCompraDTO
{
    public Modelo3DParaCompraDTO(int id, string nombre, string categoria, FormatoModelo3D formato, decimal precio, List<LicenciaModelo3DDTO> licencias)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Formato = formato;
        Precio = precio;
        Licencias = licencias;
    }

    public int Id { get; set; }
    [StringLength(200)]
    public string Nombre { get; set; }
    [StringLength(50)]
    public string Categoria { get; set; }
    public FormatoModelo3D Formato { get; set; }
    [Precision(18, 2)]
    public decimal Precio { get; set; }
    public List<LicenciaModelo3DDTO> Licencias { get; set; }
    public string? AvisoLicencia => LicenciaModelo3DDTO.GenerarAviso(Licencias);

    public override bool Equals(object? obj)
    {
        return obj is Modelo3DParaCompraDTO otro &&
               Id == otro.Id &&
               Nombre == otro.Nombre &&
               Categoria == otro.Categoria &&
               Formato == otro.Formato &&
               Precio == otro.Precio &&
               Licencias.SequenceEqual(otro.Licencias);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Nombre, Categoria, Formato, Precio);
    }
}
