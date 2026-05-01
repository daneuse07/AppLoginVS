using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
namespace AppLoginMVC.Vista
{
    internal class CircularProgressBar : UserControl
    {
        private int _valor = 0;
        private int _grosor = 14;
        private System.Windows.Forms.Timer? _timer;
        private int _objetivo = 0;
        public CircularProgressBar()
        {
            Size = new Size(100, 100);
            DoubleBuffered = true; // evita parpadeo al redibujar
            BackColor = Color.Transparent;
        }
        // ── Propiedad principal ──────────────────────────
        public int Valor
        {
            get => _valor;
            set
            {
                _valor = Math.Clamp(value, 0, 100);
                Invalidate(); // fuerza el repintado
            }
        }
        public int Grosor
        {
            get => _grosor;
            set { _grosor = value; Invalidate(); }
        }
        // ── Color dinámico según el valor ────────────────
        private Color GetColorArco() => _valor switch
        {
            < 25 => Color.FromArgb(200, 40, 40), // Rojo
            < 50 => Color.FromArgb(220, 130, 0), // Naranja
            < 75 => Color.FromArgb(200, 180, 0), // Amarillo
            _ => Color.FromArgb(40, 160, 60), // Verde
        };
        // ── Animación suave hacia un objetivo ────────────
        public void AnimarHacia(int objetivo, Action? onComplete = null)
        {
            _objetivo = Math.Clamp(objetivo, 0, 100);
            _timer?.Stop();
            _timer = new System.Windows.Forms.Timer { Interval = 12 };
            _timer.Tick += (s, e) =>
            {
                if (_valor < _objetivo)
                    Valor = Math.Min(_valor + 3, _objetivo);
                else if (_valor > _objetivo)
                    Valor = Math.Max(_valor - 3, _objetivo);
                else
                {
                    _timer.Stop();
                    onComplete?.Invoke();
                }
            };
            _timer.Start();
        }
        // ── Dibujado principal (GDI+) ────────────────────
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint =
            System.Drawing.Text.TextRenderingHint.AntiAlias;
            int margen = _grosor / 2 + 4;
            var rect = new Rectangle(margen, margen,
            Width - 2 * margen, Height - 2 * margen);
            // 1. Fondo del círculo (gris claro)
            using var penFondo = new Pen(Color.FromArgb(220, 220, 220), _grosor)
            { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawEllipse(penFondo, rect);
            // 2. Arco de progreso
            if (_valor > 0)
            {
                float angulo = 360f * _valor / 100f;
                using var penArco = new Pen(GetColorArco(), _grosor)
                { StartCap = LineCap.Round, EndCap = LineCap.Round };
                g.DrawArc(penArco, rect, -90f, angulo); // -90 = inicio arriba
            }
            // 3. Texto del porcentaje en el centro
            string texto = $"{_valor}%";
            using var font = new Font("Arial", rect.Width / 5f, FontStyle.Bold);
            var sz = g.MeasureString(texto, font);
            float tx = rect.X + (rect.Width - sz.Width) / 2f;
            float ty = rect.Y + (rect.Height - sz.Height) / 2f;
            using var brush = new SolidBrush(Color.FromArgb(30, 60, 100));
            g.DrawString(texto, font, brush, tx, ty);
        }

    }
}
