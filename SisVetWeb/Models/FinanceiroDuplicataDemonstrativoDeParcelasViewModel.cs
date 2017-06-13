using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;

namespace SisVetWeb.Models {
    public class FinanceiroDuplicataDemonstrativoDeParcelasViewModel {
        public List<FinanceiroContasReceberParcelas> FinanceiroContasReceberParcelasList { get; set; }
        public FinanceiroTipoRecebimento FinanceiroTipoRecebimento { get; set; }
    }
}