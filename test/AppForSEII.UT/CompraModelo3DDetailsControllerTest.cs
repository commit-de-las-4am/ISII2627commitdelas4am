using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.CompraModelo3DDetailsController_test
{
    public class GetCompraModelo3D_test : AppForSEII4SqliteUT
    {
        private static readonly DateTime _licenciaVigenteHasta = new DateTime(2038, 1, 23);

        public GetCompraModelo3D_test()
        {
            var robot = new Modelo3D
            {
                Nombre = "Robot articulado",
                Categoria = "Personajes",
                Formato = FormatoModelo3D.OBJ,
                Precio = 15.00m,
                Licencias = new List<LicenciaModelo3D>() { new LicenciaModelo3D { Nombre = "Licencia Estándar", FechaExpiracion = _licenciaVigenteHasta } }
            };
            var dragon = new Modelo3D
            {
                Nombre = "Dragon de fuego",
                Categoria = "Personajes",
                Formato = FormatoModelo3D.STL,
                Precio = 25.50m,
                Licencias = new List<LicenciaModelo3D>() { new LicenciaModelo3D { Nombre = "Licencia Estándar", FechaExpiracion = _licenciaVigenteHasta } }
            };

            var cliente = new Cliente
            {
                Id = "1",
                Name = "Alejandro",
                Surname = "Escribano Palomares",
                UserName = "alejandro@uclm.es",
                Email = "alejandro@uclm.es",
                DireccionFacturacion = "Calle Carmelitas 2"
            };

            var compra = new CompraModelo3D
            {
                FechaCompra = DateTime.Now,
                NombreCliente = "Alejandro",
                ApellidosCliente = "Escribano Palomares",
                CorreoElectronico = "alejandro@uclm.es",
                DireccionFacturacion = "Calle Carmelitas 2",
                Descripcion = "Compra para mi proyecto",
                PrecioTotal = 55.50m,
                MetodoPago = MetodoPago.TarjetaCredito,
                ClienteId = cliente.Id,
                Cliente = cliente,
                LineasCompraModelo = new List<LineaCompraModelo>()
            };
            compra.LineasCompraModelo.Add(new LineaCompraModelo { CantidadLicencias = 2, PrecioUnidad = 15.00m, Subtotal = 30.00m, Modelo3D = robot, CompraModelo3D = compra });
            compra.LineasCompraModelo.Add(new LineaCompraModelo { CantidadLicencias = 1, PrecioUnidad = 25.50m, Subtotal = 25.50m, Modelo3D = dragon, CompraModelo3D = compra });

            _context.Add(compra);
            _context.SaveChanges();
        }

        [Fact]
        [Trait("Database", "WithoutFixture")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetCompraModelo3D_NotFound_test()
        {
            var mock = new Mock<ILogger<CompraModelo3DDetailsController>>();
            var controller = new CompraModelo3DDetailsController(_context, mock.Object);

            var result = await controller.GetCompraModelo3D(0);

            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetCompraModelo3D_Found_test()
        {
            var mock = new Mock<ILogger<CompraModelo3DDetailsController>>();
            var controller = new CompraModelo3DDetailsController(_context, mock.Object);

            var licencias = new List<LicenciaModelo3DDTO>() { new LicenciaModelo3DDTO("Licencia Estándar", _licenciaVigenteHasta) };
            var compraEsperada = new CompraModelo3DDetailDTO(1, DateTime.Now, "Alejandro", "Escribano Palomares", "alejandro@uclm.es",
                "Calle Carmelitas 2", MetodoPago.TarjetaCredito, "Compra para mi proyecto", 55.50m,
                new List<LineaCompraModelo3DDetailDTO>());
            compraEsperada.LineasCompra.Add(new LineaCompraModelo3DDetailDTO("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, 2, 30.00m, licencias));
            compraEsperada.LineasCompra.Add(new LineaCompraModelo3DDetailDTO("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, 1, 25.50m, licencias));

            var result = await controller.GetCompraModelo3D(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var compraActual = Assert.IsType<CompraModelo3DDetailDTO>(okResult.Value);
            Assert.Equal(compraEsperada, compraActual);
        }
    }
}
