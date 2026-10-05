public class Modelo3D
{
    public Modelo3D()
    {
    }

    public Modelo3D(int id, string nombre, string categoria, FormatoModelo3D formato, decimal precio)
    {
        Id = id;
        Nombre = nombre;
        Categoria = categoria;
        Formato = formato;
        Precio = precio;
    }
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200, ErrorMessage = "El nombre del modelo no puede tener más de 200 caracteres.")]
    public string Nombre { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "La categoría del modelo no puede tener más de 50 caracteres.")]
    public string Categoria { get; set; }

    [Required]
    public FormatoModelo3D Formato { get; set; }
   
    [Column(TypeName = "decimal(18,2)")]
    [Required]
    public decimal Precio { get; set; }

    public List<LineaCompraModelo> LineasCompraModelo { get; set; }
    public List<LicenciaModelo3D> Licencias { get; set; }
}