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
                new Libro { Id = 1, Titulo = "El Principito", Autor = "Antoine de Saint-Exupéry", Año = 1943, Descripcion = "", ImagenUrl = "/img/Principito.jpg" },
                new Libro { Id = 2, Titulo = "1984", Autor = "George Orwell", Año = 1949, Descripcion = "", ImagenUrl = "/img/1984.jpg" },
                new Libro { Id = 3, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Año = 1967, Descripcion = "", ImagenUrl = "/img/Cien.jpg" },
                new Libro { Id = 4, Titulo = "Cadáver exquisito", Autor = "Agustina Bazterrica", Año = 2017, Descripcion = "", ImagenUrl = "/img/cadaver.jpg"},
                new Libro { Id = 5, Titulo = "Poesia Completa", Autor = "Alejandra Pizarnik", Año = 2001, Descripcion = "", ImagenUrl = "/img/Poesia.jpg"}
            };

            return Page();
        }
    }
}