using System.Globalization;
using AppForSEII.API.Controllers;
using AppForSEII.API.DTOs.Modelo3DDTOs;

namespace AppForSEII.UT.Modelo3DControllerTest
{
    public class GetModelos3DParaComprarTest : AppForSEII4SqliteUT
    {
        private static readonly DateTime _licenciaVigenteHasta = new DateTime(2038, 1, 23);
        private static readonly DateTime _licenciaCaducadaEn = new DateTime(2021, 4, 1);
        private const string _nombreLicenciaVigente = "Licencia Estandar";
        private const string _nombreLicenciaCaducada = "Licencia Caducada";

        public GetModelos3DParaComprarTest()
        {
            var modelos = new List<Modelo3D>()
            {
                Modelo("Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m,
                    Licencia(_nombreLicenciaVigente, _licenciaVigenteHasta)),                  
                Modelo("Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m,
                    Licencia(_nombreLicenciaVigente, _licenciaVigenteHasta),
                    Licencia(_nombreLicenciaCaducada, _licenciaCaducadaEn)),                   
                Modelo("Jarron moderno", "Decoracion", FormatoModelo3D.TresMF, 8.00m,
                    Licencia(_nombreLicenciaVigente, _licenciaVigenteHasta)),                  
                // todas sus licencias están caducadas: no tiene que salir
                Modelo("Modelo antiguo", "Decoracion", FormatoModelo3D.STL, 5.00m,
                    Licencia(_nombreLicenciaCaducada, _licenciaCaducadaEn)),                   
            };
            _context.AddRange(modelos);
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

        public static IEnumerable<object?[]> TestCasesFor_GetModelos3DParaComprar()
        {
            var licenciasVigentes = new List<LicenciaModelo3DDTO>() { new LicenciaModelo3DDTO(_nombreLicenciaVigente, _licenciaVigenteHasta) };

            var robot = new Modelo3DParaCompraDTO(1, "Robot articulado", "Personajes", FormatoModelo3D.OBJ, 15.00m, licenciasVigentes);
            var dragon = new Modelo3DParaCompraDTO(2, "Dragon de fuego", "Personajes", FormatoModelo3D.STL, 25.50m, licenciasVigentes);
            var jarron = new Modelo3DParaCompraDTO(3, "Jarron moderno", "Decoracion", FormatoModelo3D.TresMF, 8.00m, licenciasVigentes);

            var sinFiltros = new List<Modelo3DParaCompraDTO>() { dragon, jarron, robot };
            var filtroNombre = new List<Modelo3DParaCompraDTO>() { robot };
            var filtroCategoria = new List<Modelo3DParaCompraDTO>() { jarron };   
            var filtroNombreYCategoria = new List<Modelo3DParaCompraDTO>() { dragon };

            var allTests = new List<object?[]>
            {
                new object?[] { null, null, sinFiltros },
                new object?[] { "Robot", null, filtroNombre },
                new object?[] { null, "Decoracion", filtroCategoria },
                new object?[] { "Dragon", "Personajes", filtroNombreYCategoria },
            };
            return allTests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetModelos3DParaComprar))]
        public async Task GetModelos3DParaComprar_Filtros_test(string? nombre, string? categoria, List<Modelo3DParaCompraDTO> modelosEsperados)
        {
            var mock = new Mock<ILogger<Modelo3DController>>();
            ILogger<Modelo3DController> logger = mock.Object;
            var controller = new Modelo3DController(_context, logger);

            var result = await controller.GetModelos3DParaComprar(nombre, categoria);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var modelosActual = Assert.IsType<List<Modelo3DParaCompraDTO>>(okResult.Value);
            Assert.Equal(modelosEsperados, modelosActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_SinResultados_NotFound_test()
        {
            var mock = new Mock<ILogger<Modelo3DController>>();
            var controller = new Modelo3DController(_context, mock.Object);

            var result = await controller.GetModelos3DParaComprar("No existe", null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No existen modelos 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_LicenciaCaducada_NotFound_test()
        {
            var mock = new Mock<ILogger<Modelo3DController>>();
            var controller = new Modelo3DController(_context, mock.Object);

            var result = await controller.GetModelos3DParaComprar("Modelo antiguo", null);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No existen modelos 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_FiltrosNoValidos_BadRequest_test()
        {
            var mock = new Mock<ILogger<Modelo3DController>>();
            var controller = new Modelo3DController(_context, mock.Object);
            controller.ModelState.AddModelError("nombre", "El filtro no es válido");

            var result = await controller.GetModelos3DParaComprar("Robot", null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: los filtros no son válidos.", badRequestResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetModelos3DParaComprar_LicenciaProximaACaducar_Aviso_test()
        {
            var caduca = DateTime.Today.AddDays(10);
            _context.Modelos3D.Add(Modelo("Lampara", "Decoracion", FormatoModelo3D.STL, 12.00m,
                Licencia("Licencia Temporal", caduca)));
            _context.SaveChanges();

            var mock = new Mock<ILogger<Modelo3DController>>();
            var controller = new Modelo3DController(_context, mock.Object);

            var resultLampara = await controller.GetModelos3DParaComprar("Lampara", null);
            var resultRobot = await controller.GetModelos3DParaComprar("Robot", null);

            var okLampara = Assert.IsType<OkObjectResult>(resultLampara);
            var lampara = Assert.Single(Assert.IsType<List<Modelo3DParaCompraDTO>>(okLampara.Value));
            Assert.NotNull(lampara.AvisoLicencia);
            Assert.Contains(caduca.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture), lampara.AvisoLicencia!);

            var okRobot = Assert.IsType<OkObjectResult>(resultRobot);
            var robot = Assert.Single(Assert.IsType<List<Modelo3DParaCompraDTO>>(okRobot.Value));
            Assert.Null(robot.AvisoLicencia);
        }
    }
}
