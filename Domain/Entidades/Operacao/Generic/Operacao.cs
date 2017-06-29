using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Operacao.Generic {
    public class Operacao {
        [Key]
        public Int64 Id { get; set; }

        public DateTime Data { get; set; }

        public Operacao GerarOperacao() {
            this.Data = DateTime.Now;
            return this;
        }
    }
}