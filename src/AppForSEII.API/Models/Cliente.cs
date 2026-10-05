public class Cliente : ApplicationUser{
    public Cliente()
    {
    }
    public Cliente(string direccionFacturacion)
    {
        DireccionFacturacion = direccionFacturacion;
    }

    [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres.")]
    public string DireccionFacturacion{get;set;}

    public List<CompraModelo3D> Compras {get;set;}
}
