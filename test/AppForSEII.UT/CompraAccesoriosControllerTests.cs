using AppForSEII.API.Controllers;
using AppForSEII.API.Data;
using AppForSEII.API.DTOs;
using AppForSEII.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AppForSEII.UT;
using AppForSEII.API.DTOs.AccesorioDTOs;

namespace AppForSEII.Tests.Controllers
{
    public class CompraAccesoriosControllerTests : AppForSEII4SqliteUT
    {
        [Fact]
        public async Task PostCompraAccesorio_ConDatosValidos_DevuelveCreated()
        {
            var accesorio = new Accesorio(
                1,
                "Pintura Acrílica",
                CategoriaAccesorio.Repuestos, 
                "Compatible FDM",
                5,
                10m
            );
            
            _context.Accesorios.Add(accesorio);
            await _context.SaveChangesAsync(); 

            var controller = new CompraAccesoriosController(_context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Pepe",
                ApellidosCliente = "Fernandez",
                DireccionEnvio = "Calle Falsa 123",
                NumeroTelefono = "123456789",
                MetodoPago = MetodoPago.TarjetaCredito,
                LineasCompra = new List<LineaCompraAccesorioCreateDTO>
                {
                    new LineaCompraAccesorioCreateDTO { AccesorioId = 1, Cantidad = 2 }
                }
            };

            var result = await controller.PostCompraAccesorio(compraDTO);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var compraObtenida = Assert.IsType<CompraAccesorioDetailDTO>(actionResult.Value);

            var compraEsperada = new CompraAccesorioDetailDTO(
                compraObtenida.Id,                 
                compraObtenida.FechaCompra,     
                20m,                           
                "Calle Falsa 123",     
                new List<CompraAccesorioItemDTO>()
            );

            Assert.Equal(compraEsperada, compraObtenida);
        }

        [Fact]
        public async Task PostCompraAccesorio_AccesorioNoExiste_DevuelveNotFound()
        {
            int expectedStatusCode = 404;
            var controller = new CompraAccesoriosController(_context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Pepe",
                LineasCompra = new List<LineaCompraAccesorioCreateDTO>
                {
                    new LineaCompraAccesorioCreateDTO { AccesorioId = 99, Cantidad = 1 }
                }
            };

            var result = await controller.PostCompraAccesorio(compraDTO);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(expectedStatusCode, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task PostCompraAccesorio_SinStockSuficiente_DevuelveBadRequest()
        {
            var accesorio = new Accesorio(
                1,
                "Pintura Acrílica",
                CategoriaAccesorio.Repuestos,
                "Compatible FDM",
                2,
                10m
            );
            
            _context.Accesorios.Add(accesorio);
            await _context.SaveChangesAsync();

            var controller = new CompraAccesoriosController(_context);

            var compraDTO = new CompraAccesorioCreateDTO
            {
                NombreCliente = "Pepe",
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