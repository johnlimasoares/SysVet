using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Operacao.Financeiro;

namespace Domain.Entidades.Cadastro.Financeiro {
    public class FinanceiroCentroDeCusto {
        [Key]
        public Int64 Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 20 caracteres!")]
        public string Descricao { get; set; }

        public int FinanceiroCentroDeCustoGrupoId { get; set; }

        public virtual FinanceiroCentroDeCustoGrupo FinanceiroCentroDeCustoGrupo { get; set; }

    }
}