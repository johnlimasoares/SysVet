﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using Domain.EntidadesLeitura.Cadastro;
using Repository.Repositories.Base;

namespace Repository {
    public class ClienteRepository : Repository<Cliente> {

        public List<ClienteDapper> GetAllClientes(string ordenacao, string pesquisa, string tipoPesquisa) {
            var clientes = from m in GetAllClientesDapper().ToList()
                           select m;

            switch (tipoPesquisa) {
                case "Nome":
                    clientes = clientes.Where(s => s.Nome.ToUpper().Contains(pesquisa.ToUpper()));
                    break;
                case "Cpf":
                    clientes = clientes.Where(s => s.CpfCnpj.Contains(pesquisa));
                    break;
                case "Fone":
                    clientes = from cliente in clientes
                               join fone in new TelefoneRepository().GetAll().ToList() on cliente.ID equals fone.ClienteID
                               where fone.Numero.Contains(pesquisa)
                               select cliente;
                    break;
            }

            switch (ordenacao) {
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

        public IEnumerable<ClienteDapper> GetAllClientesDapper() {
            var sql = @"SELECT 
                            C.ID,
                            C.Nome,
                            C.CpfCnpj,
                            C.DataCadastro,
                            C.DataNascimento,
                            C.Sexo,
                            C.Email
                            FROM Cliente C
                            ORDER BY C.ID";
            using (var db = ctx.Database.Connection) {
                try {
                    db.Open();
                    var cliente = db.Query<ClienteDapper>(sql);
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
