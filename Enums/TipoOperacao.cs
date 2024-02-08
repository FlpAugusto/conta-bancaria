using System.Text.Json.Serialization;

namespace WebAPI.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoOperacao
    {
        Saque = 1,
        Deposito = 2,
        Transferencia = 3
    }
}
