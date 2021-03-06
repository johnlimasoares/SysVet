using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Reports
{
    public class HeaderFooterManager : PdfPageEventHelper
    {
        public string Titulo { get; set; }
        public string SubTitulo { get; set; }
        public string SubLogo { get; set; }
        public string BasePath { get; set; }
        public bool ImprimirCabecalhoPadrao { get; set; }
        public bool ImprimirRodapePadrao { get; set; }

        public ListaFiltro<Filtro> ListaDeFiltros;

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
            #region Dados do Rodap�
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

                #region P�gina
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
            #region Dados do Cabe�alho
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

                #region P�gina
                micros = new PdfPTable(1);
                cell = new PdfPCell(new Phrase("P�gina: " + (doc.PageNumber).ToString(), font));
                cell.Border = 0;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                micros.AddCell(cell);

                foreach (Filtro filtro in ListaDeFiltros)
                {
                    cell = new PdfPCell(new Phrase(string.Format("{0}:{1}\n", filtro.Nome, filtro.Valor), font));
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    micros.AddCell(cell);

                }

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

    public class Filtro
    {
        public string Nome { get; set; }
        public string Valor { get; set; }
    }

    public class ListaFiltro<T> : System.Collections.IList
    {
        private System.Collections.Generic.IList<object> list;

        public ListaFiltro(){
            list = new List<object>();
        }

        public IEnumerator GetEnumerator(){
            return list.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public object SyncRoot { get; private set; }
        public bool IsSynchronized { get; private set; }

        public int Add(object obj)
        {
            throw new NotImplementedException();
        }

        public void Add(string nome, string valor)
        {
            if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(valor))
            {
                var filtro = new Filtro() { Nome = nome, Valor = valor };
                list.Add(filtro);
            }
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public object this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool IsReadOnly { get; private set; }
        public bool IsFixedSize { get; private set; }
    }
}