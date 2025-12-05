using Biblioteca.Data;
using Biblioteca.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Biblioteca.Pages.Catalogo
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public EditarModel(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        [BindProperty]
        public CatalogoLibro Libro { get; set; }

        [BindProperty]
        public IFormFile Imagen { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            if (!User.IsInRole("Admin"))
                return RedirectToPage("/Catalogo/Index");

            using var con = Db.GetConnection(_config);

            Libro = await con.QueryFirstOrDefaultAsync<CatalogoLibro>(
                "SELECT * FROM CatalogoLibro WHERE Id=@id",
                new { id });

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!User.IsInRole("Admin"))
                return RedirectToPage("/Catalogo/Index");

            using var con = Db.GetConnection(_config);

            // Traer imagen actual
            var imagenActual = await con.QueryFirstOrDefaultAsync<string>(
                "SELECT ImagenUrl FROM CatalogoLibro WHERE Id=@Id",
                new { Libro.Id });

            string rutaNuevaImagen = imagenActual;

            // ✅ Si sube una nueva imagen, se reemplaza
            if (Imagen != null && Imagen.Length > 0)
            {
                // borrar imagen anterior
                if (!string.IsNullOrEmpty(imagenActual))
                {
                    var rutaFisica = Path.Combine(_env.WebRootPath, imagenActual.TrimStart('/'));
                    if (System.IO.File.Exists(rutaFisica))
                        System.IO.File.Delete(rutaFisica);
                }

                var ext = Path.GetExtension(Imagen.FileName);
                var nombre = Guid.NewGuid() + ext;
                var carpeta = Path.Combine(_env.WebRootPath, "img", "libros");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var nuevaRuta = Path.Combine(carpeta, nombre);

                using var stream = new FileStream(nuevaRuta, FileMode.Create);
                await Imagen.CopyToAsync(stream);

                rutaNuevaImagen = "/img/libros/" + nombre;
            }

            await con.ExecuteAsync(
                @"UPDATE CatalogoLibro 
                  SET Titulo=@Titulo, Autor=@Autor, Anio=@Anio, 
                      Descripcion=@Descripcion, ImagenUrl=@ImagenUrl
                  WHERE Id=@Id",
                new
                {
                    Libro.Titulo,
                    Libro.Autor,
                    Libro.Anio,
                    Libro.Descripcion,
                    ImagenUrl = rutaNuevaImagen,
                    Libro.Id
                });

            return RedirectToPage("Index");
        }
    }
}
