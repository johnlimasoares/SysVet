using System;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

            float[] colsW = { 13, 11, 7, 7, 7, 7, 5, 5 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(GetNovaCelula("Cliente", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Centro Custo", titulo, Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Dt Emissão", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Dt Recebimento", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Dt Vencimento", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Situação", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vl Total", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Vl Liquidado", titulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var parcelas = new FinanceiroContasReceberParcelasRepository().GetContasReceberReport(DataInicial, DataFinal, Status, PesquisaTexto);

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
            }

            doc.Add(table);
        }
    }
}