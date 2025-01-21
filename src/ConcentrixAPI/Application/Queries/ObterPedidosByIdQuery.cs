using ConcentrixAPI.Domain.Entities;
using MediatR;
using System;

namespace ConcentrixAPI.Application.Queries
{
    public class ObterPedidoByIdQuery : IRequest<Pedido>
    {
        public int Id { get; }

        public ObterPedidoByIdQuery(int id)
        {
            Id = id;
        }
    }
}
