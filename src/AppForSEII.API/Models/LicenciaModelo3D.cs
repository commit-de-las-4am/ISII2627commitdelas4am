public class LicenciaModelo3D
{
    public LicenciaModelo3D()
    {
        Nombre = string.Empty;
        FechaExpiracion = DateTime.Now;
        Modelo3D = new Modelo3D();
    }
    public LicenciaModelo3D(int id, string nombre, DateTime fechaExpiracion)
    {
        Id = id;
        Nombre = nombre;
        FechaExpiracion = fechaExpiracion;
        Modelo3D = new Modelo3D();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "El nombre de la licencia no puede tener más de 100 caracteres.")]
    public string Nombre { get; set; }

    [Required]
    public DateTime FechaExpiracion { get; set; }
    
    [Required]
    public int Modelo3DId { get; set; }
    
    [ForeignKey("Modelo3DId")]
    public Modelo3D Modelo3D { get; set; }
}