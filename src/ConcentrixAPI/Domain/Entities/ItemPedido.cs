using System.ComponentModel.DataAnnotations;

namespace ConcentrixAPI.Domain.Entities
{
    public class ItemPedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do item é obrigatório.")]
        public string NomeItem { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantidade não pode ser negativa.")]
        public int Quantidade { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Valor unitário deve ser maior que zero.")]
        public decimal ValorUnitario { get; set; }

    
        public int PedidoId { get; set; }  
        public Pedido Pedido { get; set; } 
    }
}
