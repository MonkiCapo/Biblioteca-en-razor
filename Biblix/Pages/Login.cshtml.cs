using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Biblix.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Usuario { get; set; }

        [BindProperty]
        public string Contraseña { get; set; }

        public string MensajeError { get; set; }

        [BindProperty]
        public bool UsuarioLogeado { get; set; } =false;

        public void OnGet()
        {
            MensajeError = "";
        }

        public IActionResult OnPost()
        {
            // Usuario y contraseña "duros" (de prueba)
            if (Usuario == "admin" && Contraseña == "1234")
            {
                UsuarioLogeado = true;
                return RedirectToPage("Libros");
            }

            MensajeError = "Usuario o contraseña incorrectos";
            return Page();
        }
    }
}