using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Operacao.Financeiro;

namespace Domain.Entidades.Operacao.Generic
{
    public class Operacao
    {
        [Key]
        public Int64 Id { get; set; }

        public DateTime Data { get; set; }
    }
}