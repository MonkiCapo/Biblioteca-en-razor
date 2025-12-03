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

        var passHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Password)));

        try
        {
            var sql = "INSERT INTO Usuario (Nombre, Email, PasswordHash) VALUES (@Nombre, @Email, @Pass)";
            await con.ExecuteAsync(sql, new { Nombre, Email, Pass = passHash });
            return RedirectToPage("/Login");
        }
        catch
        {
            Error = "El correo ya está registrado.";
            return Page();
        }
    }
}
