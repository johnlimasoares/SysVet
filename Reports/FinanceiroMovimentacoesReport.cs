using System;
using System.Linq;
using Domain.Enum;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Repository.Repositories;
using Utils;

namespace Reports
{
    public class FinanceiroMovimentacoesReport : ReportsBase
    {
        private string Tipo { get; set; }
        public string PesquisaTexto { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public FinanceiroMovimentacoesReport(DateTime? datainicial, DateTime? datafinal, string tipo, string pesquisaTexto)
        {
            this.Tipo = tipo;
            this.DataInicial = datainicial;
            this.DataFinal = datafinal;
            this.PesquisaTexto = pesquisaTexto;
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            base.MontaCorpoDados();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(4);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor vermelho = new BaseColor(255, 0, 0);
            BaseColor verde = new BaseColor(0, 210, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font fontCredito = FontFactory.GetFont("Verdana", 8, Font.NORMAL, verde);
            Font fontDebito = FontFactory.GetFont("Verdana", 8, Font.NORMAL, vermelho);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 14, 7, 12, 10 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(GetNovaCelula("Centro Custo", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Tipo", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Data", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Crédito/Débito", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var movimentacoes = new FinanceiroMovimentacoesRepository().GetMovimentacoesReport(DataInicial, DataFinal, Tipo, PesquisaTexto);
            decimal totalCredito = 0;
            decimal totalDebito = 0;
            decimal saldo = 0;
            foreach (var movimentacao in movimentacoes)
            {
                var fontCreditoDebito = movimentacao.TipoMovimentacao == TipoMovimentacao.Debito ? fontDebito : fontCredito;
                table.AddCell(GetNovaCelula(movimentacao.CentroCustoDescricao, font, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(movimentacao.TipoMovimentacaoDescricao, fontCreditoDebito, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(movimentacao.DataHora.ToString("G"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(movimentacao.CreditoDebito.ToString("N"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));


                if (movimentacao.TipoMovimentacao == TipoMovimentacao.Credito)
                {
                    totalCredito += movimentacao.CreditoDebito;
                }

                if (movimentacao.TipoMovimentacao == TipoMovimentacao.Debito)
                {
                    totalDebito += movimentacao.CreditoDebito;
                }
            }
            doc.Add(table);

            DottedLineSeparator dottedline = new DottedLineSeparator();
            dottedline.Offset = -2;
            dottedline.Gap = 2f;
            var paragraph = new Paragraph();
            paragraph.Add(dottedline);
            doc.Add(paragraph);

            Font fontTotais = FontFactory.GetFont("Arial", 8, Font.NORMAL, preto);
            Font fontSaldo = FontFactory.GetFont("Arial", 8, Font.BOLD, preto);
            Phrase phrase;

            doc.Add(new Chunk("\n\n"));
            var totalVencidoParagraph = new Paragraph();
            totalVencidoParagraph.Alignment = Element.ALIGN_RIGHT;
            fontTotais.Color = verde;
            phrase = new Phrase(string.Format("Total Crédito: {0}", totalCredito.ToString("N")), fontTotais);
            totalVencidoParagraph.Add(phrase);
            doc.Add(totalVencidoParagraph);

            var totalAbertoParagraph = new Paragraph();
            totalAbertoParagraph.Alignment = Element.ALIGN_RIGHT;
            fontTotais.Color = vermelho;
            phrase = new Phrase(string.Format("Total Débito: {0}", totalDebito.ToString("N")), fontTotais);
            totalAbertoParagraph.Add(phrase);
            doc.Add(totalAbertoParagraph);

            var totalPagoParagraph = new Paragraph();
            totalPagoParagraph.Alignment = Element.ALIGN_RIGHT;
            phrase = new Phrase(string.Format("Saldo: {0}", (totalCredito - totalDebito).ToString("N")), fontSaldo);
            totalPagoParagraph.Add(phrase);
            doc.Add(totalPagoParagraph);


        }
    }
}