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
    public class ClientesReport : ReportsBase
    {
        public string PesquisaTexto { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public ClientesReport(DateTime? dataInicial, DateTime? dataFinal, string pesquisaTexto)
        {
            this.DataInicial = dataInicial;
            this.DataFinal = dataFinal;
            this.PesquisaTexto = pesquisaTexto;
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            base.MontaCorpoDados();

            PdfPTable table = new PdfPTable(1);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor vermelho = new BaseColor(255, 0, 0);
            BaseColor verde = new BaseColor(0, 210, 0);
            BaseColor fundo = new BaseColor(220, 220, 220);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.ITALIC, preto);

            float[] colsW = { 14 };
            table.SetWidths(colsW);
            //table.HeaderRows = 1;  repetir cabeçalho em todas paginas          
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            var clientes = new ClienteRepository().GetClientesReport(DataInicial, DataFinal, PesquisaTexto);

            table.AddCell(GetNovaCelula(string.Format("Total de clientes: {0}", clientes.GroupBy(x => x.ClienteId).Count()), titulo, Element.ALIGN_RIGHT, 4, 0, preto, fundo));

            int clienteId = 0;
            foreach (var cliente in clientes)
            {
                if (!cliente.ClienteId.Equals(clienteId))
                {
                    var cell = new PdfPCell(new Phrase(string.Format("{0}-{1}", cliente.ClienteId, cliente.ClienteNome), FontFactory.GetFont("Verdana", 10, Font.BOLDITALIC, preto)));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.PaddingTop = 10;
                    cell.PaddingLeft = 2;
                    cell.PaddingRight = 2;
                    cell.PaddingBottom = 0;
                    cell.Border = 0;
                    cell.BorderColor = new BaseColor(0, 0, 0);
                    cell.BackgroundColor = new BaseColor(255, 255, 255);
                    cell.Colspan = 5;
                    table.AddCell(cell);

                    cell = GetNovaCelula(string.Format("Cpf/Cnpj:{0} - Cadastro:{1}", cliente.CpfCnpj, cliente.DataCadastro.IsValidDate()), FontFactory.GetFont("Verdana", 7, Font.BOLDITALIC, preto), Element.ALIGN_LEFT, 2, PdfPCell.BOTTOM_BORDER);
                    cell.Colspan = 5;
                    table.AddCell(cell);
                    clienteId = cliente.ClienteId;
                }

                table.AddCell(GetNovaCelula(string.Format("{0} : {1}", cliente.TelefoneTipo, cliente.Telefone.FormatFone()), font, Element.ALIGN_RIGHT, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }
    }
}