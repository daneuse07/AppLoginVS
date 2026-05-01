using AppLoginMVC.Modelo;
using AppLoginMVC.Vista;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppLoginMVC.Controlador
{
    internal class RegistroController
    {
        private readonly UsuarioDAO _dao = new();
        public void ProcesarRegistro(
        string nombre, string apellido, string email, string usuario,
        string contrasena, string confirmar, byte[]? foto, FrmRegistro vista)
        {
            // 1. Campos obligatorios
            if (string.IsNullOrWhiteSpace(nombre) ||
            string.IsNullOrWhiteSpace(apellido) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(usuario) ||
            string.IsNullOrWhiteSpace(contrasena))
            {
                MessageBox.Show("Todos los campos son obligatorios.",
                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 2. Contraseñas coinciden
            if (contrasena != confirmar)
            {
                MessageBox.Show("Las contraseñas no coinciden.",
                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 3. Requisitos mínimos de seguridad de la contraseña
            var faltantes = PasswordUtils.GetRequisitosIncumplidos(contrasena);
            if (faltantes.Count > 0)
            {
                string msg = "La contraseña no cumple los requisitos:\n" +
                string.Join("\n", faltantes.Select(f => $" • {f}"));
                MessageBox.Show(msg, "Contraseña insegura",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // 4. Usuario ya existe
            if (_dao.UsuarioExiste(usuario))
            {
                MessageBox.Show("El nombre de usuario ya está en uso.",
                "Usuario duplicado", MessageBoxButtons.OK,
               MessageBoxIcon.Warning);
                return;
            }
            // 5. Crear y guardar el usuario
            int nivel = PasswordUtils.CalcularFortaleza(contrasena);
            var u = new Usuario
            {
                Nombre = nombre.Trim(),
                Apellido = apellido.Trim(),
                Email = email.Trim(),
                NombreUsuario = usuario.Trim(),
                Contrasena = contrasena,
                Foto = foto,
                NivelSeguridad = nivel
            };
            if (_dao.Insertar(u))
            {
                MessageBox.Show(
                "Usuario registrado correctamente.\nAhora puedes iniciar sesión.",
               
                "Registro exitoso", MessageBoxButtons.OK,
               MessageBoxIcon.Information);
                new FrmLogin().Show();
                vista.Close();
            }
            else
            {
                MessageBox.Show("Error al guardar en la base de datos.",
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}
