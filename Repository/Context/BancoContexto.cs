using System.Reflection;
using Domain.Entidades;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Cadastro.Contato;
using Domain.Entidades.Cadastro.Localidade;
using Domain.Entidades.Operacao;
using Domain.Entidades.Operacao.Financeiro;

namespace Repository.Context {

    public class BancoContexto : DbContext {
        public BancoContexto()
            : base("ConnDB") {
            this.Configuration.LazyLoadingEnabled = true;
        }

        public DbSet<Animal> Animais { get; set; }
        public DbSet<Especie> Especies { get; set; }
        public DbSet<Raca> Racas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Telefone> Telefones { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<TipoTelefone> TipoTelefones { get; set; }
        public DbSet<UnidadeFederativa> UnidadeFederativas { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<TipoServico> TipoServicos { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Peso> Pesos { get; set; }
        public DbSet<Vacina> Vacinas { get; set; }
        public DbSet<Vacinacao> Vacinacoes { get; set; }
        public DbSet<FinanceiroCentroDeCustoGrupo> FinaceiroCentroDeCustoGrupos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Properties<string>().Configure(x => x.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(x => x.HasMaxLength(100));
        }


        public override int SaveChanges() {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null)) {
                if (entry.State == EntityState.Added) {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }
                if (entry.State == EntityState.Modified) {
                    entry.Property("DataCadastro").IsModified= false;
                }
            }          
            return base.SaveChanges();
        }

    }
}

