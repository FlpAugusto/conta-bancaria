using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace WebAPI.DTOs
{
    public class ContaBancariaDTO
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set; }
        public int Conta { get; set; }
        public int Agencia { get; set; }
        public decimal Saldo { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
