using System.Linq;
using Domain.Entidades.Cadastro.Contato;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Cadastro.Localidade;
using Repository.Context;

namespace Business.Cliente
{
    public static class ClienteBusiness
    {
        public static string GetNomeCliente(BancoContexto ctx, int clienteId)
        {
            return ctx.Clientes.Where(x => x.Id == clienteId).Select(p => new { p.Nome }).First().ToString();
        }

        public static int Save(Domain.Entidades.Cadastro.Cliente cliente, Endereco endereco, Telefone telefone)
        {
            using (var ctx = new BancoContexto())
            {
                ctx.Clientes.Add(cliente);
                if (endereco != null)
                {
                    endereco.Cliente = cliente;
                    ctx.Enderecos.Add(endereco);
                }

                if (telefone != null)
                {
                    telefone.Cliente = cliente;
                    ctx.Telefones.Add(telefone);
                }
                ctx.SaveChanges();
            }

            return cliente.Id;
        }

        public static bool HaRegistrosFinanceiroDoCliente(int clienteId)
        {
            using (var ctx = new BancoContexto())
            {
                var registroFinanceiro = ctx.FinanceiroTipoRecebimentos.FirstOrDefault(c => c.ClienteId == clienteId);
                return registroFinanceiro != null;
            }
        }
    }
}