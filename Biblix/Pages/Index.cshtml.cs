using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblix.Pages;

public class IndexModel : PageModel
{
    public string Usuario { get; set; }

    public void OnGet()
    {
        if (TempData["UsuarioLogueado"] != null)
        {
            Usuario = TempData["UsuarioLogueado"].ToString();

            // Mantener TempData viva para otros requests
            TempData.Keep("UsuarioLogueado");
        }
    }
}