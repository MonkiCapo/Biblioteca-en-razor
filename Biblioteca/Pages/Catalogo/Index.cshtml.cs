using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Dapper;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Catalogo
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;

        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public List<CatalogoLibro> Catalogo { get; set; }

        public async Task OnGet()
        {
            using var con = Db.GetConnection(_config);

            Catalogo = (await con.QueryAsync<CatalogoLibro>(
                "SELECT * FROM CatalogoLibro"
            )).ToList();

            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
                return;

            var userId = int.Parse(claim);

            // Traemos los títulos que ya tiene el usuario
            var librosUsuario = (await con.QueryAsync<string>(
                "SELECT Titulo FROM Libro WHERE UsuarioId=@UserId",
                new { UserId = userId }
            )).ToList();

            // Marcamos cuáles ya están en la biblioteca
            foreach (var libro in Catalogo)
            {
                if (librosUsuario.Contains(libro.Titulo))
                {
                    libro.YaEnMiBiblioteca = true;
                }
            }
        }


        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPost(int id)
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (claim == null)
                return RedirectToPage("/Login");

            var userId = int.Parse(claim);

            using var con = Db.GetConnection(_config);

            // 1️⃣ Verificar que el usuario exista
            var usuarioExiste = await con.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Usuario WHERE Id=@Id",
                new { Id = userId }
            );

            if (usuarioExiste == 0)
                return RedirectToPage("/Login");

            // 2️⃣ Traer libro del catálogo
            var libro = await con.QueryFirstOrDefaultAsync<CatalogoLibro>(
                "SELECT * FROM CatalogoLibro WHERE Id=@Id",
                new { Id = id }
            );

            if (libro == null)
                return RedirectToPage();

            // 3️⃣ ✅ VALIDAR SI EL USUARIO YA TIENE ESE LIBRO
            var yaExiste = await con.ExecuteScalarAsync<int>(
                @"SELECT COUNT(*) 
                FROM Libro 
                WHERE UsuarioId = @UsuarioId 
                    AND Titulo = @Titulo 
                    AND Autor = @Autor",
                new
                {
                    UsuarioId = userId,
                    libro.Titulo,
                    libro.Autor
                }
            );

            if (yaExiste > 0)
            {
                // Ya lo tiene → no se inserta
                TempData["Error"] = "Ya tienes este libro en tu biblioteca.";
                return RedirectToPage();
            }

            // 4️⃣ Insertar el libro solo si no existía
            await con.ExecuteAsync(
                    @"INSERT INTO Libro 
                    (Titulo, Autor, Anio, Descripcion, ImagenUrl, UsuarioId)
                    VALUES (@Titulo,@Autor,@Anio,@Descripcion,@ImagenUrl,@UsuarioId)",
                new
                {
                    libro.Titulo,
                    libro.Autor,
                    libro.Anio,
                    libro.Descripcion,
                    libro.ImagenUrl, // ✅ SE COPIA
                    UsuarioId = userId
                });

        return RedirectToPage("/Libros/Index");
        }
    }
}
