public class Cliente : ApplicationUser{
    public Cliente()
    {
    }
    public Cliente(string direccionFacturacion)
    {
        DireccionFacturacion = direccionFacturacion;
    }

    public string DireccionFacturacion{get;set;}
   
    [StringLength(200, ErrorMessage = "La dirección no puede tener más de 200 caracteres.")]
    

    public List<CompraModelo3D> Compras {get;set;}
}
