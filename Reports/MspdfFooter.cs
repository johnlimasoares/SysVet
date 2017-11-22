using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Reports
{
    public class MspdfFooter : PdfPageEventHelper
    {
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string SubLogo { get; set; }
        public string BasePath { get; set; }
        public bool ImprimirCabecalhoPadrao { get; set; }
        public bool ImprimirRodapePadrao { get; set; }

        public override void OnOpenDocument(PdfWriter writer, Document doc)
        {
            base.OnOpenDocument(writer, doc);
        }

        public override void OnStartPage(PdfWriter writer, Document doc)
        {
            base.OnStartPage(writer, doc);
            ImprimeCabecalho(writer, doc);
        }

        public override void OnEndPage(PdfWriter writer, Document doc)
        {
            base.OnEndPage(writer, doc);
            ImprimeRodape(writer, doc);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }




        private void ImprimeRodape(PdfWriter writer, Document doc)
        {
            #region Dados do Rodapé
            if (ImprimirRodapePadrao)
            {
                BaseColor preto = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
                Font negrito = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);
                float[] sizes = new float[] { 1.0f, 3.5f, 1f };

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = doc.PageSize.Width - (doc.LeftMargin + doc.RightMargin);
                table.SetWidths(sizes);

                #region Coluna TNE
                Image foot = Image.GetInstance(BasePath + @"\Content\tne_mascote.png");
                foot.ScalePercent(60);

                PdfPCell cell = new PdfPCell(foot);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingLeft = 10f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);

                PdfPTable micros = new PdfPTable(1);
                cell = new PdfPCell(new Phrase("TNE", negrito));
                cell.Border = 0;
                micros.AddCell(cell);
                cell = new PdfPCell(new Phrase("Treta never ends", font));
                cell.Border = 0;
                micros.AddCell(cell);
                cell = new PdfPCell(new Phrase("www.tretaneverends.com.br", font));
                cell.Border = 0;
                micros.AddCell(cell);

                cell = new PdfPCell(micros);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);
                #endregion

                #region Página
                micros = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(DateTime.Today.ToString("dd/MM/yyyy"), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micros.AddCell(cell);
                cell = new PdfPCell(new Phrase(DateTime.Now.ToString("HH:mm:ss"), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micros.AddCell(cell);

                cell = new PdfPCell(micros);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);
                #endregion

                table.WriteSelectedRows(0, -1, doc.LeftMargin, 70, writer.DirectContent);
            }
            #endregion
        }

        private void ImprimeCabecalho(PdfWriter writer, Document doc)
        {
            #region Dados do Cabeçalho
            if (ImprimirCabecalhoPadrao)
            {
                BaseColor preto = new BaseColor(0, 0, 0);
                Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
                Font titulo = FontFactory.GetFont("Verdana", 12, Font.BOLD, preto);
                float[] sizes = new float[] { 1f, 3f, 1f };

                PdfPTable table = new PdfPTable(3);
                table.TotalWidth = doc.PageSize.Width - (doc.LeftMargin + doc.RightMargin);
                table.SetWidths(sizes);

                #region Logo Empresa
                Image foot;
                if (File.Exists(BasePath + @"\PublicResources\" + SubLogo))
                {
                    foot = Image.GetInstance(BasePath + @"\PublicResources\" + SubLogo);
                }
                else
                {
                    foot = Image.GetInstance(BasePath + @"\Content\logo_anjos.png");
                }
                foot.ScalePercent(60);

                PdfPCell cell = new PdfPCell(foot);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                cell.PaddingBottom = 10f;
                table.AddCell(cell);

                PdfPTable micros = new PdfPTable(1);
                cell = new PdfPCell(new Phrase(SubTitulo, font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                micros.AddCell(cell);
                cell = new PdfPCell(new Phrase(Titulo, titulo));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                micros.AddCell(cell);

                cell = new PdfPCell(micros);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);
                #endregion

                #region Página
                micros = new PdfPTable(1);
                cell = new PdfPCell(new Phrase("Página: " + (doc.PageNumber).ToString(), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micros.AddCell(cell);

                cell = new PdfPCell(micros);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = 0;
                cell.BorderWidthTop = 1.5f;
                cell.BorderWidthBottom = 1.5f;
                cell.PaddingTop = 10f;
                table.AddCell(cell);
                #endregion

                table.WriteSelectedRows(0, -1, doc.LeftMargin, (doc.PageSize.Height - 10), writer.DirectContent);
            }
            #endregion
        }
    }
}