using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Entidades.Cadastro;
using Domain.EntidadesLeitura.Cadastro;
using Repository.Repositories.Base;
namespace Repository.Repositories {
    public class AnimalRepository : Repository<Animal> {

        public List<AnimalDapper> GetAllAnimais(string ordenacao, string pesquisa, string tipoPesquisa) {

            var animais = from m in GetAllAnimaisDapper().ToList()
                          select m;

            switch (tipoPesquisa) {
                case "Id":
                    animais = animais.Where(s => s.ID.ToString().ToUpper().Contains(pesquisa.ToString().ToUpper()));
                    break;
                case "Nome":
                    animais = animais.Where(s => s.Nome.ToUpper().Contains(pesquisa.ToUpper()));
                    break;
                case "Cpf":
                    animais = animais.Where(s => s.ClienteCpfCnpj.Contains(pesquisa));
                    break;
                case "Fone":
                    animais = from animal in animais
                              join cliente in new ClienteRepository().GetAll().ToList() on animal.ClienteId equals cliente.Id
                              join fone in new TelefoneRepository().GetAll().ToList() on cliente.Id equals fone.ClienteId
                              where fone.Numero.Contains(pesquisa)
                              select animal;

                    break;
                default:
                    //animais = animais.Where(s => s.Nome.ToUpper().Contains(pesquisa.ToUpper()));
                    break;
            }

            switch (ordenacao) {
                case "Id":
                    animais = animais.OrderBy(x => x.ID);
                    break;
                case "Id_Desc":
                    animais = animais.OrderByDescending(x => x.ID);
                    break;
                case "Nome":
                    animais = animais.OrderBy(x => x.Nome);
                    break;
                case "Nome_Desc":
                    animais = animais.OrderByDescending(x => x.Nome);
                    break;
                default:
                    animais = animais.OrderBy(x => x.ID);
                    break;

            }
            return animais.ToList();
        }

        public IEnumerable<AnimalDapper> GetAllAnimaisDapper() {
            var sql = @"SELECT 
                            A.ID,
                            A.Nome,
                            A.DataNascimento,
                            A.Sexo,
                            A.Castrado,
                            C.CpfCnpj AS ClienteCpfCnpj,
                            C.Nome AS ClienteNome,
                            C.ID AS ClienteId,
                            R.Descricao AS RacaDescricao
                            FROM Animal A 
                            INNER JOIN Cliente C ON A.ClienteId = C.ID
                            INNER JOIN Raca R ON A.RacaId = R.ID
                            ORDER BY A.ID";
            using (var db = ctx.Database.Connection) {
                try {
                    db.Open();
                    var animal = db.Query<AnimalDapper>(sql);
                    return animal;
                } catch (Exception ex) {
                    throw ex;
                } finally {
                    db.Close();
                }

            }

        }
    }
}
