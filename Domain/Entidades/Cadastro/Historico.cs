using System;
using Domain.Entidades.Operacao;

namespace Domain.Entidades.Cadastro
{
    public class Historico
    {
        public int ID { get; set; }

        public Atendimento Atendimento { get; set; }

        public string Observacao { get; set; }

        public DateTime DataHora { get; set; }


    }
}