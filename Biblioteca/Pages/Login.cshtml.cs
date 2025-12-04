using Dapper;
using Biblioteca.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class LoginModel : PageModel
{
    private readonly IConfiguration _config;

    public LoginModel(IConfiguration config)
    {
        _config = config;
    }

    [BindProperty] public string Email { get; set; }
    [BindProperty] public string Password { get; set; }
    public string Error { get; set; }

    [ValidateAntiForgeryToken]
    public async Task<IActionResult> OnPost()
    {
        using var con = Db.GetConnection(_config);

        var passHash = Convert.ToHexString(
            SHA256.HashData(Encoding.UTF8.GetBytes(Password))
        );

        var user = await con.QueryFirstOrDefaultAsync<dynamic>(
            @"SELECT u.Id, u.Nombre, r.Nombre AS Rol
              FROM Usuario u
              JOIN Rol r ON u.RolId = r.Id
              WHERE u.Email=@Email AND u.PasswordHash=@Hash",
            new { Email, Hash = passHash }
        );

        if (user == null)
        {
            Error = "Credenciales incorrectas.";
            return Page();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Nombre),
            new Claim(ClaimTypes.Role, user.Rol)
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identity));

        return RedirectToPage("/Index");
    }
}
