using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Libros
{
    public class DetallesModel : PageModel
    {
        private readonly IConfiguration _config;

        public DetallesModel(IConfiguration config)
        {
            _config = config;
        }

        public Libro Libro { get; set; }

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
    }
}
