using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace ProyectoVie.Pages.VieWeb
{
    public class ConsultarPropuestaModel : PageModel
    {
        [BindProperty]
        public Proposal Proposal { get; set; } // Representa los datos de la propuesta

        public async Task OnGetAsync(int id)
        {
            // Aquí puedes obtener los datos de la propuesta según el ID
            // Suponiendo que tienes un servicio para obtener los datos
            Proposal = await GetProposalByIdAsync(id);
        }

        private async Task<Proposal> GetProposalByIdAsync(int id)
        {
            // Lógica para obtener la propuesta de una base de datos
            // Aquí se utilizaría tu contexto de datos o repositorio para obtener la información
            // Retornando un objeto de ejemplo por ahora
            return new Proposal
            {
                AgreementNumber = "12345",
                ApprovalDate = "2024-01-01",
                ProjectCondition = "Activo",
                ProjectName = "Proyecto de Ejemplo",
                ExtensionType = "Educativa",
                SustainableGoals = "Reducción de la pobreza",
                Extensionists = new List<Extensionist>
                {
                    new Extensionist { Name = "Juan Pérez", School = "Escuela de Ciencias", AppointmentType = "Profesor", Condition = "Activo", ParticipationPeriod = "2024" }
                },
                Organizations = new List<Organization>
                {
                    new Organization { ContactName = "María López", Institution = "Organización Ejemplo", ContactInfo = "maria@example.com" }
                },
                ProposalDescription = "Esta es una descripción breve de la propuesta de extensión.",
                PdfDocumentPath = "path/to/document.pdf"
            };
        }
    }

    public class Proposal
    {
        public string AgreementNumber { get; set; }
        public string ApprovalDate { get; set; }
        public string ProjectCondition { get; set; }
        public string ProjectName { get; set; }
        public string ExtensionType { get; set; }
        public string SustainableGoals { get; set; }
        public List<Extensionist> Extensionists { get; set; }
        public List<Organization> Organizations { get; set; }
        public string ProposalDescription { get; set; }
        public string PdfDocumentPath { get; set; }
    }

    public class Extensionist
    {
        public string Name { get; set; }
        public string School { get; set; }
        public string AppointmentType { get; set; }
        public string Condition { get; set; }
        public string ParticipationPeriod { get; set; }
    }

    public class Organization
    {
        public string ContactName { get; set; }
        public string Institution { get; set; }
        public string ContactInfo { get; set; }
    }
}
