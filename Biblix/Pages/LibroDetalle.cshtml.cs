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
            // Aquí usarías tu base de datos. Por ahora, lista simulada
            var libros = new List<Libro>
{
    new Libro
    {
        Id = 1,
        Titulo = "El Principito",
        Autor = "Antoine de Saint-Exupéry",
        Año = 1943,
        Descripcion = "El Principito es una novela corta escrita e ilustrada por Antoine de Saint-Exupéry. " +
                     "La historia sigue a un piloto perdido en el desierto del Sahara que conoce a un joven príncipe de otro planeta. " +
                     "A través de sus conversaciones, el piloto reflexiona sobre la naturaleza humana, la amistad, el amor y el significado de la vida, " +
                     "contrastando la visión infantil y pura del príncipe con la visión más seria y a menudo superficial de los adultos."
    },
    new Libro
    {
        Id = 2,
        Titulo = "1984",
        Autor = "George Orwell",
        Año = 1949,
        Descripcion = "1984 de George Orwell es una novela distópica que describe un mundo totalitario en Oceanía, " +
                     "donde el Gran Hermano y el Partido controlan todos los aspectos de la vida de los ciudadanos, incluyendo sus pensamientos."
    },
    new Libro
    {
        Id = 3,
        Titulo = "Cien años de soledad",
        Autor = "Gabriel García Márquez",
        Año = 1967,
        Descripcion = "\"Cien años de soledad\", escrita por Gabriel García Márquez, es una novela que narra la historia de la familia Buendía " +
                     "a lo largo de siete generaciones en el pueblo ficticio de Macondo. La obra explora temas como la soledad, el paso del tiempo, " +
                     "el realismo mágico, el incesto y la historia de Latinoamérica, entrelazando lo mítico y lo real con una prosa rica y envolvente."
    }
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
