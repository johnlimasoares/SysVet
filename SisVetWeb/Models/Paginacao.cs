using System;

namespace SisVetWeb.Models {
    public class Paginacao {
        public int TotalRegistros { get; set; }

        public int TotalRegistrosPorPagina { get; set; }

        public int PaginaAtual { get; set; }

        public int TotalPaginas { get { return (int)Math.Ceiling((decimal)(TotalRegistros / TotalRegistrosPorPagina)); } }
    }
}