using System;
using System.Collections.Generic;
using Domain.Entidades.Operacao.Financeiro;

namespace Domain.Entidades.Cadastro.Financeiro {
    public class FinanceiroPlanoDePagamento {
        public Int64 Id { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public int IntervaloDias { get; set; }
    }
}