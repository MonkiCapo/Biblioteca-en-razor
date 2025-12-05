using Biblioteca.Data;
using Biblioteca.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteca.Pages.Catalogo
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public EliminarModel(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        // ✅ Necesario para mostrar Titulo y Autor
        public CatalogoLibro Libro { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.IsInRole("Admin"))
                return RedirectToPage("/Catalogo/Index");

            using var con = Db.GetConnection(_config);

            Libro = await con.QueryFirstOrDefaultAsync<CatalogoLibro>(
                "SELECT * FROM CatalogoLibro WHERE Id=@id",
                new { id });

            if (Libro == null)
                return RedirectToPage("Index");

            Id = id;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!User.IsInRole("Admin"))
                return RedirectToPage("/Catalogo/Index");

            using var con = Db.GetConnection(_config);

            // ✅ Traer imagen antes de borrar
            var imagen = await con.QueryFirstOrDefaultAsync<string>(
                "SELECT ImagenUrl FROM CatalogoLibro WHERE Id=@Id",
                new { Id });

            // ✅ Borrar imagen física
            if (!string.IsNullOrEmpty(imagen))
            {
                var rutaFisica = Path.Combine(_env.WebRootPath, imagen.TrimStart('/'));
                if (System.IO.File.Exists(rutaFisica))
                    System.IO.File.Delete(rutaFisica);
            }

            // ✅ Borrar registro
            await con.ExecuteAsync(
                "DELETE FROM CatalogoLibro WHERE Id=@Id",
                new { Id });

            return RedirectToPage("Index");
        }
    }
}
