using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AppForSEII.API.Models;

public class ReservaImpresora
{
    [Key]
    public int Id { get; set; }

    [Required]
    [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [System.ComponentModel.DataAnnotations.Display(Name = "Fecha de reserva")]
    public DateTime FechaReserva { get; set; }

    [System.ComponentModel.DataAnnotations.DataType(System.ComponentModel.DataAnnotations.DataType.Currency)]
    [System.ComponentModel.DataAnnotations.Display(Name = "Precio total")]
    [Precision(10, 2)]
    public decimal PrecioTotal { get; set; }

    [Required]
    [System.ComponentModel.DataAnnotations.Display(Name = "Método de pago")]
    public MetodoPago MetodoPago { get; set; }

    public string ClienteId { get; set; } = string.Empty;

    [Required]
    [ForeignKey(nameof(ClienteId))]
    public Cliente Cliente { get; set; } = null!;

    public IList<LineaReserva> LineasReserva { get; set; } = new List<LineaReserva>();

    public ReservaImpresora() { }

    public ReservaImpresora(int id, DateTime fechaReserva, decimal precioTotal, MetodoPago metodoPago, string clienteId)
    {
        Id = id;
        FechaReserva = fechaReserva;
        PrecioTotal = precioTotal;
        MetodoPago = metodoPago;
        ClienteId = clienteId;
    }
}