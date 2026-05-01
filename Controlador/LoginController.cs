using AppLoginMVC.Modelo;
using AppLoginMVC.Vista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLoginMVC.Controlador
{
    internal class LoginController
    {
        private readonly UsuarioDAO _dao = new();
        /// <summary>
        /// Valida credenciales y redirige al formulario correcto.
        /// </summary>
        public void ProcesarLogin(string usuario, string contrasena, FrmLogin
       vista)
        {
            // 1. Campos vacíos
            if (string.IsNullOrWhiteSpace(usuario) ||
            string.IsNullOrWhiteSpace(contrasena))
            {
                vista.SetEstado("Ingresa usuario y contraseña.");
                return;
            }
            // 2. Verificar si la cuenta está bloqueada
            if (_dao.EstasBloqueado(usuario))
            {
                MessageBox.Show(
                "Esta cuenta está bloqueada por múltiples intentos fallidos.\n" +

                "Contacta al administrador del sistema.",
               "Cuenta bloqueada",
               MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            // 3. Verificar credenciales en la base de datos
            var resultado = _dao.LoginSP(usuario, contrasena);

            if (resultado == "OK")
            {
                var u = _dao.ObtenerUsuario(usuario);
                new FrmBienvenida(u).Show();
                vista.Hide();
            }
            else
            {
                vista.SetEstado(resultado);
            }
        }
        }
    }
