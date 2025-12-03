using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace Biblioteca.Pages;

public class IndexModel : PageModel
{
    public async Task<IActionResult> OnPostLogout()
    {
        await HttpContext.SignOutAsync("Cookies");
        return RedirectToPage("/Index");
    }
}
