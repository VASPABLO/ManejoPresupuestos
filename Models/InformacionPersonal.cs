namespace ManejoPresupuestos.Models
{
    public class InformacionPersonal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public int UsuarioId { get; set; }
    }
}
