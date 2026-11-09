namespace AppForSEII.API.DTOs.Modelo3DDTOs;

public class LineaCompraModelo3DDetailDTO
{
    public LineaCompraModelo3DDetailDTO(string nombreModelo, string categoria, FormatoModelo3D formato, decimal precioUnidad, int cantidadLicencias, decimal subtotal, List<LicenciaModelo3DDTO> licencias)
    {
        NombreModelo = nombreModelo;
        Categoria = categoria;
        Formato = formato;
        PrecioUnidad = precioUnidad;
        CantidadLicencias = cantidadLicencias;
        Subtotal = subtotal;
        Licencias = licencias;
    }

    public string NombreModelo { get; set; }
    public string Categoria { get; set; }
    public FormatoModelo3D Formato { get; set; }
    public decimal PrecioUnidad { get; set; }
    public int CantidadLicencias { get; set; }
    public decimal Subtotal { get; set; }

    public List<LicenciaModelo3DDTO> Licencias { get; set; }

    public string? AvisoLicencia => LicenciaModelo3DDTO.GenerarAviso(Licencias);

    public override bool Equals(object? obj)
    {
        return obj is LineaCompraModelo3DDetailDTO otra &&
               NombreModelo == otra.NombreModelo &&
               Categoria == otra.Categoria &&
               Formato == otra.Formato &&
               PrecioUnidad == otra.PrecioUnidad &&
               CantidadLicencias == otra.CantidadLicencias &&
               Subtotal == otra.Subtotal &&
               Licencias.SequenceEqual(otra.Licencias);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(NombreModelo, Categoria, Formato, PrecioUnidad, CantidadLicencias, Subtotal);
    }
}
