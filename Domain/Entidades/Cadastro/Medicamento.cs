using System.ComponentModel.DataAnnotations;

namespace Domain.Entidades.Cadastro
{
    public class Medicamento{
        [Key]
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Marca { get; set; }

        public string Categoria { get; set; }

        public string Intervalo { get; set; }

        public string Lote { get; set; }
    }
}
