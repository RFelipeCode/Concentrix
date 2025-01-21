using ConcentrixAPI.Application.Commands;
using ConcentrixAPI.Application.Queries;
using ConcentrixAPI.Domain.Entities;
using ConcentrixAPI.Infrastructure.Data;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ConcentrixAPI.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcentrixAPI.Tests
{
    public class PedidoTests
    {
        private readonly PedidoContexto _context;
        private readonly PedidoHandler _handler;

        public PedidoTests()
        {
            var options = new DbContextOptionsBuilder<PedidoContexto>()
                .UseInMemoryDatabase("PedidoDb")
                .Options;

            _context = new PedidoContexto(options);
            _handler = new PedidoHandler(_context);

            _context.Pedidos.ToList().Clear();
            _context.SaveChanges();
        }

        [Fact]
        public async Task CriarPedido_Valido_DeveAdicionarPedido()
        {

            var pedido = new Pedido
            {
                NomeCliente = "Cliente Teste",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                }
            };

            var command = new CriarPedidoCommand(
                          pedido.NomeCliente,
                          pedido.DataPedido,
                          pedido.Itens
                          );



            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.NomeCliente.Should().Be("Cliente Teste");
            result.Itens.Should().HaveCount(1);
            result.ValorTotal.Should().Be(100);
        }

        [Fact]
        public async Task ObterPedidoPorId_Valido_DeveRetornarPedido()
        {

            var pedido = new Pedido
            {
                NomeCliente = "Cliente Teste",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                }
            };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var query = new ObterPedidoByIdQuery(pedido.Id);


            var result = await _handler.Handle(query, CancellationToken.None);


            result.Should().NotBeNull();
            result.Id.Should().Be(pedido.Id);
            result.NomeCliente.Should().Be("Cliente Teste");
        }

        [Fact]
        public async Task AtualizarPedido_Valido_DeveAtualizarPedido()
        {

            var pedido = new Pedido
            {
                NomeCliente = "Cliente Teste",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                }
            };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var updatedPedido = new Pedido
            {
                Id = pedido.Id,
                NomeCliente = "Cliente Atualizado",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { NomeItem = "Item 1", Quantidade = 3, ValorUnitario = 50 }
                }
            };

            var command = new AtualizarPedidoCommand(
                updatedPedido.Id,
                updatedPedido.NomeCliente,
                updatedPedido.DataPedido,
                updatedPedido.Itens
            );


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeTrue();
            var pedidoAtualizado = await _context.Pedidos.FindAsync(pedido.Id);
            pedidoAtualizado.Itens[0].Quantidade.Should().Be(3);
        }

        [Fact]
        public async Task ExcluirPedido_Valido_DeveExcluirPedido()
        {

            var pedido = new Pedido
            {
                NomeCliente = "Cliente Teste",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
                    new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                }
            };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var command = new ExcluirPedidoCommand(pedido.Id);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeTrue();
            var pedidoExcluido = await _context.Pedidos.FindAsync(pedido.Id);
            pedidoExcluido.Should().BeNull();
        }

        [Fact]
        public async Task ObterPedidos_Valido_Lista_DeveRetornarTodosOsPedidos()
        {
            _context.Pedidos.RemoveRange(_context.Pedidos);
            await _context.SaveChangesAsync();

            var pedidos = new List<Pedido>
            {
                    new Pedido
                    {
                        NomeCliente = "Cliente 1",
                        DataPedido = DateTime.Now,
                        Itens = new List<ItemPedido>
                        {
                            new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                        }
                    },
                    new Pedido
                    {
                        NomeCliente = "Cliente 2",
                        DataPedido = DateTime.Now,
                        Itens = new List<ItemPedido>
                        {
                            new ItemPedido { NomeItem = "Item 2", Quantidade = 1, ValorUnitario = 100 }
                        }
                    },
                    new Pedido
                    {
                        NomeCliente = "Cliente 3",
                        DataPedido = DateTime.Now,
                        Itens = new List<ItemPedido>
                        {
                            new ItemPedido { NomeItem = "Item 3", Quantidade = 3, ValorUnitario = 30 },
                            new ItemPedido { NomeItem = "Item 4", Quantidade = 2, ValorUnitario = 40 }
                        }
                    }
            };

            foreach (var pedido in pedidos)
            {
                await _handler.Handle(new CriarPedidoCommand(
                    pedido.NomeCliente,
                    pedido.DataPedido,
                    pedido.Itens
                ), CancellationToken.None);
            }

            var result = await _handler.Handle(new ObterPedidosQuery(null, null), CancellationToken.None);

            result.Should().HaveCount(3);
            result.Should().Contain(p => p.NomeCliente == "Cliente 1");
            result.Should().Contain(p => p.NomeCliente == "Cliente 2");
            result.Should().Contain(p => p.NomeCliente == "Cliente 3");
            result.Should().Contain(p => p.NomeCliente == "Cliente 3" && p.Itens.Count == 2);
        }

        [Fact]
        public async Task ObterPedidos_Valido_ListaComFiltro_DeveRetornarPedidosFiltrados()
        {
            _context.Pedidos.RemoveRange(_context.Pedidos);
            await _context.SaveChangesAsync();

            var pedidos = new List<Pedido>
            {
                new Pedido
                {
                    NomeCliente = "Cliente 1",
                    DataPedido = DateTime.Now,
                    Itens = new List<ItemPedido>
                    {
                        new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
                    }
                },
                new Pedido
                {
                    NomeCliente = "Cliente 2",
                    DataPedido = DateTime.Now,
                    Itens = new List<ItemPedido>
                    {
                        new ItemPedido { NomeItem = "Item 2", Quantidade = 1, ValorUnitario = 100 }
                    }
                }
            };

            foreach (var pedido in pedidos)
            {
                await _handler.Handle(new CriarPedidoCommand(
                    pedido.NomeCliente,
                    pedido.DataPedido,
                    pedido.Itens
                ), CancellationToken.None);
            }

        
            var query = new ObterPedidosQuery("Cliente 1", null);
            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().HaveCount(1); 
            result.Should().Contain(p => p.NomeCliente == "Cliente 1");
        }


        [Fact]
        public async Task CriarPedido_Invalido_DeveRetornarErroDeValidacao()
        {
            var pedido = new Pedido
            {
                NomeCliente = "",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
                {
            new ItemPedido { NomeItem = "Item 1", Quantidade = -1, ValorUnitario = 50 }  // Quantidade inválida
                }
            };

            var command = new CriarPedidoCommand(
                          pedido.NomeCliente,
                          pedido.DataPedido,
                          pedido.Itens
            );

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().BeNull();
        }


        [Fact]
        public async Task AtualizarPedido_Invalido_DeveRetornarErroDeValidacao()
        {
            var pedido = new Pedido
            {
                NomeCliente = "Cliente Teste",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
        {
            new ItemPedido { NomeItem = "Item 1", Quantidade = 2, ValorUnitario = 50 }
        }
            };
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            var updatedPedido = new Pedido
            {
                Id = pedido.Id,
                NomeCliente = "Cliente Atualizado",
                DataPedido = DateTime.Now,
                Itens = new List<ItemPedido>
        {
            new ItemPedido { NomeItem = "", Quantidade = 3, ValorUnitario = 50 }  // Nome do item inválido
        }
            };

            var command = new AtualizarPedidoCommand(
                updatedPedido.Id,
                updatedPedido.NomeCliente,
                updatedPedido.DataPedido,
                updatedPedido.Itens
            );

            // Teste que deve falhar devido à validação do item
            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();  // Espera que o resultado seja falso ou erro de validação
        }

        [Fact]
        public async Task ExcluirPedido_NaoExistente_DeveRetornarErro()
        {
            var command = new ExcluirPedidoCommand(9999); 

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();  
        }


    }
}
