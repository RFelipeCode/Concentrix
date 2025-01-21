using ConcentrixAPI.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;

namespace ConcentrixAPI.Application.Commands
{
        public class AtualizarPedidoCommand : IRequest<bool>
        {
            public int Id { get; }
            public string NomeCliente { get; }
            public DateTime DataPedido { get; }
            public List<ItemPedido> Itens { get; }

            public AtualizarPedidoCommand(int id, string nomeCliente, DateTime dataPedido, List<ItemPedido> itens)
            {
                Id = id;
                NomeCliente = nomeCliente;
                DataPedido = dataPedido;
                Itens = itens ?? new List<ItemPedido>();  
            }
        }
}
