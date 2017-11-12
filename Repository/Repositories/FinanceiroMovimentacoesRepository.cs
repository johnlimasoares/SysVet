using System;
using System.Collections.Generic;
using Dapper;
using Domain.Entidades.Operacao.Financeiro;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Repository.Repositories.Base;
using Utils;
namespace Repository.Repositories
{
    public class FinanceiroMovimentacoesRepository : Repository<FinanceiroMovimentacoes>
    {
        public IEnumerable<FinanceiroMovimentacoesDapper> GetMovimentacoesDapper(DateTime? dataInicial, DateTime? dataFinal, string tipoPesquisa, string pesquisaTexto)
        {
            var sql = @"SELECT 
                          FM.Id
                         ,CC.Descricao AS CentroCusto
                         ,FM.TipoMovimentacao 
                         ,CASE FM.TipoMovimentacao 
		                        WHEN 1 
		                        THEN 'Crédito'
		                        ELSE 'Débito'
                          END AS TipoMovimentacaoDescricao
                         ,FM.DataHora
                         ,CASE FM.TipoMovimentacao 
		                        WHEN 1 
		                        THEN FM.Credito
		                        ELSE FM.Debito
	                        END AS Valor
                        ,FM.Observacao
                         FROM 
                         FinanceiroMovimentacoes FM 
                         INNER JOIN FinanceiroCentroDeCusto CC ON FM.FinanceiroCentroDeCustoId = CC.Id
                         WHERE 1=1 ";

            if (dataInicial != null){
                sql += GetWherePeriodoMaiorIgual(dataInicial);
            }

            if (dataFinal != null){
                sql += GetWherePeriodoMenorIgual(dataFinal);
            }

            switch (tipoPesquisa)
            {
                case "CentroCusto":
                    sql += GetWherePorCentroCusto(pesquisaTexto);
                    break;
                case "Observacao":
                    sql += GetWherePorObservacao(pesquisaTexto);
                    break;
            }

            sql += " ORDER BY FM.Id Desc";

            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    return db.Query<FinanceiroMovimentacoesDapper>(sql);
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

        private string GetWherePeriodoMaiorIgual(DateTime? dataInicial){
            return string.Format(" AND CAST(FM.DataHora AS DATE) >= '{0}'", dataInicial.Value.ToString(SqlUtils.GetAmericanFormatDate()));
        }

        private string GetWherePeriodoMenorIgual(DateTime? dataFinal)
        {
            return string.Format(" AND CAST(FM.DataHora AS DATE) <= '{0}'", dataFinal.Value.ToString(SqlUtils.GetAmericanFormatDate()));
        }

        private string GetWherePorCentroCusto(string pesquisaTexto){
            if (string.IsNullOrEmpty(pesquisaTexto))
                return string.Empty;

            return string.Format(" AND CC.Descricao like '%{0}%'", pesquisaTexto);
        }

        private string GetWherePorObservacao(string pesquisaTexto)
        {
            if (string.IsNullOrEmpty(pesquisaTexto))
                return string.Empty;

            return string.Format(" AND FM.Observacao like '%{0}%'", pesquisaTexto);
        }
 
    }
}
