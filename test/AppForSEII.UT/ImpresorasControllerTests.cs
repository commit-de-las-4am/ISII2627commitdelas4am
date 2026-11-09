using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs.ReservaImpresoraDTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
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

        [Fact]
        public async Task GetImpresorasParaReserva_SinFiltros_DevuelveOkConTodasLasImpresoras()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            var listaEsperada = new List<ImpresoraParaReservaDTO>
            {
                new ImpresoraParaReservaDTO(2, "Anycubic Photon Mono", "4K", TipoImpresora.Resina, "Impresora MSLA de alta resolución en resina", 0.08m, 15.0m),
                new ImpresoraParaReservaDTO(1, "Creality Ender 3", "V2", TipoImpresora.Filamento, "Impresora FDM para filamento PLA/PETG", 0.15m, 10.0m),
                new ImpresoraParaReservaDTO(3, "Formlabs Form 3", "Plus", TipoImpresora.Polimero, "Impresora industrial de polímero avanzado", 0.25m, 30.0m)
            };

            // Act
            var result = await controller.GetImpresorasParaReserva();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var listaObtenida = Assert.IsAssignableFrom<IList<ImpresoraParaReservaDTO>>(okResult.Value);

            Assert.Equal(listaEsperada.Count, listaObtenida.Count);
            Assert.True(listaEsperada.SequenceEqual(listaObtenida));
        }

        [Fact]
        public async Task GetImpresorasParaReserva_FiltroPorNombre_DevuelveOkConCoincidencias()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            var listaEsperada = new List<ImpresoraParaReservaDTO>
            {
                new ImpresoraParaReservaDTO(1, "Creality Ender 3", "V2", TipoImpresora.Filamento, "Impresora FDM para filamento PLA/PETG", 0.15m, 10.0m)
            };

            // Act
            var result = await controller.GetImpresorasParaReserva(nombre: "Ender");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var listaObtenida = Assert.IsAssignableFrom<IList<ImpresoraParaReservaDTO>>(okResult.Value);

            Assert.Single(listaObtenida);
            Assert.True(listaEsperada.SequenceEqual(listaObtenida));
        }

        [Fact]
        public async Task GetImpresorasParaReserva_FiltroPorTipo_DevuelveOkConCoincidencias()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            var listaEsperada = new List<ImpresoraParaReservaDTO>
            {
                new ImpresoraParaReservaDTO(2, "Anycubic Photon Mono", "4K", TipoImpresora.Resina, "Impresora MSLA de alta resolución en resina", 0.08m, 15.0m)
            };

            // Act
            var result = await controller.GetImpresorasParaReserva(tipo: TipoImpresora.Resina);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var listaObtenida = Assert.IsAssignableFrom<IList<ImpresoraParaReservaDTO>>(okResult.Value);

            Assert.Single(listaObtenida);
            Assert.True(listaEsperada.SequenceEqual(listaObtenida));
        }

        [Fact]
        public async Task GetImpresorasParaReserva_FiltroCombinadoNombreYTipo_DevuelveOk()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            var listaEsperada = new List<ImpresoraParaReservaDTO>
            {
                new ImpresoraParaReservaDTO(3, "Formlabs Form 3", "Plus", TipoImpresora.Polimero, "Impresora industrial de polímero avanzado", 0.25m, 30.0m)
            };

            // Act
            var result = await controller.GetImpresorasParaReserva(nombre: "Formlabs", tipo: TipoImpresora.Polimero);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var listaObtenida = Assert.IsAssignableFrom<IList<ImpresoraParaReservaDTO>>(okResult.Value);

            Assert.Single(listaObtenida);
            Assert.True(listaEsperada.SequenceEqual(listaObtenida));
        }

        [Fact]
        public async Task GetImpresorasParaReserva_SinCoincidencias_DevuelveNotFound()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            // Act
            var result = await controller.GetImpresorasParaReserva(nombre: "Inexistente");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No se encontraron impresoras 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetImpresorasParaReserva_BaseDeDatosVacia_DevuelveNotFound()
        {
            // Arrange (sin seed data)
            var controller = new ImpresorasController(_context, _loggerMock.Object);

            // Act
            var result = await controller.GetImpresorasParaReserva();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No se encontraron impresoras 3D disponibles con los criterios seleccionados.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetImpresorasParaReserva_ModelStateInvalido_DevuelveBadRequest()
        {
            // Arrange
            var controller = new ImpresorasController(_context, _loggerMock.Object);
            controller.ModelState.AddModelError("Error", "Error de modelo forzado");

            // Act
            var result = await controller.GetImpresorasParaReserva();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error: los filtros no son válidos.", badRequestResult.Value);
        }
    }
}
