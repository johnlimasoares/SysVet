using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Entidades.Operacao.Vacinacao;
using Domain.EntidadesLeitura.Reports;
using Domain.EntidadesLeitura.WidGet;
using Domain.WidGet;
using Repository.Repositories.Base;
using Utils;

namespace Repository.Repositories
{
    public class VacinacaoRepository : Repository<Vacinacao>
    {

        public IEnumerable GetVacinacoesAVencerWidget()
        {
            var sql = @"SELECT 
                        V.DataPrevisao,
                        A.Nome AS NomeAnimal,
                        C.Nome AS NomeCliente,
                        C.ID,
                        VA.Descricao AS DescricaoVacina,
                        (SELECT TOP 1 T.Numero FROM Telefone T WHERE T.ClienteID = C.ID)AS NumeroTelefone
                        FROM 
                        Vacinacao V
	                        INNER JOIN Animal A ON V.AnimalId = A.ID
	                        INNER JOIN Cliente C ON A.ClienteId = C.ID
	                        INNER JOIN Vacina VA ON V.VacinaId = VA.ID
                        WHERE 
                        V.DataPrevisao >= CAST(GETDATE() AS DATE) AND
                        V.DataPrevisao <= GETDATE() + 10 AND 
                        V.DataVacinacao IS NULL
                        ORDER BY V.DataPrevisao ";
            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    var vacinacao = db.Query<VacinacaoAVencerWidGet>(sql);
                    return vacinacao;
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

        public IEnumerable GetVacinacoesVencidasWidget()
        {
            var sql = @"SELECT
                        V.DataPrevisao,
                        A.Nome AS NomeAnimal,
                        C.Nome AS NomeCliente,
                        C.ID,
                        VA.Descricao AS DescricaoVacina,
                        (SELECT TOP 1 T.Numero FROM Telefone T WHERE T.ClienteID = C.ID)AS NumeroTelefone
                        FROM 
                        Vacinacao V
	                        INNER JOIN Animal A ON V.AnimalId = A.ID
	                        INNER JOIN Cliente C ON A.ClienteId = C.ID
	                        INNER JOIN Vacina VA ON V.VacinaId = VA.ID
                        WHERE 
                        V.DataPrevisao < CAST(GETDATE() AS DATE) AND
                        V.DataPrevisao >= GETDATE() -30 AND
                        V.DataVacinacao IS NULL
                        ORDER BY V.DataPrevisao DESC ";
            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    var vacinacao = db.Query<VacinacaoVencidasWidGet>(sql);
                    return vacinacao;
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

        public IList<VacinacaoReport> GetVacinacoesReport(DateTime? data, DateTime? datafinal, string statusVacina, string pesquisaTexto)
        {
            var sql = @"SELECT 
                        V.DataPrevisao,
                        V.DataVacinacao,
                        A.Nome AS NomeAnimal,
                        C.Nome AS NomeCliente,
                        C.ID,
                        VA.Descricao AS DescricaoVacina,
                        (SELECT TOP 1 T.Numero FROM Telefone T WHERE T.ClienteID = C.ID)AS NumeroTelefone
                        FROM 
                        Vacinacao V
	                        INNER JOIN Animal A ON V.AnimalId = A.ID
	                        INNER JOIN Cliente C ON A.ClienteId = C.ID
	                        INNER JOIN Vacina VA ON V.VacinaId = VA.ID
                        WHERE 1=1 ";

            if (!string.IsNullOrEmpty(pesquisaTexto))
            {
                sql += string.Format(" AND (A.Nome like '%{0}%' OR C.Nome like '%{0}%')", pesquisaTexto);
            }

            switch (statusVacina)
            {
                case "Vencidas":
                    sql += " AND V.DataPrevisao < CAST(GETDATE() AS DATE) AND V.DataVacinacao IS NULL";
                    if (data != null)
                    {
                        sql += string.Format(" AND V.DataPrevisao >= '{0}'", data.Value.ToString(SqlUtils.GetAmericanFormatDate()));
                    }
                    sql += " ORDER BY V.DataPrevisao";
                    break;
                case "Vencer":
                    sql += " AND V.DataPrevisao >= CAST(GETDATE() AS DATE) AND V.DataVacinacao IS NULL";
                    if (data != null)
                    {
                        sql += string.Format(" AND V.DataPrevisao <= '{0}'", data.Value.ToString(SqlUtils.GetAmericanFormatDate()));
                    }

                    sql += " ORDER BY V.DataPrevisao ";
                    break;
                case "Aplicadas":
                    sql += " AND V.DataVacinacao IS NOT NULL";
                    if (data != null)
                    {
                        sql += string.Format(" AND V.DataVacinacao >= '{0}'", data.Value.ToString(SqlUtils.GetAmericanFormatDate()));
                    }

                    if (datafinal != null)
                    {
                        sql += string.Format(" AND V.DataVacinacao <= '{0}'", data.Value.ToString(SqlUtils.GetAmericanFormatDate()));
                    }

                    sql += " ORDER BY V.DataVacinacao ";
                    break;
            }

            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    var vacinacao = db.Query<VacinacaoReport>(sql);
                    return vacinacao.ToList();
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
