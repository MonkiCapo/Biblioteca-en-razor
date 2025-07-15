using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biblix.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace Biblix.Pages
{
    public class Libros : PageModel
    {
        public List<Libro> ListaLibros { get; set; }

        public IActionResult OnGet()
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                // Redirigir a login si no hay usuario
                return RedirectToPage("Login");
            }

            ListaLibros = new List<Libro>
            {
                new Libro { Id = 1, Titulo = "El Principito", Autor = "Antoine de Saint-Exupéry", Año = 1943 },
                new Libro { Id = 2, Titulo = "1984", Autor = "George Orwell", Año = 1949 },
                new Libro { Id = 3, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Año = 1967 }
            };

            return Page();
        }
    }
}