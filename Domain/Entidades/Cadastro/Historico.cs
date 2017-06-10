using System;
using Domain.Entidades.Operacao;
using Domain.Entidades.Operacao.Atendimento;

namespace Domain.Entidades.Cadastro
{
    public class Historico
    {
        public int Id { get; set; }

        public Atendimento Atendimento { get; set; }

        public string Observacao { get; set; }

        public DateTime DataHora { get; set; }


    }
}