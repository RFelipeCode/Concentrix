using ConcentrixAPI.Domain.Entities;
using MediatR;
using System;

namespace ConcentrixAPI.Application.Queries
{
    public class ObterPedidosQuery : IRequest<List<Pedido>>
    {
        public string? NomeCliente { get; }
        public DateTime? DataPedido { get; }

        public ObterPedidosQuery(string? nomeCliente, DateTime? dataPedido)
        {
            NomeCliente = nomeCliente;
            DataPedido = dataPedido;
        }
    }
}
