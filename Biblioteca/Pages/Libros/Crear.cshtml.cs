using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Libros
{
    public class CrearModel : PageModel
    {
        private readonly IConfiguration _config;

        public CrearModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public Libro Libro { get; set; }

        public void OnGet() { }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                ModelState.AddModelError(string.Empty, "Debes iniciar sesión para agregar libros.");
                return Page();
            }

            var userId = int.Parse(claim);

            using var con = Db.GetConnection(_config);

            await con.ExecuteAsync(
                "INSERT INTO Libro (Titulo, Autor, Anio, Descripcion, UsuarioId) " +
                "VALUES (@Titulo, @Autor, @Anio, @Descripcion, @UsuarioId)",
                new
                {
                    Libro.Titulo,
                    Libro.Autor,
                    Libro.Anio,
                    Libro.Descripcion,
                    UsuarioId = userId
                }
            );

            return RedirectToPage("Index");
        }
    }
}
