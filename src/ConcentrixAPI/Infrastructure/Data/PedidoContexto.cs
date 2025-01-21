using ConcentrixAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ConcentrixAPI.Infrastructure.Data
{
    public class PedidoContexto : DbContext
    {
        public PedidoContexto(DbContextOptions<PedidoContexto> options)
            : base(options)
        { }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Pedido>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();  

            modelBuilder.Entity<ItemPedido>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();  

            modelBuilder.Entity<Pedido>()
                .HasMany(p => p.Itens)
                .WithOne(i => i.Pedido)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
