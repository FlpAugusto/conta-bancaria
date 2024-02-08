using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.DTOs
{
    public class TransacoesDTO
    {
        public int Id { get; set; }
        public int FK_Conta { get; set; }
        public int? ContaDestino_Id { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }
    }
}
