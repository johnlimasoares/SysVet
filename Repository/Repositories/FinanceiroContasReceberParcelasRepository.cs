using System;
using System.Collections.Generic;
using Dapper;
using Domain.Entidades.Operacao.Financeiro;
using Domain.Entidades.Operacao.Generic;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Domain.Enum;
using Repository.Context;
using Repository.Repositories.Base;

namespace Repository.Repositories {
    public class FinanceiroContasReceberParcelasRepository : Repository<FinanceiroContasReceberParcelas> {
        public static void SalvarParcelasGeradas(BancoContexto ctx, Operacao operacao,
            List<FinanceiroContasReceberParcelas> financeiroContasReceberParcelasList, double financeiroCentroDeCustoId) {
            foreach (var parcela in financeiroContasReceberParcelasList) {
                ctx.FinanceiroContasReceberParcelas.Add(parcela);
                if (parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado) {
                    FinanceiroMovimentacoesRepository.GerarMovimentacaoEntrada(ctx, operacao, TipoMovimentacao.Credito,
                        financeiroCentroDeCustoId, parcela.ValorTotalLiquido);
                }
            }
        }

        public IEnumerable<FinanceiroContasReceberParcelasDapper> GetAllContasReceberDapper() {
            var sql = @"SELECT
                        C.Id AS ClienteId,
                        C.Nome AS ClienteNome,
                        CRP.Parcela,
                        FTR.QuantidadeParcelas AS TotalParcelas,
                        CRP.DataEmissao,
                        CRP.DataRecebimento,
                        CRP.DataCancelamento,
                        CRP.DataVencimento,
                        CRP.ValorTotalLiquido,
                        CRP.ValorLiquidado
                        FROM FinanceiroContasReceberParcelas CRP
                        INNER JOIN FinanceiroTipoRecebimento FTR ON CRP.FinanceiroTipoRecebimentoId = FTR.Id
                        INNER JOIN Cliente C ON FTR.ClienteId = C.Id
                        WHERE 1 = 1";
            using (var db = ctx.Database.Connection) {
                try {
                    db.Open();
                    var cliente = db.Query<FinanceiroContasReceberParcelasDapper>(sql);
                    return cliente;
                } catch (Exception ex) {
                    throw ex;
                } finally {
                    db.Close();
                }

            }
        }
    }
}