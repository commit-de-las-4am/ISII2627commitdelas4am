public class Cliente : ApplicationUser{
    public Cliente(string direccionFacturacion)
    {
        DireccionFacturacion = direccionFacturacion;
    }

    public string DireccionFacturacion{get;set;}

    public List<CompraModelo3D> Compras {get;set;}
}
