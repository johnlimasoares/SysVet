using System;

namespace Domain.EntidadesLeitura.ReportsModel
{
    public class ClientesReportModel
    {
        public int ClienteId { get; set; }
        public string ClienteNome { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime DataCadastro{ get; set; }
        public int TelefoneId { get; set; }
        public string Telefone { get; set; }
        public string TelefoneTipo { get; set; }
    }
}