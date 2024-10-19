using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProyectoVie.Pages.VieWeb
{
    public class EliminarPropuestaModel : PageModel
    {
        public Proposal Propuesta { get; set; } = new Proposal();
        public List<Extensionist> Extensionists { get; set; } = new List<Extensionist>();
        public List<Organization> Organizations { get; set; } = new List<Organization>();

        public class Proposal
        {
            public int Id { get; set; }
            public string NombreProyecto { get; set; }
            public string IdentificacionAcuerdo { get; set; }
            public string TipoExtension { get; set; }
            public string Descripcion { get; set; }
            public string ObjetivoDesarrolloSostenible { get; set; }
            public string DeclaracionFinal { get; set; }
            public DateTime FechaAprobacion { get; set; }
            public DateTime FechaSolicitud { get; set; }
            public string RutaArchivo { get; set; }
            public string UrlArchivo { get; set; }
        }

        public class Extensionist
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string TipoNombramiento { get; set; }
            public string Condicion { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
        }

        public class Organization
        {
            public string Organizacion { get; set; }
            public string Contacto { get; set; }
        }

        public async Task OnGetAsync(int propuestaId)
        {
            await CargarDetallesPropuesta(propuestaId);
        }

        private async Task CargarDetallesPropuesta(int propuestaId)
        {
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Debug.WriteLine("Conexión a la base de datos abierta correctamente.");

                using (SqlCommand command = new SqlCommand("ObtenerPropuestaDetalles", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdPropuesta", propuestaId);

                    Debug.WriteLine($"Ejecutando el procedimiento almacenado con el IdPropuesta: {propuestaId}");

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        // Leer los detalles de la propuesta
                        if (await reader.ReadAsync())
                        {
                            Propuesta.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            Propuesta.NombreProyecto = reader.GetString(reader.GetOrdinal("NombrePropuesta"));
                            Propuesta.IdentificacionAcuerdo = reader.GetString(reader.GetOrdinal("IdentificacionAcuerdo"));
                            Propuesta.TipoExtension = reader.GetString(reader.GetOrdinal("TipoExtension"));
                            Propuesta.Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                            Propuesta.ObjetivoDesarrolloSostenible = reader.GetString(reader.GetOrdinal("ODS"));
                            Propuesta.DeclaracionFinal = reader.GetString(reader.GetOrdinal("DeclaracionFinal"));
                            Propuesta.FechaAprobacion = reader.GetDateTime(reader.GetOrdinal("FechaAprobacion"));
                            Propuesta.FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud"));
                            Propuesta.RutaArchivo = reader.GetString(reader.GetOrdinal("RutaArchivo"));

                            Debug.WriteLine($"Propuesta cargada: {Propuesta.NombreProyecto}, ID: {Propuesta.Id}");
                        }
                        else
                        {
                            Debug.WriteLine($"No se encontró propuesta con ID: {propuestaId}");
                        }
                    }
                }

                // Aquí puedes cargar los Extensionists y Organizations si es necesario
                // await CargarExtensionistasYOrganizaciones(propuestaId);
            }
        }

        public async Task<IActionResult> OnPostEliminarPropuestaAsync(int Id)
        {
            Debug.WriteLine("Entrando en OnPostEliminarPropuestaAsync"); // Para verificar si se llama al método

            if (Id <= 0)
            {
                Debug.WriteLine("ID de propuesta no válido."); // Agregar mensaje de depuración
                return Page(); // Regresa a la misma página si no es válido
            }

            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("EliminarPropuesta", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID_Propuesta", Id);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        Debug.WriteLine($"Propuesta con ID: {Id} eliminada correctamente.");
                        return RedirectToPage("/Propuestas"); // Redirigir después de la eliminación
                    }
                    catch (SqlException ex)
                    {
                        ModelState.AddModelError(string.Empty, $"Error al eliminar la propuesta: {ex.Message}");
                        Debug.WriteLine($"Error al eliminar la propuesta: {ex.Message}");
                        return Page();
                    }
                }
            }
        }
    }
}
