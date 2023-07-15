using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PloomesAPITeste.Models
{
    /// <summary>
    /// Modelo de uma conta de usu�rio no sistema
    /// </summary>
    public class UserAccount
    {
        /// <summary>
        /// ID da conta do usu�rio no banco de dados
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome de usu�rio
        /// </summary>
        [Required]
        [MaxLength(20, ErrorMessage = "Nome de usu�rio n�o pode ser maior que 20 caracteres")]
        [MinLength(3, ErrorMessage = "Nome de usu�rio precisa ter pelo menos 3 caracteres")]
        public required string Username { get; set; }

        /// <summary>
        /// Uma breve apresenta��o escrita pelo usu�rio
        /// </summary>
        [MaxLength(50, ErrorMessage = "Apresenta��o do usu�rio n�o pode ser maior que 50 caracteres")]
        [DisplayName("About me")]
        public string? AboutMe { get; set; }

        /// <summary>
        /// Data e hora da cria��o da conta
        /// </summary>
        public DateTime AccountCreation { get; set; } = DateTime.Now;
    }
}