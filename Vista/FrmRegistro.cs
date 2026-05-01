using AppLoginMVC.Controlador;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppLoginMVC.Vista
{
    internal class FrmRegistro: Form
    {
        private TextBox txtNombre, txtApellido, txtEmail, txtUsuario;
        private TextBox txtContrasena, txtConfirmar;
        private PictureBox picFoto;
        private Button btnSelFoto, btnGuardar, btnCancelar;
        private PasswordStrengthControl ctrlFortaleza;
        public byte[]? FotoBytes { get; private set; }
        public FrmRegistro()
        {
            InitComponentsManual();
            ConfigurarVentana();
        }
        private void InitComponentsManual()
        {
            var panel = new Panel
            {
                AutoScroll = true,
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 250, 255),
                Padding = new Padding(30)
            };
            // Título
            panel.Controls.Add(new Label
            {
                Text = "Registro de Usuario",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(26, 60, 107),
                Location = new Point(30, 15),
                Size = new Size(340, 30),
                TextAlign = ContentAlignment.MiddleCenter
            });
            // ── Función helper: agrega etiqueta + campo ───
            int y = 55;
            Control AgregarCampo(string label, Control ctrl)
            {
                panel.Controls.Add(new Label
                {
                    Text = label,
                    Location = new Point(30, y + 2),
                    Size = new Size(120, 22), // 👈 más ancho
                    Font = new Font("Arial", 10)
                });

                ctrl.Location = new Point(150, y); // 👈 más a la derecha
                ctrl.Size = new Size(190, 26);

                panel.Controls.Add(ctrl);

                y += 36;
                return ctrl;
            }
            txtNombre = (TextBox)AgregarCampo("Nombre:", new TextBox());
            txtApellido = (TextBox)AgregarCampo("Apellido:", new TextBox());
            txtEmail = (TextBox)AgregarCampo("Email:", new TextBox());
            txtUsuario = (TextBox)AgregarCampo("Usuario:", new TextBox());
            txtContrasena = (TextBox)AgregarCampo("Contraseña:", new TextBox
            {
                PasswordChar = '●'
            });
            txtConfirmar = (TextBox)AgregarCampo("Confirmar:", new TextBox
            {
                PasswordChar = '●'
            });
            // Panel de fortaleza
            ctrlFortaleza = new PasswordStrengthControl
            {
                Location = new Point(30, y),
                Size = new Size(340, 115)
            };
            panel.Controls.Add(ctrlFortaleza);
            y += 125;
            // Actualizar fortaleza al escribir
            txtContrasena.TextChanged += (s, e) =>
            ctrlFortaleza.Actualizar(txtContrasena.Text);
            // ── Sección foto ──────────────────────────────
            panel.Controls.Add(new Label
            {
                Text = "Foto:",
                Location = new Point(30, y + 8),
                Size = new Size(60, 22),
                Font = new Font("Arial", 10)
            });
            picFoto = new PictureBox
            {
                Location = new Point(95, y),
                Size = new Size(90, 90),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            panel.Controls.Add(picFoto);
            btnSelFoto = new Button
            {
                Text = "Seleccionar Foto",
                Location = new Point(195, y + 25),
                Size = new Size(150, 32),
                Font = new Font("Arial", 9)
            };
            panel.Controls.Add(btnSelFoto);
            y += 100;
            // ── Botones Guardar / Cancelar ────────────────
            btnGuardar = new Button
            {
                Text = "Guardar",
                Location = new Point(30, y + 10),
                Size = new Size(140, 36),
                BackColor = Color.FromArgb(30, 86, 49),
                ForeColor = Color.White,
                Font = new Font("Arial", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar = new Button
            {
                Text = "Cancelar",
                Location = new Point(210, y + 10),
                Size = new Size(140, 36),
                Font = new Font("Arial", 10),
                FlatStyle = FlatStyle.Flat
            };
            panel.Controls.AddRange(new Control[] { btnGuardar, btnCancelar });
            Controls.Add(panel);
            // ── Eventos ──────────────────────────────────────
            btnSelFoto.Click += (s, e) => SeleccionarFoto();
            btnGuardar.Click += (s, e) =>
            new RegistroController().ProcesarRegistro(txtNombre.Text, txtApellido.Text,txtEmail.Text, txtUsuario.Text,txtContrasena.Text, txtConfirmar.Text,
 FotoBytes, this);
            btnCancelar.Click += (s, e) =>
            {
                new FrmLogin().Show();
                Close();
            };
        }
        // Abre el explorador y carga la imagen seleccionada
        private void SeleccionarFoto()
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Seleccionar fotografía",
                Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FotoBytes = File.ReadAllBytes(dlg.FileName);
                picFoto.Image = Image.FromFile(dlg.FileName);
            }
        }
        private void ConfigurarVentana()
        {
            Text = "Registro de Usuario";
            Size = new Size(430, 660);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
        }

    }
}
