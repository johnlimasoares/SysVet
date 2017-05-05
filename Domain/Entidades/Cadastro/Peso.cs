using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro
{
    public class Peso
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Peso")]
        public double PesoAnimal { get; set; }

        public int AnimalId{ get; set; }
        public virtual Animal Animal { get; set; }

        [Display(Name = "Data Pesagem")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataCadastro { get; set; }
        [MaxLength(8000)]
        public string Observacao { get; set; }
    }
}