using Dapper;
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Biblioteca.Pages.Libros
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config) => _config = config;

        public List<Libro> Libros { get; set; }

        public async Task OnGet()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            using var con = Db.GetConnection(_config);
            Libros = (await con.QueryAsync<Libro>(
                "SELECT * FROM Libro WHERE UsuarioId=@Id", new { Id = userId }
            )).ToList();
        }
    }
}
