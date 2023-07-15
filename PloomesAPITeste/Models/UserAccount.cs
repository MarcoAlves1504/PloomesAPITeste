using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PloomesAPITeste.Models
{
    /// <summary>
    /// Modelo de uma conta de usuário no sistema
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// ID da conta do usuário no banco de dados
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome de usuário
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "Nome de usuário não pode ser maior que 20 caracteres")]
        [MinLength(3, ErrorMessage = "Nome de usuário precisa ter pelo menos 3 caracteres")]
        public required string Username { get; set; }

        /// <summary>
        /// Uma breve apresentação escrita pelo usuário
        /// </summary>
        [MaxLength(50, ErrorMessage = "Apresentação do usuário não pode ser maior que 50 caracteres")]
        [DisplayName("About me")]
        public string? AboutMe { get; set; }

        /// <summary>
        /// Data e hora da criação da conta
        /// </summary>
        public DateTime AccountCreation { get; set; } = DateTime.Now;
    }
}