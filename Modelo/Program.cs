using AppLoginMVC.Modelo;
using AppLoginMVC.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppLoginMVC.Modelo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configuración estándar de WinForms
            ApplicationConfiguration.Initialize();
            // Verificar conexión a MySQL antes de iniciar
            if (!ConexionDB.ProbarConexion())
            {
                MessageBox.Show(
                "No se pudo conectar a la base de datos MySQL.\n" +
               "Verifica que MySQL esté activo y que los datos\n" +
               "de conexión en ConexionDB.cs sean correctos.",
               "Error de conexión",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return; // Salir si no hay conexión
            }
            // Iniciar con el formulario de Login
            Application.Run(new FrmLogin());
        }
    }

}
