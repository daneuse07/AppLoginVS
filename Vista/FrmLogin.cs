using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLoginMVC.Controlador;
namespace AppLoginMVC.Vista
{
    internal class FrmLogin : Form
    {
        private TextBox txtUsuario;
        private TextBox txtContrasena;
        private Button btnLogin, btnRegistrar;
        private Label lblEstado;
        private CircularProgressBar barraLogin;
        public FrmLogin()
        {
            InitComponentsManual();
            ConfigurarVentana();
        }
        private void InitComponentsManual()
        {
            // Panel principal
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 248, 255),
                Padding = new Padding(40)
            };
            // Título
            var lblTitulo = new Label
            {
                Text = "Iniciar Sesión",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 60, 107),
                Location = new Point(40, 20),
                Size = new Size(300, 36),
                TextAlign = ContentAlignment.MiddleCenter
            };
            // Barra circular decorativa
            barraLogin = new CircularProgressBar
            {
                Location = new Point(145, 62),
                Size = new Size(90, 90),
                Grosor = 12
            };
            // Campo Usuario
            var lblUsr = new Label
            {
                Text = "Usuario:",
                Location = new Point(40, 168),
                Size = new Size(80, 22)
            };
            txtUsuario = new TextBox
            {
                Location = new Point(125, 165),
                Size = new
           Size(210, 26)
            };
            // Campo Contraseña
            var lblPass = new Label
            {
                Text = "Contraseña:",
                Location = new
           Point(15, 208),
                Size = new Size(110, 22)
            };
            txtContrasena = new TextBox
            {
                Location = new Point(125, 205),
                Size = new Size(210, 26),
                PasswordChar = '●'
            };
            // Etiqueta de estado (errores)
            lblEstado = new Label
            {
                Text = "",
                Location = new Point(40, 242),
                Size = new Size(300, 20),
                ForeColor = Color.FromArgb(180, 0, 0),
                Font = new Font("Arial", 9, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter
            };
            // Botones
            btnLogin = new Button
            {
                Text = "Ingresar",
                Location = new Point(40, 272),
                Size = new Size(120, 36),
                BackColor = Color.FromArgb(46, 93, 168),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnRegistrar = new Button
            {
                Text = "Registrarse",
                Location = new Point(210, 272),
                Size = new Size(130, 36),
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            // ── Eventos ──────────────────────────────────────
            // Actualizar barra al escribir la contraseña
            txtContrasena.TextChanged += (s, e) =>
            {
                int nivel =
               Modelo.PasswordUtils.CalcularFortaleza(txtContrasena.Text);
                barraLogin.AnimarHacia(nivel * 25);
            };
            // Login: animar barra hasta 100% y luego validar
            btnLogin.Click += (s, e) =>
            {
                SetEstado("");
                barraLogin.AnimarHacia(100, () =>
                {
                    new LoginController().ProcesarLogin(
                    txtUsuario.Text.Trim(),
                   txtContrasena.Text.Trim(),
                   this);
                });
            };
            // Enter en contraseña = hacer clic en Login
            txtContrasena.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Return) btnLogin.PerformClick();
            };
            btnRegistrar.Click += (s, e) =>
            {
                new FrmRegistro().Show();
                Hide();
            };
            // Agregar controles al panel
            panel.Controls.AddRange(new Control[] {
 lblTitulo, barraLogin,
 lblUsr, txtUsuario,
 lblPass, txtContrasena,
 lblEstado, btnLogin, btnRegistrar
 });
            Controls.Add(panel);
        }
        public void SetEstado(string mensaje) => lblEstado.Text = mensaje;
        private void ConfigurarVentana()
        {
            Text = "Login — Sistema de Acceso";
            Size = new Size(400, 380);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

    }
}
