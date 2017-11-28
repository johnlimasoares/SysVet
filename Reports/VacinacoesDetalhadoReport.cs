using System;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Repository.Repositories;
using Utils;

namespace Reports
{
    public class VacinacoesDetalhadoReport : ReportsBase
    {
        private string StatusVacina { get; set; }
        public string PesquisaTexto { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataFinal { get; set; }

        public VacinacoesDetalhadoReport(DateTime? data, DateTime? datafinal, string statusVacina, string pesquisaTexto)
        {
            this.StatusVacina = statusVacina;
            this.Data = data;
            this.PesquisaTexto = pesquisaTexto;
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            base.MontaCorpoDados();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(6);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 13, 11, 7, 12, 7, 7 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(GetNovaCelula("Cliente", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Animal", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Telefone", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vacina", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Dt Previsão", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Dt Aplicação", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var vacinacoes = new VacinacaoRepository().GetVacinacoesReport(Data,DataFinal, StatusVacina, PesquisaTexto);

            foreach (var vacina in vacinacoes)
            {
                //if (d.cliente.RazaoSocial != clienteOld)
                //{
                //    var cell = GetNovaCelula(d.cliente.RazaoSocial, titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER);
                //    cell.Colspan = 5;
                //    table.AddCell(cell);
                //    clienteOld = d.cliente.RazaoSocial;
                //}
                table.AddCell(GetNovaCelula(vacina.NomeCliente, font, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.NomeAnimal, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.NumeroTelefone.FormatFone(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.DescricaoVacina, font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.DataPrevisao.ToString("dd/MM/yyyy"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.DataVacinacao.IsValidDate(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }
    }
}