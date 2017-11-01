

using System;
using System.ComponentModel.DataAnnotations;

namespace SisVetWeb.Models
{
    public class InformacaoDeParcelaViewModel
    {
        public int ParcelaId { get; set; }
        public string NumeroDocumento { get; set; }
        public string NomeCliente { get; set; }
        public DateTime DataVencimento { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataRecebimento { get; set; }
        public decimal ValorTotalLiquido { get; set; }
        public decimal ValorTotalRecebido { get; set; }
        public string Observacoes { get; set; }
    }
}