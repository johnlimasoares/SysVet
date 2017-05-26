using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Cadastro.Contato;
using Domain.Entidades.Cadastro.Localidade;

namespace Domain.Entidades.Cadastro
{
   public class Cliente
    {
       [Key] 
       public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o campo Nome!")]
        [MaxLength(100,ErrorMessage = "Máximo {0} caractéres")]
        [MinLength(2,ErrorMessage = "Mínimo {0} caractéres")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha o campo Cpf/Cnpj!")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "Preencha o campo Rg/Ie!")]
        public string RgIe { get; set; }

        [Required(ErrorMessage = "Preencha o campo Tipo Pessoa!")]
        public string TipoPessoa { get; set; }

        [Display(Name = "Data de Nascimento")]
        //[DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataNascimento { get; set; }

        //[Display(Name = "Data de Cadastro")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataCadastro { get; set; }

        
        [EmailAddress(ErrorMessage = "Preencha um E-Mail válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o campo Sexo!")]
        public string Sexo { get; set; }

        public virtual ICollection<Endereco> Enderecos { get; set; }

        public virtual ICollection<Telefone> Telefones { get; set; }

        public virtual ICollection<Animal> Animais { get; set; }
    }
}
