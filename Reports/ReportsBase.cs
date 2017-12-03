using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Reports
{
    public abstract class ReportsBase
    {
        protected Document doc;
        PdfWriter writer;
        MemoryStream output;

        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string SubLogo { get; set; }
        public string BasePath { get; set; }
        public bool ImprimirCabecalhoPadrao { get; set; }
        public bool ImprimirRodapePadrao { get; set; }
        public bool Paisagem { get; set; }
        public ListaFiltro<Filtro> ListaDeFiltros { get; set; }

        public ReportsBase()
        {
            InicializaVariaveis();
        }

        private void InicializaVariaveis()
        {
            ImprimirCabecalhoPadrao = true;
            ImprimirRodapePadrao = true;
            Titulo = string.Empty;
            SubTitulo = string.Empty;
            BasePath = string.Empty;
            Paisagem = false;
            ListaDeFiltros = new ListaFiltro<Filtro>();
        }

        public MemoryStream GetOutput()
        {
            MontaCorpoDados();

            if (output == null || output.Length == 0)
            {
                throw new Exception("Sem dados para exibir.");
            }

            try
            {
                writer.Flush();

                if (writer.PageEmpty)
                {
                    doc.Add(new Paragraph("Nenhum registro para listar."));
                }

                doc.Close();
            }
            catch { }
            finally
            {
                doc = null;
                writer = null;
            }

            return output;
        }

        public virtual void MontaCorpoDados()
        {
            if (!Paisagem)
            {
                doc = new Document(PageSize.A4, 20, 10, 80, 40);
            }
            else
            {
                doc = new Document(PageSize.A4.Rotate(), 20, 10, 80, 80);
            }
            output = new MemoryStream();
            writer = PdfWriter.GetInstance(doc, output);

            doc.AddAuthor("Anjos de Patas");
            doc.AddTitle(Titulo);
            doc.AddSubject(SubTitulo);

            var headerFooter = new HeaderFooterManager();
            headerFooter.ListaDeFiltros = ListaDeFiltros;
            headerFooter.Titulo = Titulo;
            headerFooter.SubTitulo = SubTitulo;
            headerFooter.BasePath = BasePath;
            headerFooter.ImprimirCabecalhoPadrao = ImprimirCabecalhoPadrao;
            headerFooter.ImprimirRodapePadrao = ImprimirRodapePadrao;
            writer.PageEvent = headerFooter;
            doc.Open();
        }

        protected PdfPCell GetNovaCelula(string texto, Font fonte, int alinhamento, float espacamento, int borda, BaseColor corBorda, BaseColor corFundo)
        {
            var cell = new PdfPCell(new Phrase(texto, fonte));
            cell.HorizontalAlignment = alinhamento;
            cell.Padding = espacamento;
            cell.Border = borda;
            cell.BorderColor = corBorda;            
            cell.BackgroundColor = corFundo;
            return cell;
        }

        protected PdfPCell GetNovaCelula(string texto, Font fonte, int alinhamento, float espacamento, int borda, BaseColor corBorda)
        {
            return GetNovaCelula(texto, fonte, alinhamento, espacamento, borda, corBorda, new BaseColor(255, 255, 255));
        }

        protected PdfPCell GetNovaCelula(string texto, Font fonte, int alinhamento = 0, float espacamento = 5, int borda = 0)
        {
            return GetNovaCelula(texto, fonte, alinhamento, espacamento, borda, new BaseColor(0, 0, 0), new BaseColor(255, 255, 255));
        }
    }
}
