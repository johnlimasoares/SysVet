using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Domain.Entidades.Cadastro;
using Domain.EntidadesLeitura.Cadastro;
using Domain.EntidadesLeitura.ReportsModel;
using Repository.Repositories.Base;
using Utils;

namespace Repository.Repositories
{
    public class ClienteRepository : Repository<Cliente>
    {

        public List<ClienteDapper> GetAllClientes(string ordenacao, string pesquisa, string tipoPesquisa)
        {
            var clientes = from m in GetAllClientesDapper().ToList()
                           select m;

            if (!string.IsNullOrEmpty(pesquisa))
            {
                switch (tipoPesquisa)
                {
                    case "Nome":
                        clientes = clientes.Where(s => s.Nome.ToUpper().Contains(pesquisa.ToUpper()));
                        break;
                    case "Cpf":
                        clientes = clientes.Where(s => s.CpfCnpj.Contains(pesquisa));
                        break;
                    case "Fone":
                        clientes = from cliente in clientes
                                   join fone in new TelefoneRepository().GetAll().ToList() on cliente.ID equals fone.ClienteId
                                   where fone.Numero.Contains(pesquisa)
                                   select cliente;
                        break;
                }
            }

            switch (ordenacao)
            {
                case "Id":
                    clientes = clientes.OrderBy(x => x.ID);
                    break;
                case "Id_Desc":
                    clientes = clientes.OrderByDescending(x => x.ID);
                    break;
                case "Nome":
                    clientes = clientes.OrderBy(x => x.Nome);
                    break;
                case "Nome_Desc":
                    clientes = clientes.OrderByDescending(x => x.Nome);
                    break;
                default:
                    clientes = clientes.OrderBy(x => x.ID);
                    break;

            }

            return clientes.ToList();
        }

        public IEnumerable<ClienteDapper> GetAllClientesDapper()
        {
            var sql = @"SELECT 
                            C.ID,
                            C.Nome,
                            C.CpfCnpj,
                            C.DataCadastro,
                            C.DataNascimento,
                            C.Sexo
                            FROM Cliente C
                            ORDER BY C.ID";
            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    var cliente = db.Query<ClienteDapper>(sql);
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


        public Cliente Excluir(int id)
        {
            var cliente = this.Find(id);
            if (cliente != null)
            {
                this.Excluir(c => c == cliente);
                this.SalvarTodos();
                return cliente;
            }

            throw new Exception("Cliente inexistente no banco de dados");
        }

        public string GetNomeCliente(double clienteId)
        {
            var cliente = this.GetAll()
                        .Where(x => x.Id == clienteId)
                        .Select(p => new { p.Nome })
                        .FirstOrDefault();

            return cliente == null ? string.Empty : cliente.Nome;
        }

        public IEnumerable<ClientesReportModel> GetClientesReport(DateTime? dataInicial, DateTime? dataFinal, string pesquisaTexto)
        {
            var sql = @"SELECT 
                        C.ID AS ClienteId
                        ,C.Nome AS ClienteNome
                        ,C.CpfCnpj
                        ,C.DataCadastro
                        ,T.ID AS TelefoneId
                        ,T.Numero AS Telefone
                        ,TT.Descricao AS  TelefoneTipo
                        FROM Cliente C
                        LEFT JOIN Telefone T ON T.ClienteID = C.ID
                        LEFT JOIN TipoTelefone TT ON T.TipoTelefoneId = TT.ID
                        WHERE 1=1 ";

            if (!string.IsNullOrEmpty(pesquisaTexto))
            {
                sql += string.Format(" AND C.Nome like '%{0}%' OR C.ID like '%{0}%'", pesquisaTexto);
            }

            if (dataInicial != null)
            {
                sql += string.Format(" AND CAST(C.DataCadastro AS DATE) >= '{0}'", dataInicial.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }

            if (dataFinal != null)
            {
                sql += string.Format(" AND CAST(C.DataCadastro AS DATE) <= '{0}'", dataFinal.Value.ToString(SqlUtils.GetAmericanFormatDate()));
            }

            using (var db = ctx.Database.Connection)
            {
                try
                {
                    db.Open();
                    return db.Query<ClientesReportModel>(sql);
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
