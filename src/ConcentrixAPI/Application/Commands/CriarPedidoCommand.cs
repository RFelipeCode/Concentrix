using ConcentrixAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace ConcentrixAPI.Application.Commands
{
    public class CriarPedidoCommand : IRequest<Pedido>
    {
        public string NomeCliente { get; set; }
        public DateTime DataPedido { get; set; }
        public List<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        public CriarPedidoCommand(string nomeCliente, DateTime dataPedido, List<ItemPedido> itens)
        {
            NomeCliente = nomeCliente;
            DataPedido = DataPedido = dataPedido == DateTime.MinValue ? DateTime.Now : dataPedido; 
            Itens = itens ?? new List<ItemPedido>(); 
        }

        public void DefinirPedidoIdNosItens(int pedidoId)
        {
            foreach (var item in Itens)
            {
                item.PedidoId = pedidoId; 
            }
        }
    }
}
