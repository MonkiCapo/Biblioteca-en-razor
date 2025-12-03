using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Libros
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguration _config;

        public EditarModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public Libro Libro { get; set; }

        // Cargar libro
        public async Task<IActionResult> OnGet(int id)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
                return RedirectToPage("/Login");

            var userId = int.Parse(claim);

            using var con = Db.GetConnection(_config);

            Libro = await con.QueryFirstOrDefaultAsync<Libro>(
                "SELECT * FROM Libro WHERE Id=@Id AND UsuarioId=@UserId",
                new { Id = id, UserId = userId }
            );

            if (Libro == null)
                return RedirectToPage("Index");

            return Page();
        }

        // Guardar cambios
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
                return RedirectToPage("/Login");

            var userId = int.Parse(claim);

            using var con = Db.GetConnection(_config);

            await con.ExecuteAsync(
                "UPDATE Libro SET " +
                "Titulo=@Titulo, Autor=@Autor, Anio=@Anio, Descripcion=@Descripcion " +
                "WHERE Id=@Id AND UsuarioId=@UserId",
                new
                {
                    Libro.Titulo,
                    Libro.Autor,
                    Libro.Anio,
                    Libro.Descripcion,
                    Libro.Id,
                    UserId = userId
                }
            );

            return RedirectToPage("Index");
        }
    }
}
