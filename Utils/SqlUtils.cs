using System;
using System.Globalization;

namespace Utils
{
    public static class SqlUtils
    {
        public static string GetSqlParcelasAbertas(this string sql, DateTime? dataInicial, DateTime? dataFinal)
        {
            sql += " AND CRP.SituacaoParcelaFinanceira = 1 ";
            if (dataInicial != null)
            {
                sql += string.Format(" AND CAST(CRP.DataEmissao AS DATE) >= '{0}'", dataInicial.Value.ToString(GetAmericanFormatDate()));
            }
            if (dataFinal != null)
            {
                sql += string.Format(" AND CAST(CRP.DataEmissao AS DATE) <= '{0}'", dataFinal.Value.ToString(GetAmericanFormatDate()));
            }

            return sql;
        }

        public static string GetSqlParcelasVencidas(this string sql, DateTime? dataInicial, DateTime? dataFinal)
        {
            sql += " AND CRP.SituacaoParcelaFinanceira = 1 ";
            if (dataInicial != null)
            {
                sql += string.Format(" AND CAST(CRP.DataVencimento AS DATE) >= '{0}'", dataInicial.Value.ToString(GetAmericanFormatDate()));
            }

            var dataFinalFiltro = " < CAST(GETDATE() AS DATE)";
            if (dataFinal != null)
                dataFinalFiltro = string.Format(" <= '{0}'", dataFinal.Value.ToString(GetAmericanFormatDate()));

            sql += string.Format(" AND CAST(CRP.DataVencimento AS DATE) {0}", dataFinalFiltro);

            return sql;
        }

        public static string GetSqlParcelasRecebidas(this string sql, DateTime? dataInicial, DateTime? dataFinal)
        {
            sql += " AND CRP.SituacaoParcelaFinanceira = 3 ";
            if (dataInicial != null)
            {
                sql += string.Format(" AND CAST(CRP.DataRecebimento AS DATE) >= '{0}'", dataInicial.Value.ToString(GetAmericanFormatDate()));
            }
            if (dataFinal != null)
            {
                sql += string.Format(" AND CAST(CRP.DataRecebimento AS DATE) <= '{0}'", dataFinal.Value.ToString(GetAmericanFormatDate()));
            }

            return sql;
        }

        public static string GetSqlParcelasPorCodigoCliente(this string sql, string pesquisaCliente)
        {
            int codigoCliente = 0;
            if (string.IsNullOrEmpty(pesquisaCliente) || !Int32.TryParse(pesquisaCliente, out codigoCliente))
            {
                return sql;
            }

            sql += string.Format(" AND C.Id = {0}", codigoCliente);

            return sql;
        }

        public static string GetSqlParcelasPorNomeCliente(this  string sql, string pesquisaCliente)
        {
            if (string.IsNullOrEmpty(pesquisaCliente))
            {
                return sql;
            }

            sql += string.Format(" AND C.Nome like '%{0}%'", pesquisaCliente);
            return sql;
        }

        private static string GetAmericanFormatDate()
        {
            return "yyyy-MM-dd";
        }
    }
}