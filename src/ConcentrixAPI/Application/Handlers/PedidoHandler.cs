using ConcentrixAPI.Application.Commands;
using ConcentrixAPI.Application.Queries;
using ConcentrixAPI.Application.Exceptions;
using ConcentrixAPI.Domain.Entities;
using ConcentrixAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ConcentrixAPI.Application.Handlers
{
    public class PedidoHandler :
        IRequestHandler<ObterPedidosQuery, List<Pedido>>,
        IRequestHandler<ObterPedidoByIdQuery, Pedido>,
        IRequestHandler<AtualizarPedidoCommand, bool>,
        IRequestHandler<ExcluirPedidoCommand, bool>,
        IRequestHandler<CriarPedidoCommand, Pedido>
    {
        private readonly PedidoContexto _context;

        public PedidoHandler(PedidoContexto context)
        {
            _context = context;
        }

        // Obter todos os pedidos com filtragem
        public async Task<List<Pedido>> Handle(ObterPedidosQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = _context.Pedidos.Include(p => p.Itens).AsQueryable();

            if (!string.IsNullOrEmpty(request.NomeCliente))
                query = query.Where(p => p.NomeCliente.Contains(request.NomeCliente));

            if (request.DataPedido.HasValue)
                query = query.Where(p => p.DataPedido.Date == request.DataPedido.Value.Date);

            return await query.ToListAsync(cancellationToken);
        }

        // Obter um pedido por ID
        public async Task<Pedido> Handle(ObterPedidoByIdQuery request, CancellationToken cancellationToken)
        {
            var pedido = await _context.Pedidos
                            .Include(p => p.Itens)
                            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (pedido == null)
            {
                throw new NotFoundException("Pedido não encontrado.");
            }

            return pedido;
        }

        // Atualizar um pedido
        public async Task<bool> Handle(AtualizarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _context.Pedidos.Include(p => p.Itens)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (pedido == null)
                return false;

           
            var valida = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), valida, true);
            if (!isValid)
            {
                foreach (var validationResult in valida)
                {
                    throw new ValidationException(validationResult.ErrorMessage);
                }
            }

            pedido.NomeCliente = request.NomeCliente;
            pedido.DataPedido = request.DataPedido;

            pedido.Itens = request.Itens;

            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Excluir um pedido
        public async Task<bool> Handle(ExcluirPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _context.Pedidos
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (pedido == null)
                return false;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        // Criar um novo pedido
        public async Task<Pedido> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
           
            var valida = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, new ValidationContext(request), valida, true);
            if (!isValid)
            {
                foreach (var validationResult in valida)
                {
                    throw new ValidationException(validationResult.ErrorMessage);
                }
            }

            var pedido = new Pedido
            {
                NomeCliente = request.NomeCliente,
                DataPedido = request.DataPedido,
                Itens = request.Itens
            };

            
            foreach (var item in pedido.Itens)
            {
                item.PedidoId = pedido.Id;
            }

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync(cancellationToken);

            return pedido;
        }
    }
}
