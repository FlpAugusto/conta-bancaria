using System.Reflection.Metadata.Ecma335;

namespace WebAPI.DTOs
{
    public class ExtratoDTO
    {
        public string TipoOperacao { get; set; }
        public string RazaoSocialContaDestino { get; set; }
        public decimal Valor { get; set; }
    }
}
