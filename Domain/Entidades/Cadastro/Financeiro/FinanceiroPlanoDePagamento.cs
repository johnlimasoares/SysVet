namespace Domain.Entidades.Cadastro.Financeiro {
    public class FinanceiroPlanoDePagamento {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeParcelas { get; set; }
        public int IntervaloDias { get; set; }

    }
}