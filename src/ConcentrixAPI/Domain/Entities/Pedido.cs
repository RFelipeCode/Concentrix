using System.ComponentModel.DataAnnotations;

namespace ConcentrixAPI.Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do cliente é obrigatório.")]
        public string NomeCliente { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data do pedido é obrigatória.")]
        public DateTime DataPedido { get; set; }

        [Required(ErrorMessage = "O pedido deve conter itens.")]
        public List<ItemPedido> Itens { get; set; } = new();

        [Range(0, double.MaxValue, ErrorMessage = "Valor total não pode ser negativo.")]
        public decimal ValorTotal => Itens.Sum(i => i.Quantidade * i.ValorUnitario);
    }
}
