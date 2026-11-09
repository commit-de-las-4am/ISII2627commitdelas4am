using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.CompraModelo3DControllerTest
{
    public class CreateCompraModelo3DTest : AppForSEII4SqliteUT
    {
        private const string _nombre = "Alejandro";
        private const string _apellidos = "Escribano Palomares";
        private const string _correo = "alejandro@uclm.es";
        private const string _direccion = "Calle Carmelitas 2";
        private static readonly DateTime _licenciaVigenteHasta = new DateTime(2038, 1, 23);
        private static readonly DateTime _licenciaCaducadaEn = new DateTime(2021, 4, 1);

        public CreateCompraModelo3DTest()
        {
            var modelos = new List<Modelo3D>()
            {
                Modelo("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m,
                    Licencia("Licencia Estándar", _licenciaVigenteHasta)),   
                Modelo("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m,
                    Licencia("Licencia Estándar", _licenciaVigenteHasta)),   
                // sin licencias vigentes: no se puede comprar
                Modelo("Jarron antiguo", "Decoracion", FormatoModelo3D.TresMF, 8.00m,
                    Licencia("Licencia Caducada", _licenciaCaducadaEn)),     
            };

            var cliente = new Cliente
            {
                Id = "1",
                Name = _nombre,
                Surname = _apellidos,
                UserName = _correo,
                Email = _correo,
                DireccionFacturacion = _direccion
            };

            _context.AddRange(modelos);
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        private static Modelo3D Modelo(string nombre, string categoria, FormatoModelo3D formato, decimal precio, params LicenciaModelo3D[] licencias)
        {
            return new Modelo3D
            {
                Nombre = nombre,
                Categoria = categoria,
                Formato = formato,
                Precio = precio,
                Licencias = licencias.ToList()
            };
        }

        private static LicenciaModelo3D Licencia(string nombre, DateTime fechaExpiracion)
        {
            return new LicenciaModelo3D { Nombre = nombre, FechaExpiracion = fechaExpiracion };
        }

        public static IEnumerable<object[]> TestCasesFor_CreateCompraModelo3D_Error()
        {
            var lineasValidas = new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(1, 1) };

            var carritoVacio = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>());
            
            var pagoConBizum = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.Bizum, lineasValidas);
            
            var clienteNoRegistrado = new CompraModelo3DCreateDTO(_nombre, _apellidos, "otro@uclm.es", _direccion, null,
                MetodoPago.PayPal, lineasValidas);
            
            var modeloInexistente = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(99, 1) });
            
            var cantidadCero = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(1, 0) });
            
            var modeloRepetido = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(1, 1), new LineaCompraModelo3DCreateDTO(1, 2) });
            
            var modeloSinLicenciaVigente = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(3, 1) });

            var allTests = new List<object[]>
            {
                new object[] { carritoVacio, "Error: debes comprar al menos un modelo 3D." },
                new object[] { pagoConBizum, "Error: el método de pago debe ser tarjeta de crédito o PayPal." },
                new object[] { clienteNoRegistrado, "Error: el cliente no está registrado." },
                new object[] { modeloInexistente, "El modelo 3D seleccionado no existe." },
                new object[] { cantidadCero, "Error: la cantidad de licencias debe ser al menos 1." },
                new object[] { modeloRepetido, "Error: el modelo 3D con id 1 aparece repetido en la compra." },
                new object[] { modeloSinLicenciaVigente, "Error: el modelo 3D 'Jarron antiguo' no tiene licencias vigentes." },
            };
            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateCompraModelo3D_Error))]
        public async Task CreateCompraModelo3D_Error_test(CompraModelo3DCreateDTO compraDTO, string errorEsperado)
        {
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            var controller = new CompraModelo3DController(_context, mock.Object);

            var result = await controller.CreateCompraModelo3D(compraDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorActual = Assert.IsType<string>(badRequestResult.Value);
            Assert.Equal(errorEsperado, errorActual);
            Assert.Empty(_context.ComprasModelo3D);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateCompraModelo3D_DatosNoValidos_BadRequest_test()
        {
            //Arrange: flujo alternativo 7.1, falta algún dato obligatorio
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            var controller = new CompraModelo3DController(_context, mock.Object);
            controller.ModelState.AddModelError("DireccionFacturacion", "La dirección de facturación es obligatoria");
            var compraDTO = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, "", null,
                MetodoPago.TarjetaCredito, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(1, 1) });

            var result = await controller.CreateCompraModelo3D(compraDTO);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: los datos de la compra no son válidos.", badRequestResult.Value);
            Assert.Empty(_context.ComprasModelo3D);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateCompraModelo3D_Success_test()
        {
            var mock = new Mock<ILogger<CompraModelo3DController>>();
            var controller = new CompraModelo3DController(_context, mock.Object);

            var compraDTO = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, "Compra para mi proyecto",
                MetodoPago.TarjetaCredito,
                new List<LineaCompraModelo3DCreateDTO>()
                {
                    new LineaCompraModelo3DCreateDTO(1, 2),
                    new LineaCompraModelo3DCreateDTO(2, 1),
                });

            decimal subtotalRobot = 2 * 15.00m;
            decimal subtotalDragon = 1 * 25.50m;
            decimal precioTotal = subtotalRobot + subtotalDragon;
            var licencias = new List<LicenciaModelo3DDTO>() { new LicenciaModelo3DDTO("Licencia Estándar", _licenciaVigenteHasta) };

            var compraEsperada = new CompraModelo3DDetailDTO(1, DateTime.Now, _nombre, _apellidos, _correo, _direccion,
                MetodoPago.TarjetaCredito, "Compra para mi proyecto", precioTotal,
                new List<LineaCompraModelo3DDetailDTO>()
                {
                    new LineaCompraModelo3DDetailDTO("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, 2, subtotalRobot, licencias),
                    new LineaCompraModelo3DDetailDTO("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, 1, subtotalDragon, licencias),
                });

            var result = await controller.CreateCompraModelo3D(compraDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetCompraModelo3D", createdResult.ActionName);
            Assert.Equal("CompraModelo3DDetails", createdResult.ControllerName);
            var compraActual = Assert.IsType<CompraModelo3DDetailDTO>(createdResult.Value);
            Assert.Equal(compraEsperada, compraActual);

            Assert.Equal(1, _context.ComprasModelo3D.Count());
            Assert.Equal(2, _context.LineaCompraModelos.Count());
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreateCompraModelo3D_LicenciaProximaACaducar_Aviso_test()
        {
            var caduca = DateTime.Today.AddDays(10);
            _context.Modelos3D.Add(Modelo("Lampara", "Decoracion", FormatoModelo3D.STL, 12.00m,
                Licencia("Licencia Temporal", caduca)));   
            _context.SaveChanges();

            var mock = new Mock<ILogger<CompraModelo3DController>>();
            var controller = new CompraModelo3DController(_context, mock.Object);
            var compraDTO = new CompraModelo3DCreateDTO(_nombre, _apellidos, _correo, _direccion, null,
                MetodoPago.PayPal, new List<LineaCompraModelo3DCreateDTO>() { new LineaCompraModelo3DCreateDTO(4, 1) });

            var result = await controller.CreateCompraModelo3D(compraDTO);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var compra = Assert.IsType<CompraModelo3DDetailDTO>(createdResult.Value);
            Assert.Null(compra.Descripcion);
            Assert.Equal(12.00m, compra.PrecioTotal);
            var linea = Assert.Single(compra.LineasCompra);
            Assert.NotNull(linea.AvisoLicencia);
            Assert.Contains(caduca.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture), linea.AvisoLicencia!);
        }
    }
}
