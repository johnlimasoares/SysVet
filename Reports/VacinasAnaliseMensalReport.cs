using System;
using System.Security.Cryptography;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Repository.Repositories;
using Utils;

namespace Reports
{
    public class VacinasAnaliseMensalReport : ReportsBase
    {
        private string DescricaoVacina { get; set; }
        private DateTime? DataInicial { get; set; }
        private DateTime? DataFinal { get; set; }

        public VacinasAnaliseMensalReport(DateTime? dataInicial, DateTime? datafinal, string descricaoVacina)
        {
            this.DataInicial = dataInicial;
            this.DataFinal = datafinal;
            this.DescricaoVacina = descricaoVacina;
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            base.MontaCorpoDados();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(2);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Arial", 8, Font.NORMAL, preto);
            Font fonteTitulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = {12, 7 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            //table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            //table.DefaultCell.BorderColor = preto;
            //table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            //table.DefaultCell.Padding = 10;

            table.AddCell(GetNovaCelula("Vacina", fonteTitulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(GetNovaCelula("Quantidade aplicações", fonteTitulo, Element.ALIGN_CENTER, 2, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var vacinacoes = new VacinacaoRepository().GetVacinacoesMensalReport(DataInicial, DataFinal, DescricaoVacina);
            string ano = string.Empty;
            string mes = string.Empty;
            foreach (var vacina in vacinacoes)
            {
                if (!vacina.Ano.Equals(ano) || !vacina.Mes.Equals(mes))
                {
                    var cell = GetNovaCelula(string.Format("{0}/{1}", vacina.Mes, vacina.Ano),FontFactory.GetFont("Verdana", 10, Font.BOLDITALIC , preto), Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER);
                    cell.Colspan = 5;                    
                    table.AddCell(cell);
                    ano = vacina.Ano;
                    mes = vacina.Mes;
                }
                table.AddCell(GetNovaCelula(vacina.DescricaoVacina, font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(GetNovaCelula(vacina.QuantidadeAplicacoes.ToString("D"), font, Element.ALIGN_CENTER, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }
    }
}