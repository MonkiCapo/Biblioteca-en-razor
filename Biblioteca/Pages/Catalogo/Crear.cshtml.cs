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
        private readonly IWebHostEnvironment _env;

        public CrearModel(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        [BindProperty]
        public CatalogoLibro Libro { get; set; }

        [BindProperty]
        public IFormFile Imagen { get; set; }   // ✅ Correcto

        public IActionResult OnGet()
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol != "Admin")
                return RedirectToPage("/Catalogo/Index");

            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost()
        {
            var rol = User.FindFirstValue(ClaimTypes.Role);
            if (rol != "Admin")
                return RedirectToPage("/Catalogo/Index");

             if (!ModelState.IsValid)
                 return Page();

            string rutaImagen = null;

            // ✅ VALIDACIÓN CORRECTA DE IMAGEN
            if (Imagen != null && Imagen.Length > 0)
            {
                var ext = Path.GetExtension(Imagen.FileName).ToLower();

                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                {
                    ModelState.AddModelError("", "Solo se permiten imágenes JPG o PNG.");
                    return Page();
                }

                var nombreArchivo = Guid.NewGuid() + ext;
                var carpeta = Path.Combine(_env.WebRootPath, "img", "libros");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                using var stream = new FileStream(rutaCompleta, FileMode.Create);
                await Imagen.CopyToAsync(stream);

                rutaImagen = "/img/libros/" + nombreArchivo;
            }

            using var con = Db.GetConnection(_config);

            await con.ExecuteAsync(
                @"INSERT INTO CatalogoLibro 
                (Titulo, Autor, Anio, Descripcion, ImagenUrl)
                VALUES (@Titulo, @Autor, @Anio, @Descripcion, @ImagenUrl)",
                new
                {
                    Libro.Titulo,
                    Libro.Autor,
                    Libro.Anio,
                    Libro.Descripcion,
                    ImagenUrl = rutaImagen
                }
            );

            return RedirectToPage("Index");
        }
    }
}
