using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class ContaBancaria
    {
        [Key]
        public int Id { get; set; }

        [JsonPropertyName("nome")]
        public string RazaoSocial { get; set; }

        [JsonPropertyName("cnpj")]
        public string CNPJ { get; set; }
        public int Conta { get; set; }
        public int Agencia { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Saldo { get; set; }
        public string? NomeDocumento { get; set; }

        [NotMapped]
        public HttpStatusCode StatusCode { get; set; }
    }
}
