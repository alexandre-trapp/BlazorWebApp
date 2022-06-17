using System.ComponentModel.DataAnnotations;

namespace VitaWeb.Shared
{
    public class UFCreate
    {
        [Required(ErrorMessage = "O Id/sigla da uf é obrigatório.")]
        [StringLength(maximumLength: 2, ErrorMessage = "A sigla da uf não pode ter mais de dois caracteres")]
        [RegularExpression(@"^[a-zA-Z- ]{1,40}$", ErrorMessage = "Números e símbolos não são permitidos na sigla da UF.")]
        public string Id { get; set; }

        public string Stt { get; set; } = "A";

        [Required(ErrorMessage = "O nome da uf é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome da uf não pode ter mais de vinte caracteres")]
        [RegularExpression(@"^[a-zA-Z- ]{1,40}$", ErrorMessage = "Números não são permitidos no nome.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O nome da região da uf é obrigatório.")]
        [StringLength(15, ErrorMessage = "O nome da região da uf não pode ter mais de 15 caracteres")]
        [RegularExpression(@"^[a-zA-Z- ]{1,40}$", ErrorMessage = "Números não são permitidos no nome da região.")]
        public string Regiao { get; set; }
    }
}
