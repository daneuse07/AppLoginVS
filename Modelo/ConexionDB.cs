using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace AppLoginMVC.Modelo
{
    internal class ConexionDB
    {
        // ── Parámetros de conexión — ajusta según tu entorno ──────
        private const string Servidor = "localhost";
        private const string Puerto = "3306";
        private const string BaseDatos = "app_login_csharp";
        private const string Usuario = "root";
        private const string Password = "daneuse707"; // <-- Cambiar
        private static readonly string CadenaConexion =
        $"Server={Servidor};Port={Puerto};Database={BaseDatos};" +
        $"User={Usuario};Password={Password};" +
        "CharSet=utf8mb4;AllowPublicKeyRetrieval=true;";
        // Instancia única (Singleton)
        private static ConexionDB? _instancia;
        private MySqlConnection? _conexion;
        private ConexionDB() { }
        public static ConexionDB Instancia
        {
            get
            {
                _instancia ??= new ConexionDB();
                return _instancia;
            }
        }
        // Obtener (o abrir) la conexión
        public MySqlConnection GetConexion()
        {
            try
            {
                if (_conexion == null)
                    _conexion = new MySqlConnection(CadenaConexion);
                if (_conexion.State != System.Data.ConnectionState.Open)
                    _conexion.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            return _conexion;
        }
        // Cerrar conexión de forma segura
        public void Cerrar()
        {
            if (_conexion?.State == System.Data.ConnectionState.Open)
                _conexion.Close();
        }
        // Probar conexión (útil en el inicio de la app)
        public static bool ProbarConexion()
        {
            try
            {
                using var conn = new MySqlConnection(CadenaConexion);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

    }
}