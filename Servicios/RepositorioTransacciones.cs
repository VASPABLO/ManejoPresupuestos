using Dapper;
using ManejoPresupuestos.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuestos.Servicios
{
    public interface IRepositorioTransacciones
    {
        Task Actualizar(Transaccion transaccion, decimal MontoAnterior, int CuentaAnteriorId);
        Task Borrar(int id);
        Task Crear(Transaccion transaccion);
        Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionPorCuenta modelo);
        Task<Transaccion> ObtenerPorId(int id, int usuarioId);
        Task<IEnumerable<ResultadoObtenerPorMes>> ObtenerPorMes(int usuarioId, int año);
        Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(ParametroObtenerTransaccionesPorUsuario modelo);
        Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo);
    }
    public class RepositorioTransacciones: IRepositorioTransacciones
    {
        private readonly string connectionString;
        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Transaccion transaccion)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>("Transacciones_Insertar",
                new {
                    transaccion.UsuarioId, 
                    transaccion.FechaTransaccion, 
                    transaccion.Monto, 
                    transaccion.CategoriaId, 
                    transaccion.CuentaId, 
                    transaccion.Nota 
                },
                commandType: System.Data.CommandType.StoredProcedure);
            transaccion.Id = id;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorCuentaId(ObtenerTransaccionPorCuenta modelo)
        {
            using var conection = new SqlConnection(connectionString);
            return await conection.QueryAsync<Transaccion>(@"SELECT t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                                                            cu.Nombre as Cuenta, c.TipoOperacionId
                                                            FROM Transacciones t
                                                            INNER JOIN Categorias c
                                                            ON c.Id = t.CategoriaId
                                                            INNER JOIN Cuentas cu
                                                            ON cu.Id = t.CuentaId
                                                            WHERE t.CuentaId = @CuentaId AND t.UsarioId = @UsuarioId
                                                            AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin", modelo);
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioId(ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var conection = new SqlConnection(connectionString);
            return await conection.QueryAsync<Transaccion>(@"SELECT t.Id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria,
                                                            cu.Nombre as Cuenta, c.TipoOperacionId, Nota
                                                            FROM Transacciones t
                                                            INNER JOIN Categorias c
                                                            ON c.Id = t.CategoriaId
                                                            INNER JOIN Cuentas cu
                                                            ON cu.Id = t.CuentaId
                                                            WHERE  t.UsarioId = @UsuarioId
                                                            AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin
                                                            ORDER BY t.FechaTransaccion DESC", modelo);
                                                           
        }

        public async Task Actualizar(Transaccion transaccion, decimal montoAnterios, int cuentaAnteriorId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Actualizar",
                new
                {
                    transaccion.Id,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota,
                    montoAnterios,
                    cuentaAnteriorId
                },
                commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<Transaccion> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaccion>(
                @"SELECT Transacciones. *, cat.TipoOperacionId
                    FROM Transacciones
                INNER JOIN Categorias cat
                ON CAT.Id = Transacciones.CategoriaId
                WHERE Transacciones.Id = @Id AND Transacciones.UsarioId = @UsuarioId", new { id, usuarioId });
        }

        public async Task<IEnumerable<ResultadoObtenerPorSemana>> ObtenerPorSemana(
            ParametroObtenerTransaccionesPorUsuario modelo)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ResultadoObtenerPorSemana>(@"
             SELECT DATEDIFF (d, @fechaInicio, FechaTransaccion) / 7 + 1 as Semana,
                SUM (Monto) as Monto, cat.TipoOperacionId
                FROM Transacciones
                INNER JOIN Categorias cat
                ON cat.Id = Transacciones.CategoriaId
                WHERE Transacciones.UsarioId = @usuarioId AND
                FechaTransaccion BETWEEN @fechaInicio and @fechaFin
                GROUP BY DATEDIFF(d, @fechaInicio, FechaTransaccion) / 7, 
                cat.TipoOperacionId",modelo);
        }

        public async Task<IEnumerable<ResultadoObtenerPorMes>> ObtenerPorMes(int usuarioId,
            int año)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<ResultadoObtenerPorMes>(@"
                    SELECT MONTH (FechaTransaccion) as Mes,
                    SUM (Monto) as Monto, cat.TipoOperacionId
                    FROM Transacciones
                    INNER JOIN Categorias cat
                    ON cat.Id = Transacciones.CategoriaId
                    WHERE Transacciones.UsarioId = @usuarioId AND YEAR(FechaTransaccion) = @Año
                    GROUP BY Month(FechaTransaccion), cat.TipoOperacionId", new { usuarioId, año });
        }

       public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("Transacciones_Borrar",
                new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
