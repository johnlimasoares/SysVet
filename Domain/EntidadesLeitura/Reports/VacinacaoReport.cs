using System;

namespace Domain.EntidadesLeitura.Reports
{
    public class VacinacaoReport
    {
        public DateTime DataPrevisao { get; set; }
        public DateTime DataVacinacao { get; set; }
        public string NomeAnimal { get; set; }
        public string NomeCliente { get; set; }
        public string DescricaoVacina { get; set; }
        public string NumeroTelefone { get; set; }
    }
}