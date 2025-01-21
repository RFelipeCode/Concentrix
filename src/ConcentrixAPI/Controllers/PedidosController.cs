using ConcentrixAPI.Application.Commands;
using ConcentrixAPI.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ConcentrixAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Método para listar todos os pedidos 
        [HttpGet]
        public async Task<IActionResult> GetPedidos([FromQuery] string? nomeCliente, [FromQuery] DateTime? dataPedido)
        {
            var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            var timeoutCancellationToken = cts.Token;

            try
            {
                var query = new ObterPedidosQuery(nomeCliente, dataPedido);

                var pedidos = await _mediator.Send(query, timeoutCancellationToken);

                return Ok(pedidos);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(408, "A solicitação levou mais de 5 minutos e foi cancelada.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }

        // Método para obter um pedido por ID 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var query = new ObterPedidoByIdQuery(id);
            var pedido = await _mediator.Send(query);
            if (pedido == null)
                return NotFound();
            return Ok(pedido);
        }

        // Método para criar um pedido
        [HttpPost]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var pedido = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetPedido), new { id = pedido.Id }, pedido);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }


        // Método para atualizar um pedido 
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarPedido(int id, [FromBody] AtualizarPedidoCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != command.Id)
                return BadRequest("ID do pedido não corresponde");

            try
            {
                var result = await _mediator.Send(command);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarPedido(int id)
        {
            try
            {
                var command = new ExcluirPedidoCommand(id);
                var result = await _mediator.Send(command);

                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro inesperado: {ex.Message}");
            }
        }
    }
}
