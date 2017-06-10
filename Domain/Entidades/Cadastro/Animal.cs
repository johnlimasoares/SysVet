using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Operacao;
using Domain.Entidades.Operacao.Atendimento;
using Domain.Entidades.Operacao.Vacinacao;

namespace Domain.Entidades.Cadastro {
    public class Animal {
        [Key]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Nome { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Pelagem { get; set; }

        public int RacaId { get; set; }

        public virtual Raca Raca { get; set; }

        public int ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }

        [StringLength(8000, ErrorMessage = "A Descrição não deve ter mais que 8000 caracteres!")]
        public string Observacao { get; set; }

        [Required(ErrorMessage = "Preencha o campo Sexo!")]
        public string Sexo { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataNascimento { get; set; }

        public bool Obito { get; set; }

        public bool Castrado { get; set; }

        public virtual ICollection<Peso> Pesos { get; set; }

        public virtual ICollection<Atendimento> Atendimentos { get; set; }

        public virtual ICollection<Vacinacao> Vacinacoes { get; set; }

        public string GetIdade() {
            return Convert.ToString(DateTime.Now.Year - DataNascimento.Year);
        }
    }

}
