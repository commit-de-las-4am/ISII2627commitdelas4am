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
    public class ReservaImpresorasControllerTests : AppForSEII4SqliteUT
    {
        private readonly Mock<ILogger<ReservaImpresorasController>> _loggerMock;

        public ReservaImpresorasControllerTests()
        {
            _loggerMock = new Mock<ILogger<ReservaImpresorasController>>();
        }

        private async Task SeedDatabaseAsync()
        {
            var impresora1 = new Impresora3D("Creality Ender 3", "V2", TipoImpresora.Filamento, "Impresora FDM para filamento PLA/PETG", 0.20m, 10.0m);
            var impresora2 = new Impresora3D("Anycubic Photon Mono", "4K", TipoImpresora.Resina, "Impresora MSLA de alta resolución en resina", 0.50m, 15.0m);

            _context.Impresoras3D.AddRange(impresora1, impresora2);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task PostReservaImpresora_DatosValidosNuevoCliente_DevuelveCreatedAtAction()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var lineasDTO = new List<LineaReservaCreateDTO>
            {
                new LineaReservaCreateDTO(1, TiempoReserva.DosHoras)
            };

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                lineasDTO
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var reservaObtenida = Assert.IsType<ReservaImpresoraDetailDTO>(createdResult.Value);

            // 10.0m base + (2 horas * 0.20m) = 10.40m
            decimal precioEsperado = 10.40m;
            var lineasEsperadas = new List<LineaReservaItemDTO>
            {
                new LineaReservaItemDTO(
                    "Creality Ender 3",
                    "V2",
                    TipoImpresora.Filamento,
                    "Impresora FDM para filamento PLA/PETG",
                    TiempoReserva.DosHoras,
                    0.20m,
                    10.0m,
                    10.40m
                )
            };

            var reservaEsperada = new ReservaImpresoraDetailDTO(
                reservaObtenida.Id,
                reservaObtenida.FechaReserva,
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                precioEsperado,
                lineasEsperadas
            );

            Assert.True(reservaEsperada.Equals(reservaObtenida));
        }

        [Fact]
        public async Task PostReservaImpresora_ClienteExistente_ReutilizaClienteYDevuelveCreatedAtAction()
        {
            // Arrange
            await SeedDatabaseAsync();
            var clienteExistente = new Cliente(
                "cliente-1",
                "Juan",
                "Perez",
                "juan.perez@alu.uclm.es",
                "Campus Universitario UCLM"
            )
            {
                PhoneNumber = "612345678"
            };
            _context.Clientes.Add(clienteExistente);
            await _context.SaveChangesAsync();

            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var lineasDTO = new List<LineaReservaCreateDTO>
            {
                new LineaReservaCreateDTO(2, TiempoReserva.UnaHora)
            };

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.PayPal,
                lineasDTO
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var reservaObtenida = Assert.IsType<ReservaImpresoraDetailDTO>(createdResult.Value);

            // 15.0m base + (1 hora * 0.50m) = 15.50m
            decimal precioEsperado = 15.50m;
            var lineasEsperadas = new List<LineaReservaItemDTO>
            {
                new LineaReservaItemDTO(
                    "Anycubic Photon Mono",
                    "4K",
                    TipoImpresora.Resina,
                    "Impresora MSLA de alta resolución en resina",
                    TiempoReserva.UnaHora,
                    0.50m,
                    15.0m,
                    15.50m
                )
            };

            var reservaEsperada = new ReservaImpresoraDetailDTO(
                reservaObtenida.Id,
                reservaObtenida.FechaReserva,
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.PayPal,
                precioEsperado,
                lineasEsperadas
            );

            Assert.True(reservaEsperada.Equals(reservaObtenida));
        }

        [Fact]
        public async Task PostReservaImpresora_ImpresoraNoExiste_DevuelveNotFound()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var lineasDTO = new List<LineaReservaCreateDTO>
            {
                new LineaReservaCreateDTO(999, TiempoReserva.UnaHora)
            };

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                lineasDTO
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("La impresora con ID 999 no existe.", notFoundResult.Value);
        }

        [Fact]
        public async Task PostReservaImpresora_SinLineasDeReserva_DevuelveBadRequest()
        {
            // Arrange
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                new List<LineaReservaCreateDTO>()
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Debe incluir al menos una impresora para ser reservada.", badRequestResult.Value);
        }

        [Fact]
        public async Task PostReservaImpresora_TiempoReservaNoValido_DevuelveBadRequest()
        {
            // Arrange
            await SeedDatabaseAsync();
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var lineasDTO = new List<LineaReservaCreateDTO>
            {
                new LineaReservaCreateDTO(1, TiempoReserva.NoDefinido)
            };

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                lineasDTO
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("El tiempo de reserva especificado no es válido para la impresora Creality Ender 3.", badRequestResult.Value);
        }

        [Fact]
        public async Task PostReservaImpresora_ModelStateInvalido_DevuelveBadRequest()
        {
            // Arrange
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);
            controller.ModelState.AddModelError("Error", "Error de modelo forzado");

            var reservaDTO = new ReservaImpresoraCreateDTO(
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                new List<LineaReservaCreateDTO>()
            );

            // Act
            var result = await controller.PostReservaImpresora(reservaDTO);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Los datos de la reserva no son válidos.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetReservaImpresora_IdExistente_DevuelveOkConDetalles()
        {
            // Arrange
            await SeedDatabaseAsync();
            var cliente = new Cliente(
                "cliente-test",
                "Juan",
                "Perez",
                "juan.perez@alu.uclm.es",
                "Campus Universitario UCLM"
            )
            {
                PhoneNumber = "612345678"
            };
            _context.Clientes.Add(cliente);

            var reserva = new ReservaImpresora(
                0,
                DateTime.Now,
                10.40m,
                MetodoPago.TarjetaCredito,
                cliente.Id
            )
            {
                Cliente = cliente
            };

            var impresora = await _context.Impresoras3D.FindAsync(1);
            var linea = new LineaReserva(
                0,
                TiempoReserva.DosHoras,
                10.40m,
                1,
                0
            )
            {
                Impresora = impresora!
            };
            reserva.LineasReserva.Add(linea);

            _context.ReservasImpresoras.Add(reserva);
            await _context.SaveChangesAsync();

            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            var lineasEsperadas = new List<LineaReservaItemDTO>
            {
                new LineaReservaItemDTO(
                    "Creality Ender 3",
                    "V2",
                    TipoImpresora.Filamento,
                    "Impresora FDM para filamento PLA/PETG",
                    TiempoReserva.DosHoras,
                    0.20m,
                    10.0m,
                    10.40m
                )
            };

            var reservaEsperada = new ReservaImpresoraDetailDTO(
                reserva.Id,
                reserva.FechaReserva,
                "Juan",
                "Perez",
                "Campus Universitario UCLM",
                "612345678",
                MetodoPago.TarjetaCredito,
                10.40m,
                lineasEsperadas
            );

            // Act
            var result = await controller.GetReservaImpresora(reserva.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservaObtenida = Assert.IsType<ReservaImpresoraDetailDTO>(okResult.Value);
            Assert.True(reservaEsperada.Equals(reservaObtenida));
        }

        [Fact]
        public async Task GetReservaImpresora_IdInexistente_DevuelveNotFound()
        {
            // Arrange
            var controller = new ReservaImpresorasController(_context, _loggerMock.Object);

            // Act
            var result = await controller.GetReservaImpresora(999);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("La reserva con ID 999 no existe.", notFoundResult.Value);
        }
    }
}
