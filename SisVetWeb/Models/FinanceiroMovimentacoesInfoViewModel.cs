using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Domain.Enum;

namespace SisVetWeb.Models
{
    public class FinanceiroMovimentacoesInfoViewModel
    {
        public FinanceiroMovimentacoesInfoViewModel(IEnumerable<FinanceiroMovimentacoesDapper> financeiroMovimentacoesesList)
        {
            this._financeiroMovimentacoesesList = financeiroMovimentacoesesList;
            SomarCalcularValores();
        }

        private readonly IEnumerable<FinanceiroMovimentacoesDapper> _financeiroMovimentacoesesList;

        public decimal TotalDebitos { get; private set; }

        public decimal TotalCreditos { get; private set; }

        public decimal Saldo { get; private set; }


        private void SomarCalcularValores()
        {
            TotalDebitos = _financeiroMovimentacoesesList.Where(x => x.TipoMovimentacao == TipoMovimentacao.Debito).Sum(x => x.Valor);
            TotalCreditos = _financeiroMovimentacoesesList.Where(x => x.TipoMovimentacao == TipoMovimentacao.Credito).Sum(x => x.Valor);
            Saldo = TotalCreditos - TotalDebitos;
        }

        public IEnumerable<FinanceiroMovimentacoesDapper> GetListMovimentacoes(){
            return _financeiroMovimentacoesesList;
        } 

    }
}