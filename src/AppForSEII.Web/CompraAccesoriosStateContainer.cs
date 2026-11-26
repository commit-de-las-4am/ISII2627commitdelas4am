using AppForSEII.Web.API;

namespace AppForSEII.Web
{
    public class CompraAccesoriosStateContainer
    {
        public CompraAccesorioCreateDTO Compra { get; private set; } = new CompraAccesorioCreateDTO();

        private readonly Dictionary<int, AccesorioParaComprarDTO> _accesoriosInfo = new();

        public double PrecioTotal => Compra.LineasCompra.Sum(l =>
            l.Cantidad * (_accesoriosInfo.TryGetValue(l.AccesorioId, out var acc) ? acc.Precio : 0));

        public AccesorioParaComprarDTO? GetAccesorioInfo(int accesorioId) =>
            _accesoriosInfo.TryGetValue(accesorioId, out var acc) ? acc : null;

        public void AddAccesorioToCompra(AccesorioParaComprarDTO accesorio)
        {
            if (accesorio.CantidadDisponible <= 0) return;

            _accesoriosInfo[accesorio.Id] = accesorio;

            var existente = Compra.LineasCompra.FirstOrDefault(l => l.AccesorioId == accesorio.Id);
            if (existente != null)
            {
                if (existente.Cantidad < accesorio.CantidadDisponible)
                    existente.Cantidad++;
            }
            else
            {
                Compra.LineasCompra.Add(new LineaCompraAccesorioCreateDTO
                {
                    AccesorioId = accesorio.Id,
                    Cantidad = 1
                });
            }
        }

        public void RemoveLineaCompra(LineaCompraAccesorioCreateDTO linea)
        {
            Compra.LineasCompra.Remove(linea);
        }

        public void ClearCart()
        {
            Compra.LineasCompra.Clear();
            _accesoriosInfo.Clear();
        }

        public void CompraProcessed()
        {
            Compra = new CompraAccesorioCreateDTO();
            _accesoriosInfo.Clear();
        }
    }
}