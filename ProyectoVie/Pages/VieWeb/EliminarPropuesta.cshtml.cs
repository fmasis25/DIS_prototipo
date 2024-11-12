using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
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
            public string RutaArchivo { get; set; } // Ruta al archivo PDF
            public string UrlArchivo { get; set; } // URL completa del archivo PDF
            public string Escuela { get; set; }
            public string Introduccion { get; set; }
            public string PerfilesProfesionales { get; set; }
            public string ParticipacionEstudiantil { get; set; }
            public string RiesgosCumplimiento { get; set; }
            public string ViabilidadFinanciera { get; set; }
            public string ImpactoSocial { get; set; }
            public string ImpactoAcademico { get; set; }
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
                        // Leer los detalles de la propuesta (primer conjunto de resultados)
                        if (await reader.ReadAsync())
                        {
                            Propuesta.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                            Propuesta.NombreProyecto = reader.GetString(reader.GetOrdinal("NombrePropuesta"));
                            Propuesta.IdentificacionAcuerdo = reader.GetString(reader.GetOrdinal("IdentificacionAcuerdo"));
                            Propuesta.Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                            Propuesta.TipoExtension = reader.GetString(reader.GetOrdinal("TipoExtension"));
                            Propuesta.ObjetivoDesarrolloSostenible = reader.GetString(reader.GetOrdinal("ODS"));
                            Propuesta.DeclaracionFinal = reader.GetString(reader.GetOrdinal("DeclaracionFinal"));
                            Propuesta.FechaAprobacion = reader.GetDateTime(reader.GetOrdinal("FechaAprobacion"));
                            Propuesta.FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud"));
                            Propuesta.RutaArchivo = reader.GetString(reader.GetOrdinal("RutaArchivo"));
                            Propuesta.Escuela = reader.GetString(reader.GetOrdinal("Escuela"));
                            Propuesta.Introduccion = reader.GetString(reader.GetOrdinal("Introduccion"));
                            Propuesta.PerfilesProfesionales = reader.GetString(reader.GetOrdinal("PerfilesProfesionales"));
                            Propuesta.ParticipacionEstudiantil = reader.GetString(reader.GetOrdinal("ParticipacionEstudiantil"));
                            Propuesta.RiesgosCumplimiento = reader.GetString(reader.GetOrdinal("RiesgosCumplimiento"));
                            Propuesta.ViabilidadFinanciera = reader.GetString(reader.GetOrdinal("ViabilidadFinanciera"));
                            Propuesta.ImpactoSocial = reader.GetString(reader.GetOrdinal("ImpactoSocial"));
                            Propuesta.ImpactoAcademico = reader.GetString(reader.GetOrdinal("ImpactoAcademico"));

                            // Ajustar la RutaArchivo para la URL de GitHub
                            Propuesta.RutaArchivo = Propuesta.RutaArchivo.Replace(@"Pages\PDF\", ""); // Eliminar 'Pages/PDF/' de la ruta
                            Propuesta.UrlArchivo = $"https://raw.githubusercontent.com/fmasis25/DIS_prototipo/main/ProyectoVie/Pages/PDF/{Propuesta.RutaArchivo}";

                            Debug.WriteLine($"Propuesta cargada: {Propuesta.NombreProyecto}, ID: {Propuesta.Id}, URL: {Propuesta.UrlArchivo}");
                        }
                        else
                        {
                            Debug.WriteLine($"No se encontraron detalles para la propuesta con IdPropuesta: {propuestaId}");
                        }

                        // Mover al segundo conjunto de resultados: Extensionistas
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var extensionist = new Extensionist
                                {
                                    Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                    Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                                    TipoNombramiento = reader.GetString(reader.GetOrdinal("Nombramiento")),
                                    Condicion = reader.GetString(reader.GetOrdinal("Condicion")),
                                    FechaInicio = reader.GetDateTime(reader.GetOrdinal("FechaInicio")),
                                    FechaFin = reader.GetDateTime(reader.GetOrdinal("FechaFin"))
                                };
                                Extensionists.Add(extensionist);
                                Debug.WriteLine($"Extensionista añadido: {extensionist.Nombre} {extensionist.Apellido}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"No se encontraron extensionistas para la propuesta con IdPropuesta: {propuestaId}");
                        }

                        // Mover al tercer conjunto de resultados: Organizaciones
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var organization = new Organization
                                {
                                    Organizacion = reader.GetString(reader.GetOrdinal("NombreInstitucion")),
                                    Contacto = reader.GetString(reader.GetOrdinal("Contacto"))
                                };
                                Organizations.Add(organization);
                                Debug.WriteLine($"Organización añadida: {organization.Organizacion}");
                            }
                        }
                        else
                        {
                            Debug.WriteLine($"No se encontraron organizaciones para la propuesta con IdPropuesta: {propuestaId}");
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostEliminarPropuestaAsync(int Id)
        {
            Debug.WriteLine("Entrando en OnPostEliminarPropuestaAsync");

            if (Id <= 0)
            {
                Debug.WriteLine("ID de propuesta no válido.");
                return Page();
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
                        return RedirectToPage("/VieWeb/MenuExtensionista");
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
