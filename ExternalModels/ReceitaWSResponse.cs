using System.Text.Json.Serialization;

namespace WebAPI.ExternalModels
{
    public class ReceitaWSResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("ultima_atualizacao")]
        public DateTime UltimaAtualizacao { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("porte")]
        public string Porte { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; }

        [JsonPropertyName("fantasia")]
        public string Fantasia { get; set; }

        [JsonPropertyName("abertura")]
        public string Abertura { get; set; }

        [JsonPropertyName("atividade_principal")]
        public List<AtividadePrimaria> AtividadePrincipal { get; set; }

        [JsonPropertyName("atividades_secundarias")]
        public List<AtividadesSecundaria> AtividadesSecundarias { get; set; }

        [JsonPropertyName("natureza_juridica")]
        public string NaturezaJuridica { get; set; }

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("numero")]
        public string Numero { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("cep")]
        public string Cep { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("municipio")]
        public string Municipio { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("telefone")]
        public string Telefone { get; set; }

        [JsonPropertyName("efr")]
        public string Efr { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("data_situacao")]
        public string DataSituacao { get; set; }

        [JsonPropertyName("motivo_situacao")]
        public string MotivoSituacao { get; set; }

        [JsonPropertyName("situacao_especial")]
        public string SituacaoEspecial { get; set; }

        [JsonPropertyName("data_situacao_especial")]
        public string DataSituacaoEspecial { get; set; }

        [JsonPropertyName("capital_social")]
        public string CapitalSocial { get; set; }

        [JsonPropertyName("qsa")]
        public List<Socios> Qsa { get; set; }

        public class AtividadePrimaria
        {
            [JsonPropertyName("code")]
            public string Codigo { get; set; }

            [JsonPropertyName("text")]
            public string Descricao { get; set; }
        }

        public class AtividadesSecundaria
        {
            [JsonPropertyName("code")]
            public string Codigo { get; set; }

            [JsonPropertyName("text")]
            public string Descricao { get; set; }
        }

        public class Socios
        {
            [JsonPropertyName("nome")]
            public string NomeSocio { get; set; }

            [JsonPropertyName("qual")]
            public string Paeticipacao { get; set; }

            [JsonPropertyName("pais_origem")]
            public string PaisOrigem { get; set; }

            [JsonPropertyName("nome_rep_legal")]
            public string NomeRepLegal { get; set; }

            [JsonPropertyName("qual_rep_legal")]
            public string QualRepLegal { get; set; }
        }
    }
}
