using System.Collections.Generic;
using System.Linq;
using Domain.Entidades.Operacao.Financeiro;

namespace SisVetWeb.Models {
    public class FinanceiroDemonstrativoDeParcelasViewModel {
        public List<FinanceiroContasReceberParcelas> FinanceiroContasReceberParcelasList { get; set; }
        public FinanceiroTipoRecebimento FinanceiroTipoRecebimento { get; set; }
        public string NomeCliente { get; set; }
        public string DescricaoPlanoPagamento { get; set; }
        public decimal ValorTotal { get { return FinanceiroContasReceberParcelasList.Sum(x => x.ValorTotalLiquido); } }
    }
}