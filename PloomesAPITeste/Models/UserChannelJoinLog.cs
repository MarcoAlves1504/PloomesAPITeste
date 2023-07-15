using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PloomesAPITeste.Models {
    /// <summary>
    /// Modelo para registro e log de entrada de usuários em canais de texto no sistema
    /// </summary>
    public class UserChannelJoinLog {
        /// <summary>
        /// ID do usuário no log de entrada dos canais
        /// </summary>
        [Key]
        [ForeignKey("UserAccount")]
        public int UserId { get; set; }

        /// <summary>
        /// ID do canal no log de entrada de usuários em canais de texto
        /// </summary>
        [Key]
        [ForeignKey("TextChannel")]
        public int ChannelId { get; set; }

        /// <summary>
        /// Data e hora em que o usuário entrou nesse canal
        /// </summary>
        public DateTime JoinTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Construtor do registro do log de entrada
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="channelId"></param>
        public UserChannelJoinLog(int userId, int channelId) {
            this.UserId = userId;
            this.ChannelId = channelId;
        }
    }
}
