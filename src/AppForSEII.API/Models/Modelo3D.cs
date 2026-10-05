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
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public FormatoModelo3D Formato { get; set; }
    public decimal Precio { get; set; }

    public List<LineaCompraModelo> LineasCompraModelo { get; set; }
    public List<LicenciaModelo3D> Licencias { get; set; }
}