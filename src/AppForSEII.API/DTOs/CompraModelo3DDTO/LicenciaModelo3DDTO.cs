using System.Globalization;

namespace AppForSEII.API.DTOs.Modelo3DDTOs;

public class LicenciaModelo3DDTO
{
    public LicenciaModelo3DDTO(string nombre, DateTime fechaExpiracion)
    {
        Nombre = nombre;
        FechaExpiracion = fechaExpiracion;
    }

    [StringLength(100)]
    public string Nombre { get; set; }

    public DateTime FechaExpiracion { get; set; }

    // Flujo alternativo 7.4: la licencia caduca en el próximo mes
    public bool ExpiraEnProximoMes => FechaExpiracion <= DateTime.Now.AddMonths(1);

    // Licencias de un modelo que todavía no han caducado, ordenadas por fecha de expiración
    public static List<LicenciaModelo3DDTO> VigentesDe(Modelo3D modelo)
    {
        var ahora = DateTime.Now;
        return (modelo.Licencias ?? new List<LicenciaModelo3D>())
            .Where(l => l.FechaExpiracion > ahora)
            .OrderBy(l => l.FechaExpiracion)
            .Select(l => new LicenciaModelo3DDTO(l.Nombre, l.FechaExpiracion))
            .ToList();
    }

    public static string? GenerarAviso(IEnumerable<LicenciaModelo3DDTO> licencias)
    {
        var proximas = licencias
            .Where(l => l.ExpiraEnProximoMes)
            .OrderBy(l => l.FechaExpiracion)
            .ToList();

        if (proximas.Count == 0)
            return null;

        var detalle = string.Join(", ", proximas.Select(l =>
            $"{l.Nombre} (hasta el {l.FechaExpiracion.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)})"));
        return $"Atención: hay licencias que caducan en el próximo mes: {detalle}.";
    }

    public override bool Equals(object? obj)
    {
        return obj is LicenciaModelo3DDTO otra &&
               Nombre == otra.Nombre &&
               FechaExpiracion == otra.FechaExpiracion;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Nombre, FechaExpiracion);
    }
}
