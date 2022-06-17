using System.ComponentModel.DataAnnotations;

namespace VitaWeb.Shared
{
    public partial class UsuarioAuthRequest
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [RegularExpression(@"^([\w\-]+\.)*[\w\-]+@([\w\- ]+\.)+([\w\-]{2,3})$", ErrorMessage = "E-mail deve ser válido, não ter espaços, ter letras minúsculas, ter @ no meio e não iniciar com www, http ou https!")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "O E-mail deve ter no mínimo 2 e máximo 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "A senha deve ter no mínimo 4 e no máximo 100 caracteres")]
        public string Password { get; set; }
    }
}
