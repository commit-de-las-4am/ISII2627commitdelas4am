using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Xunit;

namespace AppForSEII.UT
{
    public class ImpresorasControllerTests : AppForSEII4SqliteUT
    {
        private readonly Mock<ILogger<ImpresorasController>> _loggerMock;

        public ImpresorasControllerTests()
        {
            _loggerMock = new Mock<ILogger<ImpresorasController>>();
        }

        private async Task SeedDatabaseAsync()
        {
            var impresora1 = new Impresora3D("Creality Ender 3", "V2", TipoImpresora.Filamento, "Impresora FDM para filamento PLA/PETG", 0.15m, 10.0m);
            var impresora2 = new Impresora3D("Anycubic Photon Mono", "4K", TipoImpresora.Resina, "Impresora MSLA de alta resolución en resina", 0.08m, 15.0m);
            var impresora3 = new Impresora3D("Formlabs Form 3", "Plus", TipoImpresora.Polimero, "Impresora industrial de polímero avanzado", 0.25m, 30.0m);

            _context.Impresoras3D.AddRange(impresora1, impresora2, impresora3);
            await _context.SaveChangesAsync();
        }

        public static IEnumerable<object?[]> TestCasesFor_GetImpresorasParaReserva()
        {
            var impresora1 = new ImpresoraParaReservaDTO(1, "Creality Ender 3", "V2", TipoImpresora.Filamento, "Impresora FDM para filamento PLA/PETG", 0.15m, 10.0m);
            var impresora2 = new ImpresoraParaReservaDTO(2, "Anycubic Photon Mono", "4K", TipoImpresora.Resina, "Impresora MSLA de alta resolución en resina", 0.08m, 15.0m);
            var impresora3 = new ImpresoraParaReservaDTO(3, "Formlabs Form 3", "Plus", TipoImpresora.Polimero, "Impresora industrial de polímero avanzado", 0.25m, 30.0m);

            var todasOrdenadas = new List<ImpresoraParaReservaDTO> { impresora2, impresora1, impresora3 };
            var filtroNombre = new List<ImpresoraParaReservaDTO> { impresora1 };
            var filtroTipo = new List<ImpresoraParaReservaDTO> { impresora2 };
            var filtroCombinado = new List<ImpresoraParaReservaDTO> { impresora3 };

            return new List<object?[]>
            {
                new object?[] { null, null, todasOrdenadas },
                new object?[] { "Ender", null, filtroNombre },
                new object?[] { null, TipoImpresora.Resina, filtroTipo },
                new object?[] { "Formlabs", TipoImpresora.Polimero, filtroCombinado },
                new object?[] { null, TipoImpresora.NoDefinido, todasOrdenadas }
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_GetImpresorasParaReserva))]
        public async Task GetImpresorasParaReserva_Filtros_DevuelveOkConCoincidencias(
            string? nombre,
            TipoImpresora? tipo,
            List<ImpresoraParaReservaDTO> listaEsperada)
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);
            int expectedStatusCode = (int)HttpStatusCode.OK;

            // Act
            var result = await controller.GetImpresorasParaReserva(nombre, tipo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedStatusCode, okResult.StatusCode);

            var listaObtenida = Assert.IsType<List<ImpresoraParaReservaDTO>>(okResult.Value);
            Assert.Equal(listaEsperada, listaObtenida);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetImpresorasParaReserva_SinCoincidencias_DevuelveNotFound()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);
            int expectedStatusCode = (int)HttpStatusCode.NotFound;
            string expectedMessage = "No se encontraron impresoras 3D disponibles con los criterios seleccionados.";

            // Act
            var result = await controller.GetImpresorasParaReserva(nombre: "Inexistente");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
            Assert.Equal(expectedMessage, notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetImpresorasParaReserva_BaseDeDatosVacia_DevuelveNotFound()
        {
            // Arrange (sin seed data)
            var controller = new ImpresorasController(_context, _loggerMock.Object);
            int expectedStatusCode = (int)HttpStatusCode.NotFound;
            string expectedMessage = "No se encontraron impresoras 3D disponibles con los criterios seleccionados.";

            // Act
            var result = await controller.GetImpresorasParaReserva();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
            Assert.Equal(expectedMessage, notFoundResult.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task GetImpresorasParaReserva_ModelStateInvalido_DevuelveBadRequest()
        {
            // Arrange
            var controller = new ImpresorasController(_context, _loggerMock.Object);
            controller.ModelState.AddModelError("Error", "Error de modelo forzado");
            int expectedStatusCode = (int)HttpStatusCode.BadRequest;
            string expectedMessage = "Error: los filtros no son válidos.";

            // Act
            var result = await controller.GetImpresorasParaReserva();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(expectedStatusCode, badRequestResult.StatusCode);
            Assert.Equal(expectedMessage, badRequestResult.Value);
        }
    }
}
