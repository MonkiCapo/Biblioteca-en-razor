using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;

namespace Biblix.Pages;

public class IndexModel : PageModel
{
    public string Usuario { get; set; }

    public void OnGet()
    {
         Usuario = HttpContext.Session.GetString("Usuario");
    }
}