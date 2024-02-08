namespace WebAPI.DTOs
{
    public class ContaBancariaAddDTO
    {
        public string CNPJ { get; set; }
        public int Conta { get; set; }
        public int Agencia { get; set; }
        public string DocumentoBase64 { get; set; }
    }
}
