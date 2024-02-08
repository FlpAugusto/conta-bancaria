using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Transacoes
    {
        [Key]
        public int Id { get; set; }
        public int FK_Conta { get; set; }
        public int? ContaDestino_Id { get; set; }
        public decimal Valor { get; set; }
        public TipoOperacao TipoOperacao { get; set; }
    }
}
