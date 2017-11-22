using System;

namespace Domain.EntidadesLeitura.Cadastro {
    public class ClienteDapper {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Sexo { get; set; }
    }
}
