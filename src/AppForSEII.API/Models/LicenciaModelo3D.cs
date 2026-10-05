public class LicenciaModelo3D
{
    public LicenciaModelo3D()
    {
    }
    public LicenciaModelo3D(int id, string nombre, DateTime fechaExpiracion)
    {
        Id = id;
        Nombre = nombre;
        FechaExpiracion = fechaExpiracion;
    }
    public int Id { get; set; }
    public string Nombre { get; set; }
    public DateTime FechaExpiracion { get; set; }

    public int Modelo3DId { get; set; }

    public Modelo3D Modelo3D { get; set; }
}