using EntityFrameworkCore.Triggered;
using PloomesAPITeste.Data;
using PloomesAPITeste.Models;
using Z.EntityFramework.Plus;

namespace PloomesAPITeste.Triggers {
    /// <summary>
    /// Trigger para remover mensagens de canais excluídos, e limpar os registros do log de entrada em canais excluídos
    /// </summary>
    public class CleanMessagesAndJoinLogFromDeletedChannel : IBeforeSaveTrigger<TextChannel> {
        readonly APIDbContext _db;
        /// <summary>
        /// Inicializa classe com o contexto do banco de dados
        /// </summary>
        /// <param name="db"></param>
        public CleanMessagesAndJoinLogFromDeletedChannel(APIDbContext db) {
            _db = db;
        }

        /// <summary>
        /// Garante a remoção de mensagens em canais excluídos e limpeza do log de entrada nesses canais
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task BeforeSave(ITriggerContext<TextChannel> context, CancellationToken cancellationToken) {
            if (context.ChangeType == ChangeType.Deleted) {
                _db.TextMessages.Where(m => m.ChannelId == context.Entity.Id).Delete();
                _db.UserChannelJoinsLog.Where(j => j.ChannelId == context.Entity.Id).Delete();
            }
            return Task.CompletedTask;
        }
    }
}
