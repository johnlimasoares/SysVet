using System;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Cadastro;

namespace Domain.Entidades.Operacao {
    public class Vacinacao {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Data Previsão")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? DataPrevisao { get; set; }

        [Display(Name = "Data Vacinação")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime? DataVacinacao { get; set; }

        public int VacinaId { get; set; }
        public virtual Vacina Vacina { get; set; }

        public int AnimalId { get; set; }
        public virtual Animal Animal { get; set; }

    }
}