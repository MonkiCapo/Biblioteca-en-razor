using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Libros
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguration _config;

        public EliminarModel(IConfiguration config)
        {
            _config = config;
        }

        public Libro Libro { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var con = Db.GetConnection(_config);

            Libro = await con.QueryFirstOrDefaultAsync<Libro>(
                "SELECT * FROM Libro WHERE Id=@Id AND UsuarioId=@UserId",
                new { Id = id, UserId = userId }
            );

            if (Libro == null)
                return RedirectToPage("Index");

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var con = Db.GetConnection(_config);

            await con.ExecuteAsync(
                "DELETE FROM Libro WHERE Id=@Id AND UsuarioId=@UserId",
                new { Id = id, UserId = userId }
            );

            return RedirectToPage("Index");
        }
    }
}
