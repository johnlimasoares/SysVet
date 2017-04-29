using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Cadastro;

namespace Domain.Entidades.Operacao
{
    public class Atendimento
    {
        public int ID { get; set; }

        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }

        [Display(Name = "Data de Entrada")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataEntrada { get; set; }

        [Display(Name = "Data de Saída")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? DataSaida { get; set; }

        public TimeSpan HoraAtendimento { get; set; }

        public string Situacao { get; set; }

        public double ValorAtendimento { get; set; }

        public double ValorDesconto { get; set; }

        [MaxLength(8000,ErrorMessage = "Máximo de 8000 caractéres")]
        public string Observacao { get; set; }

        public virtual ICollection<Servico> Servicos { get; set; }
    }
}