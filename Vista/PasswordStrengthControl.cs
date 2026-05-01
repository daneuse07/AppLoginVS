using AppLoginMVC.Modelo;
using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AppLoginMVC.Vista
{
    internal class PasswordStrengthControl : UserControl
    {
        private CircularProgressBar _barra;
        private Label _lblNivel;
        private Label[] _lblReqs;
        private static readonly string[] REQUISITOS =
        {
 "Mínimo 8 caracteres",
 "Al menos 1 mayuscula (A-Z)",
 "Al menos 1 minúscula (a-z)",
 "Al menos 1 número (0-9)",
 "Al menos 1 símbolo (!@#$%)"
 };
        public PasswordStrengthControl()
        {
            Size = new Size(340, 110);
            BackColor = Color.FromArgb(245, 250, 255);
            BorderStyle = BorderStyle.FixedSingle;
            // Barra circular izquierda
            _barra = new CircularProgressBar
            {
                Size = new Size(90, 90),
                Location = new Point(8, 8),
                Grosor = 11
            };
            Controls.Add(_barra);
            // Etiqueta nivel
            _lblNivel = new Label
            {
                Text = "Sin contraseña",
                Location = new Point(8, 96),
                Size = new Size(90, 16),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 8, FontStyle.Bold),
                ForeColor = Color.Gray
            };
            Controls.Add(_lblNivel);
            // Lista de requisitos
            _lblReqs = new Label[REQUISITOS.Length];
            for (int i = 0; i < REQUISITOS.Length; i++)
            {
                _lblReqs[i] = new Label
                {
                    Text = $" ✗ {REQUISITOS[i]}",
                    Location = new Point(106, 8 + i * 20),
                    Size = new Size(228, 18),
                    Font = new Font("Arial", 9),
                    ForeColor = Color.FromArgb(160, 40, 40)
                };
                Controls.Add(_lblReqs[i]);
            }
        }
        // Llamar desde el evento TextChanged de la caja de contraseña
        public void Actualizar(string pass)
        {
            int nivel = PasswordUtils.CalcularFortaleza(pass);
            _barra.AnimarHacia(nivel * 25);
            _lblNivel.Text = PasswordUtils.GetEtiqueta(nivel);
            _lblNivel.ForeColor = PasswordUtils.GetColor(nivel);
            string p = pass ?? "";
            SetReq(0, p.Length >= PasswordUtils.MIN_LONGITUD);
            SetReq(1, System.Text.RegularExpressions.Regex.IsMatch(p, "[A-Z]"));
            SetReq(2, System.Text.RegularExpressions.Regex.IsMatch(p, "[a-z]"));
            SetReq(3, System.Text.RegularExpressions.Regex.IsMatch(p, "[0-9]"));
            SetReq(4, System.Text.RegularExpressions.Regex.IsMatch(
            p, @"[!@#$%^&*()_+\-=\[\]{};':.,<>?]"));
        }
        private void SetReq(int i, bool ok)
        {
            if (ok)
            {
                _lblReqs[i].Text = $" ✓ {REQUISITOS[i]}";
                _lblReqs[i].ForeColor = Color.FromArgb(30, 130, 60);
            }
            else
            {
                _lblReqs[i].Text = $" ✗ {REQUISITOS[i]}";
                _lblReqs[i].ForeColor = Color.FromArgb(160, 40, 40);
            }
        }
    }
}
