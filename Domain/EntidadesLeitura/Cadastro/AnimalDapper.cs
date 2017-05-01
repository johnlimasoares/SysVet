using System;

namespace Domain.EntidadesLeitura.Cadastro {
    public class AnimalDapper {
        public int ID { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public bool Castrado { get; set; }
        public int ClienteId { get; set; }
        public string ClienteCpfCnpj { get; set; }
        public string ClienteNome { get; set; }
        public string RacaDescricao { get; set; }

    }
}
