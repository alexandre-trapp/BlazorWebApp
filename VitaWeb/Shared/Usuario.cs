using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VitaWeb.Shared
{
    public partial class Usuario
    {
        [Required(ErrorMessage = "O Id é obrigatório.")]
        [RegularExpression(@"^(?=[a-zA-Z-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÍÏÓÔÕÖÚÇÑ0-9._]{2,12}$)(?!.*[_.]{2})[^_.].*[^_.]$",
                    ErrorMessage = "São permitidos somente letras minúscula/maiúsculas, com acentuação e números no id do usuário, no mínimo com 2 e máximo 12 caracteres")]
        public string Id { get; set; }

        [Required(ErrorMessage = "O E-mail é obrigatório.")]
        [RegularExpression(@"^([\w\-]+\.)*[\w\-]+@([\w\- ]+\.)+([\w\-]{2,3})$", ErrorMessage = "E-mail deve ser válido, não ter espaços, ter letras minúsculas, ter @ no meio e não iniciar com www, http ou https!")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "O E-mail deve ter no mínimo 2 e máximo 100 caracteres")]
        public string Email { get; set; }

        public string Stt { get; set; } = "A";

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "A senha deve ter no mínimo 4 e no máximo 100 caracteres")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A senha de confirmação é obrigatória.")]
        [StringLength(maximumLength: 100, MinimumLength = 4, ErrorMessage = "A senha de confirmação deve ter no mínimo 4 e no máximo 100 caracteres")]
        [Compare(nameof(Password), ErrorMessage = "Senhas não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}
