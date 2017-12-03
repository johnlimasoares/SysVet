using System.Collections.Generic;
using Domain.Entidades.Cadastro;

namespace SisVetWeb.Models {
    public class AnimalViewModel {
        public IEnumerable<Animal> Animais { get; set; }

        public Paginacao Paginacao { get; set; }

    }
}