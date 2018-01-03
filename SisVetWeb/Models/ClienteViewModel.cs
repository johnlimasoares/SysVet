using System.ComponentModel.DataAnnotations;
using Domain.Entidades.Cadastro.Contato;
using Domain.Entidades.Cadastro.Localidade;
using System;


namespace SisVetWeb.Models
{
    public class ClienteViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string CpfCnpj { get; set; }

        public string RgIe { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string TipoPessoa { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public DateTime DataNascimento { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Email { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Sexo { get; set; }


        public string NumeroTelefone { get; set; }

        public virtual int TipoTelefoneId { get; set; }

        public string NumeroTelefoneContato { get; set; }

        public virtual int TipoTelefoneContatoId { get; set; }

        public string NumeroTelefoneContato2 { get; set; }

        public virtual int TipoTelefoneContato2Id { get; set; }


        [StringLength(50, MinimumLength = 2, ErrorMessage = "A Descrição deve ser entre 2 e 50 caracteres!")]
        public string Rua { get; set; }

        public string Cep { get; set; }

        public string Complemento { get; set; }
        
        public string Numero { get; set; }

        public int? CidadeId { get; set; }

    }

    //public class EnderecoViewModel
    //{
    //    public string Rua { get; set; }

    //    public string CEP { get; set; }

    //    public string Numero { get; set; }

    //    public string Complemento { get; set; } 

    //    public Cidade Cidade { get; set; }

    //}

    //public class TelefoneViewModel
    //{

    //}
}