using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace ProyectoVie.Pages.VieWeb
{
    public class AgregarPropuestaModel : PageModel
    {
        [BindProperty]
        public Propuesta Proposal { get; set; } = new Propuesta();

        public class Propuesta
        {
            public string NumeroAcuerdo { get; set; }
            public DateTime FechaAprobacion { get; set; }
            public string NombreProyecto { get; set; }
            public string TipoExtension { get; set; }
            public string ObjetivoDesarrolloSostenible { get; set; }
            public string NombreExtensionista { get; set; }
            public string InstitucionAcademica { get; set; }
            public string TipoNombramiento { get; set; }
            public string Condicion { get; set; }
            public string PeriodoParticipacion { get; set; }
            public string NombreContacto { get; set; }
            public string Organizacion { get; set; }
            public string Contacto { get; set; }
            public string DescripcionPropuesta { get; set; }
        }

        public void OnGet()
        {
            // Inicializa el modelo si es necesario
            Proposal = new Propuesta
            {
                NumeroAcuerdo = "12-2017",
                FechaAprobacion = DateTime.Now
            };
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            return RedirectToPage("Index"); // Redirige a la página principal después de guardar
        }
    }
}
