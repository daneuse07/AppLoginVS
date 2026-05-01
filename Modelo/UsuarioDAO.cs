using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
namespace AppLoginMVC.Modelo
{
    internal class UsuarioDAO
    {
        private MySqlConnection GetConn() => ConexionDB.Instancia.GetConexion();
        // ═══════════════════════════════════════════════════
        // CREATE — Insertar usuario con foto BLOB
        // ═══════════════════════════════════════════════════
        public bool Insertar(Usuario u)
        {
            const string sql = @"INSERT INTO usuarios
 (nombre, apellido, email, usuario, contrasena, foto, nivel_seg)
 VALUES (@nom, @ape, @mail, @usr, @pass, @foto, @niv)";
            try
            {
                using var cmd = new MySqlCommand(sql, GetConn());
                cmd.Parameters.AddWithValue("@nom", u.Nombre);
                cmd.Parameters.AddWithValue("@ape", u.Apellido);
                cmd.Parameters.AddWithValue("@mail", u.Email);
                cmd.Parameters.AddWithValue("@usr", u.NombreUsuario);
                cmd.Parameters.AddWithValue("@pass", u.Contrasena);
                cmd.Parameters.AddWithValue("@foto", (object?)u.Foto ??
               DBNull.Value);
                cmd.Parameters.AddWithValue("@niv", u.NivelSeguridad);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[DAO] Error Insertar: {ex.Message}");
                return false;
            }
        }
        // ═══════════════════════════════════════════════════
        // READ — Login por credenciales
        // ═══════════════════════════════════════════════════
        public string LoginSP(string usuario, string contrasena)
        {
            using var cmd = new MySqlCommand("sp_login", GetConn());
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@p_usuario", usuario);
            cmd.Parameters.AddWithValue("@p_contrasena", contrasena);

            var output = new MySqlParameter("@p_resultado", MySqlDbType.VarChar, 30);
            output.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(output);

            cmd.ExecuteNonQuery();

            return output.Value.ToString();
        }
        // ═══════════════════════════════════════════════════
        // READ — Listar todos los usuarios activos
        // ═══════════════════════════════════════════════════
        public List<Usuario> ListarTodos()
        {
            var lista = new List<Usuario>();
            const string sql = "SELECT id,nombre,apellido,email,usuario,nivel_seg," + "intentos,bloqueado,ultimo_acc,fecha_reg FROM usuarios";
        try
            {
                using var cmd = new MySqlCommand(sql, GetConn());
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Usuario
                    {
                        Id = reader.GetInt32("id"),
                        Nombre = reader.GetString("nombre"),
                        Apellido = reader.GetString("apellido"),
                        Email = reader.GetString("email"),
                        NombreUsuario = reader.GetString("usuario"),
                        NivelSeguridad = reader.GetInt32("nivel_seg"),
                        Intentos = reader.GetInt32("intentos"),
                        Bloqueado = reader.GetBoolean("bloqueado"),
                    });
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[DAO] Error Listar: {ex.Message}");
            }
            return lista;
        }
        // ═══════════════════════════════════════════════════
        // UPDATE — Actualizar datos del usuario
        // ═══════════════════════════════════════════════════
        public bool Actualizar(Usuario u)
        {
            const string sql = @"UPDATE usuarios
 SET nombre=@nom, apellido=@ape, email=@mail,
 contrasena=@pass, foto=@foto, nivel_seg=@niv
 WHERE id=@id";
            try
            {
                using var cmd = new MySqlCommand(sql, GetConn());
                cmd.Parameters.AddWithValue("@nom", u.Nombre);
                cmd.Parameters.AddWithValue("@ape", u.Apellido);
                cmd.Parameters.AddWithValue("@mail", u.Email);
                cmd.Parameters.AddWithValue("@pass", u.Contrasena);
                cmd.Parameters.AddWithValue("@foto", (object?)u.Foto ??
DBNull.Value);
                cmd.Parameters.AddWithValue("@niv", u.NivelSeguridad);
                cmd.Parameters.AddWithValue("@id", u.Id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[DAO] Error Actualizar: {ex.Message}");
                return false;
            }
        }
        // ═══════════════════════════════════════════════════
        // DELETE — Borrado lógico (activo = 0)
        // ═══════════════════════════════════════════════════
        public bool Eliminar(int id)
        {
            try
            {
                using var cmd = new MySqlCommand(
                "UPDATE usuarios SET activo=0 WHERE id=@id", GetConn());
                cmd.Parameters.AddWithValue("@id", id);
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"[DAO] Error Eliminar: {ex.Message}");
                return false;
            }
        }
        public Usuario? ObtenerUsuario(string usuario)
        {
            const string sql = @"SELECT * FROM usuarios WHERE usuario = @usr LIMIT 1";

            using var cmd = new MySqlCommand(sql, GetConn());
            cmd.Parameters.AddWithValue("@usr", usuario);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return MapearUsuario(reader);
            }

            return null;
        }
        // ── Helpers públicos ──────────────────────────────
        public bool UsuarioExiste(string usuario)
        {
            using var cmd = new MySqlCommand(
            "SELECT COUNT(*) FROM usuarios WHERE usuario=@u", GetConn());
            cmd.Parameters.AddWithValue("@u", usuario);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }
        public bool EstasBloqueado(string usuario)
        {
            using var cmd = new MySqlCommand(
            "SELECT bloqueado FROM usuarios WHERE usuario=@u", GetConn());
            cmd.Parameters.AddWithValue("@u", usuario);
            var val = cmd.ExecuteScalar();
            return val != null && Convert.ToInt32(val) == 1;
        }
        // ── Privados ──────────────────────────────────────
        private void ActualizarUltimoAcceso(string usuario)
        {
            using var cmd = new MySqlCommand(
            "UPDATE usuarios SET intentos=0, ultimo_acc=NOW() WHERE usuario = @u",
           
            GetConn());
            cmd.Parameters.AddWithValue("@u", usuario);
            cmd.ExecuteNonQuery();
        }
        private static Usuario MapearUsuario(MySqlDataReader r) => new()
        {
            Id = r.GetInt32("id"),
            Nombre = r.GetString("nombre"),
            Apellido = r.GetString("apellido"),
            Email = r.GetString("email"),
            NombreUsuario = r.GetString("usuario"),
            Foto = r.IsDBNull(r.GetOrdinal("foto")) ? null
       : (byte[])r["foto"],
            NivelSeguridad = r.GetInt32("nivel_seg"),
            Bloqueado = r.GetBoolean("bloqueado"),
            Intentos = r.GetInt32("intentos"),
            Activo = r.GetBoolean("activo"),
        };

    }
}
