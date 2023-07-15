using EntityFrameworkCore.Triggered;
using PloomesAPITeste.Data;
using PloomesAPITeste.Models;
using Z.EntityFramework.Plus;

namespace PloomesAPITeste.Triggers {
    /// <summary>
    /// Trigger para remover registros do log de entrada sobre usuários excluídos em canais de texto
    /// </summary>
    public class CleanJoinLogFromDeletedUser : IBeforeSaveTrigger<UserAccount> {
        readonly APIDbContext _db;
        /// <summary>
        /// Inicializa classe com o contexto do banco de dados
        /// </summary>
        /// <param name="db"></param>
        public CleanJoinLogFromDeletedUser(APIDbContext db) {
            _db = db;
        }

        /// <summary>
        ///  Garante a limpeza do log de entrada, removendo dados sobre o usuário excluído
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task BeforeSave(ITriggerContext<UserAccount> context, CancellationToken cancellationToken) {
            if (context.ChangeType == ChangeType.Deleted) {
                _db.UserChannelJoinsLog.Where(j => j.UserId == context.Entity.Id).Delete();
            }
            return Task.CompletedTask;
        }
    }
}
