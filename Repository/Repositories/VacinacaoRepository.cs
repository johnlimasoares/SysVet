using System;
using System.Collections;
using System.Data;
using Dapper;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Operacao;
using Domain.WidGet;
using Repository.Context;
using Repository.Repositories.Base;
using System.Linq;
namespace Repository.Repositories {
    public class VacinacaoRepository : Repository<Vacinacao> {

        public IEnumerable GetAllVacinacoesAVencerDapper() {
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
            using (var db = ctx.Database.Connection) {
                try {
                    db.Open();
                    var vacinacao = db.Query<VacinacaoAVencerWidGet>(sql);
                    return vacinacao;
                } catch (Exception ex) {
                    throw ex;
                } finally {
                    db.Close();
                }

            }



        }

        public IEnumerable GetAllVacinacoesVencidasDapper() {
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
            using (var db = ctx.Database.Connection) {
                try {
                    db.Open();
                    var vacinacao = db.Query<VacinacaoVencidasWidGet>(sql);
                    return vacinacao;
                } catch (Exception ex) {
                    throw ex;
                } finally {
                    db.Close();
                }

            }
        }
    }
}
