using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblix.Pages;

public class IndexModel : PageModel
{
    public string Usuario { get; set; }

    
    public bool UsuarioLogeado;

    public void OnGet()
    {
        if (UsuarioLogeado)
        {
            Usuario = "UsuarioLogeado";
        }
    }
}