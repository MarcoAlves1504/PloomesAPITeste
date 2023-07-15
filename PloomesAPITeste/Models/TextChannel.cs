using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PloomesAPITeste.Models {
    /// <summary>
    /// Modelo de um canal de texto no sistema
    /// </summary>
    public class TextChannel {
        /// <summary>
        /// ID do canal de texto no banco de dados
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do canal de texto visível para os usuários
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Nome do canal não pode ser maior que 30 caracteres")]
        [MinLength(2, ErrorMessage = "Nome do canal precisa ter pelo menos 2 caracteres")]
        public required string ChannelName { get; set; }

        /// <summary>
        /// Descrição opcional do canal de texto visível para os usuários
        /// </summary>
        [MaxLength(100, ErrorMessage = "Descrição do canal não pode ser maior que 100 caracteres")]
        public string? ChannelDescription { get; set; }

        /// <summary>
        /// Data e hora da criação do canal de texto
        /// </summary>
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
