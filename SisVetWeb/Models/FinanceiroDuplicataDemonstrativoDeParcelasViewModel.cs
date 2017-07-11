using System.Collections.Generic;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Operacao.Financeiro;

namespace SisVetWeb.Models {
    public class FinanceiroDuplicataDemonstrativoDeParcelasViewModel {
        public List<FinanceiroContasReceberParcelas> FinanceiroContasReceberParcelasList { get; set; }
        public FinanceiroTipoRecebimento FinanceiroTipoRecebimento { get; set; }
        public string NomeCliente { get; set; }
        public string DescricaoPlanoPagamento { get; set; }
    }
}