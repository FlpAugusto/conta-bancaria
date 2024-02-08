namespace WebAPI.DTOs
{
    public class TransacoesTransferenciaDTO
    {
        public int IdContaOrigem { get; set; }
        public int ContaDestino { get; set; }
        public int AgenciaDestino { get; set; }
        public decimal Valor { get; set; }

    }
}
