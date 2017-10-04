

using System;

namespace SisVetWeb.Models
{
    public class BaixaDeParcelaViewModel
    {
        public int ParcelaId { get; set; }
        public string NumeroDocumento { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public decimal ValorTotalRecebido { get; set; }
    }
}