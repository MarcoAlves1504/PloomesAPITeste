using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PloomesAPITeste.Models {
    /// <summary>
    /// Modelo de uma mensagem de texto no sistema
    /// </summary>
    public class TextMessage {
        /// <summary>
        /// ID da mensagem no banco de dados
        /// </summary>
        [Key]
        public int MessageId { get; set; }

        /// <summary>
        /// ID do usuário remetente da mensagem
        /// </summary>
        [ForeignKey("UserAccount")]
        public int SenderId { get; set; }

        /// <summary>
        /// ID do canal onde a mensagem foi enviada
        /// </summary>
        [ForeignKey("TextChannel")]
        public int ChannelId { get; set; }

        /// <summary>
        /// Conteúdo da mensagem
        /// </summary>
        [Required]
        public required string MessageContent { get; set; }

        /// <summary>
        /// Data e hora de envio da mensagem
        /// </summary>
        public DateTime MessageSentTime { get; set; } = DateTime.Now;
    }
}
