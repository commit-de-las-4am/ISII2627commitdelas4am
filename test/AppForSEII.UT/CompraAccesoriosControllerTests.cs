using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AppForSEII.Tests.Controllers
{
    public class CompraAccesoriosControllerTests
    {
        private ApplicationDbContext ConstruirContextoEnMemoria()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task PostCompraAccesorio_ConDatosValidos_DevuelveCreated()
        {

            var context = ConstruirContextoEnMemoria();
            var accesorio = new Accesorio 
            { 
                Id = 1, 
                Nombre = "Pintura Acrílica", 
                Precio = 10m, 
                CantidadDisponible = 5 
            };
            context.Accesorios.Add(accesorio);
            await context.SaveChangesAsync();

            var controller = new CompraAccesoriosController(context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Jorge",
                ApellidosCliente = "Navarro",
                DireccionEnvio = "Calle Falsa 123",
                NumeroTelefono = "123456789",
                MetodoPago = MetodoPago.TarjetaCredito,
                LineasCompra = new List<LineaCompraAccesorioCreateDTO>
                {
                    new LineaCompraAccesorioCreateDTO { AccesorioId = 1, Cantidad = 2 }
                }
            };

            var result = await controller.PostCompraAccesorio(compraDTO);

            Assert.IsType<CreatedAtActionResult>(result.Result);
            
            var accesorioActualizado = await context.Accesorios.FindAsync(1);
            Assert.Equal(3, accesorioActualizado!.CantidadDisponible);
        }

        [Fact]
        public async Task PostCompraAccesorio_AccesorioNoExiste_DevuelveNotFound()
        {
            var context = ConstruirContextoEnMemoria();
            var controller = new CompraAccesoriosController(context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Jorge",
                LineasCompra = new List<LineaCompraAccesorioCreateDTO>
                {
                    new LineaCompraAccesorioCreateDTO { AccesorioId = 99, Cantidad = 1 }
                }
            };

            var result = await controller.PostCompraAccesorio(compraDTO);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostCompraAccesorio_SinStockSuficiente_DevuelveBadRequest()
        {
            var context = ConstruirContextoEnMemoria();
            var accesorio = new Accesorio 
            { 
                Id = 1, 
                Nombre = "Pintura Acrílica", 
                Precio = 10m, 
                CantidadDisponible = 2
            };
            context.Accesorios.Add(accesorio);
            await context.SaveChangesAsync();

            var controller = new CompraAccesoriosController(context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Jorge",
                LineasCompra = new List<LineaCompraAccesorioCreateDTO>
                {
                    new LineaCompraAccesorioCreateDTO { AccesorioId = 1, Cantidad = 5 }
                }
            };

            var result = await controller.PostCompraAccesorio(compraDTO);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}