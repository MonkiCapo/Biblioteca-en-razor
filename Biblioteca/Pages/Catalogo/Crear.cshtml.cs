using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;
using System.Security.Claims;

namespace Biblioteca.Pages.Catalogo
{
    public class CrearModel : PageModel
    {
        private readonly IConfiguration _config;

        public CrearModel(IConfiguration config)
        {
            _config = config;
        }

        [BindProperty]
        public CatalogoLibro Libro { get; set; }

        public IActionResult OnGet()
        {
            // ✅ Solo ADMIN puede entrar
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol != "Admin")
                return RedirectToPage("/Catalogo/Index");

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            // ✅ Solo ADMIN puede insertar
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol != "Admin")
                return RedirectToPage("/Catalogo/Index");

            if (!ModelState.IsValid)
                return Page();

            using var con = Db.GetConnection(_config);

            await con.ExecuteAsync(
                @"INSERT INTO CatalogoLibro 
                (Titulo, Autor, Anio, Descripcion)
                VALUES (@Titulo, @Autor, @Anio, @Descripcion)",
                new
                {
                    Libro.Titulo,
                    Libro.Autor,
                    Libro.Anio,
                    Libro.Descripcion
                }
            );

            return RedirectToPage("Index");
        }
    }
}
