using System;

namespace Domain.EntidadesLeitura.Reports
{
    public class VacinasAnaliseMensal
    {
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string DescricaoVacina { get; set; }
        public int QuantidadeAplicacoes { get; set; }
    }
}