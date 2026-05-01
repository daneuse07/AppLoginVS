using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLoginMVC.Modelo;
namespace AppLoginMVC.Vista
{
    internal class FrmBienvenida: Form
    {
        public FrmBienvenida(Usuario usuario)
        {
            InitComponentsManual(usuario);
            ConfigurarVentana();
        }
        private void InitComponentsManual(Usuario u)
        {
            BackColor = Color.FromArgb(235, 245, 255);

            var picFoto = new PictureBox
            {
                Location = new Point(20, 20),
                Size = new Size(130, 130),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(210, 230, 250)
            };

            if (u.Foto != null && u.Foto.Length > 0)
            {
                using var ms = new System.IO.MemoryStream(u.Foto);
                picFoto.Image = Image.FromStream(ms);
            }

            var lblBienvenida = new Label
            {
                Text = "¡Bienvenido al Sistema!",
                Location = new Point(170, 25),
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 60, 107)
            };

            var lblNombre = new Label
            {
                Text = u.NombreCompleto,
                Location = new Point(170, 60),
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Arial", 13),
                ForeColor = Color.FromArgb(50, 50, 50)
            };

            var lblUsuario = new Label
            {
                Text = $"@{u.NombreUsuario} | {u.Email}",
                Location = new Point(170, 90),
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Arial", 9, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            string etiq = PasswordUtils.GetEtiqueta(u.NivelSeguridad);
            Color col = PasswordUtils.GetColor(u.NivelSeguridad);

            var lblSeg = new Label
            {
                Text = $"Seguridad de contraseña: {etiq}",
                Location = new Point(170, 115),
                AutoSize = true,
                MaximumSize = new Size(300, 0),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = col
            };

            var separador = new Panel
            {
                Location = new Point(20, 165),
                Size = new Size(450, 1),
                BackColor = Color.FromArgb(46, 93, 168)
            };

            var lblAcceso = new Label
            {
                Text = u.UltimoAcceso.HasValue
                    ? $"Último acceso: {u.UltimoAcceso:dd/MM/yyyy HH:mm}"
                    : "Primer acceso al sistema",
                Location = new Point(20, 175),
                Size = new Size(440, 20),
                Font = new Font("Arial", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnSalir = new Button
            {
                Text = "Cerrar Sesión",
                Location = new Point(160, 205),
                Size = new Size(160, 38),
                BackColor = Color.FromArgb(176, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Arial", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };

            btnSalir.Click += (s, e) =>
            {
                new FrmLogin().Show();
                Close();
            };

            Controls.AddRange(new Control[]
            {
        picFoto, lblBienvenida, lblNombre, lblUsuario,
        lblSeg, separador, lblAcceso, btnSalir
            });
        }
        private void ConfigurarVentana()
        {
            Text = "Bienvenida";
            Size = new Size(500, 320);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }
        }
    }
