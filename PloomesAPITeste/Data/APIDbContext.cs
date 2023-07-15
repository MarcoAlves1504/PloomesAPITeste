using Microsoft.EntityFrameworkCore;
using PloomesAPITeste.Models;

namespace PloomesAPITeste.Data {
    /// <summary>
    /// Representa a conexão com nosso banco de dados
    /// </summary>
    public class APIDbContext : DbContext {
        /// <summary>
        /// Opções de conexão com nosso banco de dados
        /// </summary>
        /// <param name="options"></param>
        public APIDbContext(DbContextOptions<APIDbContext> options) : base(options) { }

        /// <summary>
        /// Configura o nosso banco de dados. Usado para criar triggers para manter a integridade dos dados
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseTriggers(triggerOptions => {
                triggerOptions.AddTrigger<Triggers.CleanMessagesAndJoinLogFromDeletedChannel>();
                triggerOptions.AddTrigger<Triggers.CleanJoinLogFromDeletedUser>();
            });
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// Tabela de usuários no BD
        /// </summary>
        public DbSet<UserAccount> UserAccounts { get; set; }

        /// <summary>
        /// Tabela de canais de texto no BD
        /// </summary>
        public DbSet<TextChannel> TextChannels { get; set; }

        /// <summary>
        /// Tabela de mensagens de texto no BD
        /// </summary>
        public DbSet<TextMessage> TextMessages { get; set; }

        /// <summary>
        /// Tabela com registros do log de entrada de usuários em canais de texto
        /// </summary>
        public DbSet<UserChannelJoinLog> UserChannelJoinsLog { get; set; }

        /// <summary>
        /// Regra para usar dois atributos (ID do usuário e do canal de texto) como chave primária do log de entrada em canais de texto
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserChannelJoinLog>().HasKey(j => new { j.UserId, j.ChannelId });
        }
    }
}
