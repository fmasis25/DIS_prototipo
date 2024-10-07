using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ProyectoVie.Pages.VieWeb
{
    public class ModificarPropuestaModel : PageModel
    {
        [BindProperty]
        public string NumeroAcuerdo { get; set; }

        [BindProperty]
        public string FechaAprobacion { get; set; }

        [BindProperty]
        public string CondicionProyecto { get; set; }

        [BindProperty]
        public string NombreProyecto { get; set; }

        [BindProperty]
        public string TipoExtension { get; set; }

        [BindProperty]
        public string ObjetivosSostenibles { get; set; }

        [BindProperty]
        public string DescripcionPropuesta { get; set; }

        public async Task OnGetAsync()
        {
            // Aquí puedes cargar los datos existentes desde la base de datos.
            NumeroAcuerdo = "12345"; // Carga desde base de datos
            FechaAprobacion = "2024-01-01"; // Carga desde base de datos
            CondicionProyecto = "Activo"; // Carga desde base de datos
            NombreProyecto = "Proyecto de Ejemplo"; // Carga desde base de datos
            TipoExtension = "Educativa"; // Carga desde base de datos
            ObjetivosSostenibles = "Reducción de la pobreza"; // Carga desde base de datos
            DescripcionPropuesta = "Esta es una descripción breve de la propuesta de extensión."; // Carga desde base de datos
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            return RedirectToPage("Exito"); // Cambia "Exito" por el nombre de tu página de éxito
        }
    }
}
