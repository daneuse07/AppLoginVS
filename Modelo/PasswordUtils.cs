using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace AppLoginMVC.Modelo
{
    internal class PasswordUtils
    {
        // ── Requisitos mínimos de seguridad ────────────────────
        public const int MIN_LONGITUD = 8;
        public const int FUERTE_LONGITUD = 12;
        private static readonly Regex RgxMayuscula = new(@"[A-Z]");
        private static readonly Regex RgxMinuscula = new(@"[a-z]");
        private static readonly Regex RgxNumero = new(@"[0-9]");
        private static readonly Regex RgxSimbolo = new(@"[!@#$%^&*()_+\-=\[\]{};':.,<>?]");
        /// <summary>
        /// Calcula el nivel de fortaleza de 0 a 4.
        /// 0=Muy débil 1=Débil 2=Media 3=Fuerte 4=Muy fuerte
        /// </summary>
        public static int CalcularFortaleza(string pass)
        {
            if (string.IsNullOrEmpty(pass)) return 0;
            int score = 0;
            if (pass.Length >= MIN_LONGITUD) score++;
            if (pass.Length >= FUERTE_LONGITUD) score++;
            if (RgxMayuscula.IsMatch(pass)) score++;
            if (RgxMinuscula.IsMatch(pass)) score++;
            if (RgxNumero.IsMatch(pass)) score++;
            if (RgxSimbolo.IsMatch(pass)) score++;
            return Math.Min(4, Math.Max(0, score - 1));
        }
        /// <summary>
        /// Verifica si la contraseña cumple TODOS los requisitos mínimos.
        /// </summary>
        public static bool EsValida(string pass) =>
        !string.IsNullOrEmpty(pass) &&
        pass.Length >= MIN_LONGITUD &&
        RgxMayuscula.IsMatch(pass) &&
        RgxMinuscula.IsMatch(pass) &&
        RgxNumero.IsMatch(pass) &&
        RgxSimbolo.IsMatch(pass);
        /// <summary>
        /// Devuelve los requisitos NO cumplidos (para mostrar en pantalla).
        /// </summary>
        public static List<string> GetRequisitosIncumplidos(string pass)
        {
            var faltantes = new List<string>();
            if (string.IsNullOrEmpty(pass) || pass.Length < MIN_LONGITUD)
                faltantes.Add($"Mínimo {MIN_LONGITUD} caracteres");
            if (!RgxMayuscula.IsMatch(pass ?? ""))
                faltantes.Add("Al menos 1 letra MAYÚSCULA (A-Z)");
            if (!RgxMinuscula.IsMatch(pass ?? ""))
                faltantes.Add("Al menos 1 letra minúscula (a-z)");
            if (!RgxNumero.IsMatch(pass ?? ""))
                faltantes.Add("Al menos 1 número (0-9)");
            if (!RgxSimbolo.IsMatch(pass ?? ""))
                faltantes.Add("Al menos 1 símbolo especial (!@#$%)");
            return faltantes;
        }
        /// <summary> Etiqueta del nivel para mostrar en la UI. </summary>
        public static string GetEtiqueta(int nivel) => nivel switch
        {
            0 => "Muy débil",
            1 => "Débil",
            2 => "Media",
            3 => "Fuerte",
            4 => "Muy fuerte",
            _ => ""
        };
        /// <summary> Color para la UI según el nivel de fortaleza. </summary>
        public static Color GetColor(int nivel) => nivel switch
        {
            0 => Color.FromArgb(176, 0, 0), // Rojo oscuro
            1 => Color.FromArgb(200, 80, 0), // Naranja oscuro
            2 => Color.FromArgb(190, 160, 0), // Amarillo oscuro
            3 => Color.FromArgb(0, 130, 50), // Verde
            4 => Color.FromArgb(0, 100, 30), // Verde oscuro
            _ => Color.Gray
        };

    }
}
