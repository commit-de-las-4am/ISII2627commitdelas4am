public class Cliente : ApplicationUser{
    public Cliente()
    {    
    }
    public Cliente(string direccionFacturacion)
    {
        DireccionFacturacion = direccionFacturacion;
    }
    public string DireccionFacturacion{get;set;}
}
