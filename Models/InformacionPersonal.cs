using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuestos.Models
{
    public class InformacionPersonal
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El primer apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El primer apellido no puede exceder los 50 caracteres.")]
        public string PrimerApellido { get; set; }

        [StringLength(50, ErrorMessage = "El segundo apellido no puede exceder los 50 caracteres.")]
        public string SegundoApellido { get; set; }

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "La cédula debe tener exactamente 9 caracteres.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "La cédula debe contener solo números.")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es una dirección de correo electrónico válida.")]
        public string Correo { get; set; }
    
        public int UsuarioId { get; set; }
    }
}
