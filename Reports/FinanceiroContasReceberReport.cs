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
    public class FinanceiroContasReceberReport : ReportsBase
    {
        private string Status { get; set; }
        public string PesquisaTexto { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }

        public FinanceiroContasReceberReport(DateTime? datainicial, DateTime? datafinal, string status, string pesquisaTexto)
        {
            this.Status = status;
            this.DataInicial = datainicial;
            this.DataFinal = datafinal;
            this.PesquisaTexto = pesquisaTexto;
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            base.MontaCorpoDados();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(8);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 14, 10, 7, 7, 7, 7, 5, 6 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(GetNovaCelula("Cliente", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Centro Custo", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Emissão", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Recebimento", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vencimento", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Situação", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vlr Total", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vlr Pago", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var parcelas = new FinanceiroContasReceberParcelasRepository().GetContasReceberReport(DataInicial, DataFinal, Status, PesquisaTexto);
            decimal totalPago = 0;
            decimal totalAberto = 0;
            decimal totalVencidos = 0;
            var dataAtual = DateTime.Now;
            foreach (var parcela in parcelas)
            {
                table.AddCell(GetNovaCelula(parcela.ClienteNome, font, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.CentroCustoDescricao, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.DataEmissao.IsValidDate(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.DataRecebimento.IsValidDate(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.DataVencimento.IsValidDate(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.SituacaoParcelaFinanceira.ToString(), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.ValorTotalLiquido.ToString("N"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(parcela.ValorLiquidado.ToString("N"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));

                totalPago += parcela.ValorLiquidado;

                if (parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto)
                {
                    totalAberto += parcela.ValorTotalLiquido;
                }

                if (parcela.DataVencimento < dataAtual && parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto)
                {
                    totalVencidos += parcela.ValorTotalLiquido;
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
            Phrase phrase;

            doc.Add(new Chunk("\n\n"));
            var totalVencidoParagraph = new Paragraph();
            totalVencidoParagraph.Alignment = Element.ALIGN_RIGHT;
            phrase = new Phrase(string.Format("Total Vencidas: {0}", totalVencidos.ToString("N")), fontTotais);
            totalVencidoParagraph.Add(phrase);
            doc.Add(totalVencidoParagraph);

            var totalAbertoParagraph = new Paragraph();
            totalAbertoParagraph.Alignment = Element.ALIGN_RIGHT;
            phrase = new Phrase(string.Format("Total Aberto: {0}", totalAberto.ToString("N")), fontTotais);
            totalAbertoParagraph.Add(phrase);
            doc.Add(totalAbertoParagraph);

            var totalPagoParagraph = new Paragraph();
            totalPagoParagraph.Alignment = Element.ALIGN_RIGHT;
            phrase = new Phrase(string.Format("Total Liquidado: {0}", totalPago.ToString("N")), fontTotais);
            totalPagoParagraph.Add(phrase);
            doc.Add(totalPagoParagraph);


        }
    }
}