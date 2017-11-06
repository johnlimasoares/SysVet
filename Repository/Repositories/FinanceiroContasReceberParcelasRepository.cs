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
        public IEnumerable<FinanceiroContasReceberParcelasDapper> GetAllContasReceberDapper(string tipoPesquisa, DateTime? dataInicial, DateTime? dataFinal, string pesquisaCliente, string tipoPesquisaCliente)
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
                    sql = sql.GetSqlParcelasAbertas(dataInicial, dataFinal);
                    break;
                case "":
                    sql = sql.GetSqlParcelasAbertas(dataInicial, dataFinal);
                    break;
                case "Recebidas":
                    sql = sql.GetSqlParcelasRecebidas(dataInicial, dataFinal);
                    break;
                case "Vencidas":
                    sql = sql.GetSqlParcelasVencidas(dataInicial, dataFinal);
                    break;

            }

            switch (tipoPesquisaCliente)
            {
                case "Codigo":
                    sql = sql.GetSqlParcelasPorCodigoCliente(pesquisaCliente);
                    break;              
                case "Nome":
                    sql = sql.GetSqlParcelasPorNomeCliente(pesquisaCliente);
                    break;
            }

            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    var cliente = db.Query<FinanceiroContasReceberParcelasDapper>(sql);
                    return cliente;
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


    }
}