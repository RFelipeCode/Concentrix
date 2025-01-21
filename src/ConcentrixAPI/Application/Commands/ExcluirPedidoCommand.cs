using ConcentrixAPI.Domain.Entities;
using MediatR;

namespace ConcentrixAPI.Application.Commands
{
    public class ExcluirPedidoCommand : IRequest<bool>
    {
        public int Id { get; }

        public ExcluirPedidoCommand(int id)
        {
            Id = id;
        }
    }
}
