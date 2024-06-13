using ManejoPresupuestos.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuestos.Servicios
{
    public interface IRepositorioInformacionPersonal
    {
        Task Crear(InformacionPersonal informacionPersonal);
        Task Actualizar(InformacionPersonal informacionPersonal);
        Task Borrar(int id);
        Task<InformacionPersonal> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<InformacionPersonal>> Obtener(int usuarioId);
        Task<IEnumerable<InformacionPersonal>> BuscarPorCedula(int usuarioId, string cedula);
    }
    public class RepositorioInformacionPersonal : IRepositorioInformacionPersonal
    {
        private readonly string connectionString;

        public RepositorioInformacionPersonal(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(InformacionPersonal informacionPersonal)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(
                @"INSERT INTO InformacionPersonal (Nombre, PrimerApellido, SegundoApellido, Cedula, Correo, UsuarioId) 
                VALUES (@Nombre, @PrimerApellido, @SegundoApellido, @Cedula, @Correo, @UsuarioId);
                SELECT SCOPE_IDENTITY();", informacionPersonal);
            informacionPersonal.Id = id;
        }

        public async Task<IEnumerable<InformacionPersonal>> BuscarPorCedula(int usuarioId, string cedula)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<InformacionPersonal>(
                @"SELECT * FROM InformacionPersonal 
              WHERE UsuarioId = @UsuarioId AND Cedula LIKE '%' + @Cedula + '%';",
                new { UsuarioId = usuarioId, Cedula = cedula });
        }

        public async Task Actualizar(InformacionPersonal informacionPersonal)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"UPDATE InformacionPersonal 
                SET Nombre = @Nombre, PrimerApellido = @PrimerApellido, SegundoApellido = @SegundoApellido, Cedula = @Cedula, Correo = @Correo 
                WHERE Id = @Id AND UsuarioId = @UsuarioId;", informacionPersonal);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"DELETE FROM InformacionPersonal 
                WHERE Id = @Id;", new { id });
        }

        public async Task<InformacionPersonal> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<InformacionPersonal>(
                @"SELECT * FROM InformacionPersonal 
                WHERE Id = @Id AND UsuarioId = @UsuarioId;", new { id, usuarioId });
        }

        public async Task<IEnumerable<InformacionPersonal>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<InformacionPersonal>(
                @"SELECT * FROM InformacionPersonal 
                WHERE UsuarioId = @UsuarioId;", new { usuarioId });
        }
    }



}

