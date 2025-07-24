using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblix.Models;
using System.Collections.Generic;

namespace Biblix.Pages
{
    public class LibroDetalleModel : PageModel
    {
        public Libro LibroSeleccionado { get; set; }

        public IActionResult OnGet(int id)
        {
            var usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToPage("Login");
            }

           
        var libros = new List<Libro>
        {
            new Libro { Id = 1, Titulo = "El Principito", Autor = "Antoine de Saint-Exupéry", Año = 1943, Descripcion = "Un joven príncipe viaja a diferentes planetas y aprende sobre la vida.", ImagenUrl = "/img/Principito.jpg" },
            new Libro { Id = 2, Titulo = "1984", Autor = "George Orwell", Año = 1949, Descripcion = "Una novela distópica sobre un régimen totalitario que controla la vida de las personas.", ImagenUrl = "/img/1984.jpg" },
            new Libro { Id = 3, Titulo = "Cien años de soledad", Autor = "Gabriel García Márquez", Año = 1967, Descripcion = "La historia de la familia Buendía a lo largo de varias generaciones en el pueblo de Macondo.", ImagenUrl = "/img/Cien.jpg" }
        };

            LibroSeleccionado = libros.FirstOrDefault(l => l.Id == id);

            if (LibroSeleccionado == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
