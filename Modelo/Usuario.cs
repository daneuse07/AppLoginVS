using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLoginMVC.Modelo
{
    internal class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public byte[] Foto { get; set; } // BLOB: bytes de la imagen
        public int NivelSeguridad { get; set; } // 0=Muy débil ... 4=Muy fuerte
        public bool Bloqueado { get; set; }
        public int Intentos { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; } = true;
        // Propiedad calculada: nombre completo
        public string NombreCompleto => $"{Nombre} {Apellido}";
        public override string ToString() =>
        $"{NombreCompleto} (@{NombreUsuario})";

    }
}
