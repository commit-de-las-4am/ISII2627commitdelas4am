public class Cliente : ApplicationUser{
    public Cliente():base()
    {
        DireccionFacturacion = string.Empty;
        Compras = new List<CompraModelo3D>();

    }
    public Cliente(string id, string name, string surname, string userName,string direccionFacturacion):base()
    {
        Id = id;
        Name = name;
        Surname = surname;
        UserName = userName;
        Email = userName;
        DireccionFacturacion = direccionFacturacion;
        Compras = new List<CompraModelo3D>();
    }

    
    [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres.")]
    public string DireccionFacturacion{get;set;}

    public List<CompraModelo3D> Compras {get;set;}
}
