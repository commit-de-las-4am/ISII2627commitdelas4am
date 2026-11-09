namespace AppForSEII.API.DTOs.Modelo3DDTOs;

public class LineaCompraModelo3DDTO
{
    public LineaCompraModelo3DDTO()
    {
    }

    public LineaCompraModelo3DDTO(int modelo3DId, int cantidadLicencias)
    {
        Modelo3DId = modelo3DId;
        CantidadLicencias = cantidadLicencias;
    }

    [Range(1, int.MaxValue, ErrorMessage = "El modelo 3D es obligatorio")]
    public int Modelo3DId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe comprar al menos 1 licencia")]
    public int CantidadLicencias { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is LineaCompraModelo3DDTO otra &&
               Modelo3DId == otra.Modelo3DId &&
               CantidadLicencias == otra.CantidadLicencias;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Modelo3DId, CantidadLicencias);
    }
}
