using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Models
{
    public class CatalogoLibro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int? Anio { get; set; }
        public string Descripcion { get; set; }

        public string? ImagenUrl { get; set; }  // ✅ NUEVO

        public bool YaEnMiBiblioteca { get; set; } // Solo para vista
    }

}