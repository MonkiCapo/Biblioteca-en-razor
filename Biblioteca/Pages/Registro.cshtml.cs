using Dapper;
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

public class RegistroModel : PageModel
{
    private readonly IConfiguration _config;

    public RegistroModel(IConfiguration config)
    {
        _config = config;
    }

    [BindProperty] public string Nombre { get; set; }
    [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }
    public string Error { get; set; }

    public async Task<IActionResult> OnPost()
    {
        using var con = Db.GetConnection(_config);

        var passHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(Password))
        );

        try
        {
            var sql = @"INSERT INTO Usuario 
                (Nombre, Email, PasswordHash, RolId) 
                VALUES (@Nombre, @Email, @Pass, @RolId)";

            await con.ExecuteAsync(sql, new
            {
                Nombre,
                Email,
                Pass = passHash,
                RolId = 2 // Usuario común
            });

            return RedirectToPage("/Login");
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR REGISTRO: " + ex.Message);

            if (ex.Message.Contains("Duplicate"))
                Error = "El correo ya está registrado.";
            else
                Error = "Error interno al registrar usuario.";

            return Page();
        }
    }

}
