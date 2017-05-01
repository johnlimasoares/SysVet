using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.Entidades;
using Domain.Entidades.Cadastro;

namespace SisVetWeb.Models {
    public class AnimalViewModel {
        public IEnumerable<Animal> Animais { get; set; }

        public Paginacao Paginacao { get; set; }

    }
}