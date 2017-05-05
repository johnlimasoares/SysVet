using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro {
    public class TipoServico {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Máximo de 100 caractéres!")]
        public string Descricao { get; set; }

        public Double Valor { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}
