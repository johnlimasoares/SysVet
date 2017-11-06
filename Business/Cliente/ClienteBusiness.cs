using System.Linq;
using Domain.Entidades.Cadastro;
using Repository.Context;

namespace Business.Cliente
    
{
    public static class ClienteBusiness
    {
        public static string GetNomeCliente(BancoContexto ctx, int clienteId){
            return ctx.Clientes.Where(x => x.Id == clienteId).Select(p => new{p.Nome}).First().ToString();
        }
    }
}