using System;
using System.Collections.Generic;
using Dapper;
using Domain.Entidades.Operacao.Financeiro;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Repository.Repositories.Base;
using Utils;

namespace Repository.Repositories
{
    public class FinanceiroContasReceberParcelasRepository : Repository<FinanceiroContasReceberParcelas>
    {
        public IEnumerable<FinanceiroContasReceberParcelasDapper> GetContasReceberDapper(string tipoPesquisa, DateTime? dataInicial, DateTime? dataFinal, string pesquisaCliente, string tipoPesquisaCliente)
        {
            var sql = @"SELECT
                        CRP.Id AS ParcelaId,
                        C.Id AS ClienteId,
                        C.Nome AS ClienteNome,
                        CRP.Parcela,
                        FTR.QuantidadeParcelas AS TotalParcelas,
                        CRP.DataEmissao,
                        CRP.DataRecebimento,
                        CRP.DataCancelamento,
                        CRP.DataVencimento,
                        CRP.SituacaoParcelaFinanceira,
                        CRP.ValorTotalLiquido,
                        CRP.ValorLiquidado,
                        (SELECT SUM(_CRP.ValorTotalLiquido) FROM FinanceiroContasReceberParcelas _CRP) AS ValorTotalEmitidas
                        FROM FinanceiroContasReceberParcelas CRP
                        INNER JOIN FinanceiroTipoRecebimento FTR ON CRP.FinanceiroTipoRecebimentoId = FTR.Id
                        INNER JOIN Cliente C ON FTR.ClienteId = C.Id
                        WHERE 1 = 1 ";


            switch (tipoPesquisa)
            {
                case "Abertas":
                    sql += GetWhereParcelasAbertas(dataInicial, dataFinal);
                    break;
                case "Recebidas":
                    sql += GetWhereParcelasRecebidas(dataInicial, dataFinal);
                    break;
                case "Vencidas":
                    sql += GetWhereParcelasVencidas(dataInicial, dataFinal);
                    break;

            }

            switch (tipoPesquisaCliente)
            {
                case "Codigo":
                    sql += GetWhereParcelasPorCodigoCliente(pesquisaCliente);
                    break;
                case "Nome":
                    sql += GetWhereParcelasPorNomeCliente(pesquisaCliente);
                    break;
            }

            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    return db.Query<FinanceiroContasReceberParcelasDapper>(sql);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    db.Close();
                }

            }
        }

        private string GetWhereParcelasAbertas(DateTime? dataInicial, DateTime? dataFinal)
        {
            string where = string.Empty;
            where += " AND CRP.SituacaoParcelaFinanceira = 1 ";
            if (dataInicial != null)
            {
                where += string.Format(" AND CAST(CRP.DataEmissao AS DATE) >= '{0}'", dataInicial.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }
            if (dataFinal != null)
            {
                where += string.Format(" AND CAST(CRP.DataEmissao AS DATE) <= '{0}'", dataFinal.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }

            return where;
        }

        private string GetWhereParcelasVencidas(DateTime? dataInicial, DateTime? dataFinal)
        {
            string where = string.Empty;
            where += " AND CRP.SituacaoParcelaFinanceira = 1 ";
            if (dataInicial != null)
            {
                where += string.Format(" AND CAST(CRP.DataVencimento AS DATE) >= '{0}'", dataInicial.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }

            var dataFinalFiltro = " < CAST(GETDATE() AS DATE)";
            if (dataFinal != null)
                dataFinalFiltro = string.Format(" <= '{0}'", dataFinal.Value.ToString(SqlUtils.GetAmericanFormatDate()));

            where += string.Format(" AND CAST(CRP.DataVencimento AS DATE) {0}", dataFinalFiltro);

            return where;
        }

        private string GetWhereParcelasRecebidas(DateTime? dataInicial, DateTime? dataFinal)
        {
            string where = string.Empty;
            where += " AND CRP.SituacaoParcelaFinanceira = 3 ";
            if (dataInicial != null)
            {
                where += string.Format(" AND CAST(CRP.DataRecebimento AS DATE) >= '{0}'", dataInicial.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }
            if (dataFinal != null)
            {
                where += string.Format(" AND CAST(CRP.DataRecebimento AS DATE) <= '{0}'", dataFinal.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }

            return where;
        }

        private string GetWhereParcelasPorCodigoCliente(string pesquisaCliente)
        {
            string where = string.Empty;
            int codigoCliente = 0;
            if (string.IsNullOrEmpty(pesquisaCliente) || !Int32.TryParse(pesquisaCliente, out codigoCliente))
            {
                return where;
            }

            where += string.Format(" AND C.Id = {0}", codigoCliente);

            return where;
        }

        private string GetWhereParcelasPorNomeCliente(string pesquisaCliente)
        {
            string where = string.Empty;
            if (string.IsNullOrEmpty(pesquisaCliente))
            {
                return where;
            }

            where += string.Format(" AND C.Nome like '%{0}%'", pesquisaCliente);
            return where;
        }

    }
}