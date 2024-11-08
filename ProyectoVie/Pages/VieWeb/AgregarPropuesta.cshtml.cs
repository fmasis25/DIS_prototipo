using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Linq;

namespace ProyectoVie.Pages.VieWeb
{
    public class AgregarPropuestaModel : PageModel
    {
        // Campos de la tabla Propuesta: Modelo
        [BindProperty]
        public string NombreProyecto { get; set; }

        [BindProperty]
        public string DeclaracionFinal { get; set; }

        [BindProperty]
        public string IdentificacionAcuerdo { get; set; }

        [BindProperty]
        public string TipoExtension { get; set; }

        [BindProperty]
        public string Descripcion { get; set; }

        [BindProperty]
        public string ObjetivoDesarrolloSostenible { get; set; } // ODS ComboBox

        [BindProperty]
        public string Escuela { get; set; } // Escuela ComboBox

        [BindProperty]
        public string Introduccion { get; set; } // Nuevo campo

        [BindProperty]
        public string PerfilesProfesionales { get; set; } // Nuevo campo

        [BindProperty]
        public string ParticipacionEstudiantil { get; set; } // Nuevo campo

        [BindProperty]
        public string RiesgosCumplimiento { get; set; } // Nuevo campo

        [BindProperty]
        public string ViabilidadFinanciera { get; set; } // Nuevo campo

        [BindProperty]
        public string ImpactoSocial { get; set; } // Nuevo campo

        [BindProperty]
        public string ImpactoAcademico { get; set; } // Nuevo campo

        [BindProperty]
        public DateTime FechaAprobacion { get; set; }

        [BindProperty]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [BindProperty]
        public int ID_UsuarioPromotor { get; set; }

        [BindProperty]
        public int ID_Estado { get; set; } = 2;

        [BindProperty]
        public string RutaArchivo { get; set; }

        [BindProperty]
        public string NombreArchivo { get; set; }

        [BindProperty]
        public IFormFile PdfFile { get; set; }

        public string ErrorMessage { get; set; }

        [BindProperty]
        public List<Extensionist> Extensionists { get; set; } = new List<Extensionist>();

        [BindProperty]
        public List<Organization> Organizations { get; set; } = new List<Organization>();

        public List<string> ExtensionistErrorMessages { get; set; } = new List<string>();
        public List<string> OrganizationErrorMessages { get; set; } = new List<string>();

        public async Task<IActionResult> OnPostAsync()
        {
            Debug.WriteLine("OnPostAsync ha comenzado");

            ID_UsuarioPromotor = HttpContext.Session.GetInt32("UserID") ?? 0;

            if (PdfFile != null && PdfFile.Length > 0)
            {
                Debug.WriteLine("Archivo PDF detectado");

                string pdfDirectory = Path.Combine("Pages", "PDF");
                RutaArchivo = Path.Combine(pdfDirectory, PdfFile.FileName);

                try
                {
                    Directory.CreateDirectory(pdfDirectory);

                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), RutaArchivo), FileMode.Create))
                    {
                        await PdfFile.CopyToAsync(stream);
                    }

                    using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                    {
                        PrintModel();
                        connection.Open();

                        // Comienza a ejecutar el procedimiento almacenado para agregar la propuesta
                        try
                        {
                            using (SqlCommand command = new SqlCommand("AgregarPropuestas", connection))
                            {
                                command.CommandType = CommandType.StoredProcedure;

                                // Agregar parámetros
                                command.Parameters.AddWithValue("@InNombrePropuesta", NombreProyecto);
                                command.Parameters.AddWithValue("@InIdentificacionAcuerdo", IdentificacionAcuerdo);
                                command.Parameters.AddWithValue("@InDescripcion", Descripcion);
                                command.Parameters.AddWithValue("@InTipoExtension", TipoExtension);
                                command.Parameters.AddWithValue("@InODS", ObjetivoDesarrolloSostenible);
                                command.Parameters.AddWithValue("@InDeclaracionFinal", DeclaracionFinal);
                                command.Parameters.AddWithValue("@InFechaAprobacion", FechaAprobacion);
                                command.Parameters.AddWithValue("@InFechaSolicitud", FechaSolicitud);
                                command.Parameters.AddWithValue("@InID_UsuarioPromotor", ID_UsuarioPromotor);
                                command.Parameters.AddWithValue("@InID_Estado", ID_Estado);
                                command.Parameters.AddWithValue("@InRutaArchivo", RutaArchivo);
                                command.Parameters.AddWithValue("@InNombreArchivo", PdfFile.FileName);
                                command.Parameters.AddWithValue("@InEscuela", Escuela);
                                command.Parameters.AddWithValue("@InIntroduccion", Introduccion);
                                command.Parameters.AddWithValue("@InPerfilesProfesionales", PerfilesProfesionales);
                                command.Parameters.AddWithValue("@InParticipacionEstudiantil", ParticipacionEstudiantil);
                                command.Parameters.AddWithValue("@InRiesgosCumplimiento", RiesgosCumplimiento);
                                command.Parameters.AddWithValue("@InViabilidadFinanciera", ViabilidadFinanciera);
                                command.Parameters.AddWithValue("@InImpactoSocial", ImpactoSocial);
                                command.Parameters.AddWithValue("@InImpactoAcademico", ImpactoAcademico);



                                SqlParameter outIdParam = new SqlParameter("@OutID_Propuesta", SqlDbType.Int)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                command.Parameters.Add(outIdParam);

                                await command.ExecuteNonQueryAsync();
                                int idPropuesta = (int)outIdParam.Value;
                                Debug.WriteLine($"ID_propuesta generado: {idPropuesta}");

                                // Insertar extensionistas
                                if (Extensionists != null && Extensionists.Count > 0)
                                {
                                    foreach (var extensionist in Extensionists)
                                    {
                                        try
                                        {
                                            using (SqlCommand cmdExtensionist = new SqlCommand("InsertarGrupoExtensionistas", connection))
                                            {
                                                cmdExtensionist.CommandType = CommandType.StoredProcedure;
                                                cmdExtensionist.Parameters.AddWithValue("@Nombre", extensionist.Nombre);
                                                cmdExtensionist.Parameters.AddWithValue("@Apellido", extensionist.Apellido);
                                                cmdExtensionist.Parameters.AddWithValue("@ID_propuesta", idPropuesta);
                                                cmdExtensionist.Parameters.AddWithValue("@Condicion", extensionist.Condicion);
                                                cmdExtensionist.Parameters.AddWithValue("@Nombramiento", extensionist.TipoNombramiento);
                                                cmdExtensionist.Parameters.AddWithValue("@FechaInicio", extensionist.FechaInicio);
                                                cmdExtensionist.Parameters.AddWithValue("@FechaFin", extensionist.FechaFin);
                                                await cmdExtensionist.ExecuteNonQueryAsync();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ExtensionistErrorMessages.Add($"Error al insertar extensionista {extensionist.Nombre}: {ex.Message}");
                                            Debug.WriteLine($"Error al insertar extensionista {extensionist.Nombre}: {ex.Message}");
                                        }
                                    }
                                }

                                // Insertar organizaciones
                                if (Organizations != null && Organizations.Count > 0)
                                {
                                    foreach (var organization in Organizations)
                                    {
                                        try
                                        {
                                            using (SqlCommand cmdOrganization = new SqlCommand("AgregarInstitucion", connection))
                                            {
                                                cmdOrganization.CommandType = CommandType.StoredProcedure;
                                                cmdOrganization.Parameters.AddWithValue("@NombreInstitucion", organization.Organizacion);
                                                cmdOrganization.Parameters.AddWithValue("@Contacto", organization.Contacto);
                                                cmdOrganization.Parameters.AddWithValue("@ID_propuesta", idPropuesta);
                                                await cmdOrganization.ExecuteNonQueryAsync();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            OrganizationErrorMessages.Add($"Error al insertar organización {organization.Organizacion}: {ex.Message}");
                                            Debug.WriteLine($"Error al insertar organización {organization.Organizacion}: {ex.Message}");
                                        }
                                    }
                                }
                            }
                        }
                        catch (SqlException ex)
                        {
                            // Manejo de errores de SQL al agregar la propuesta
                            Debug.WriteLine($"Error de SQL: {ex.Message}");
                            foreach (SqlError error in ex.Errors)
                            {
                                Debug.WriteLine($"Código de Error: {error.Number}, Mensaje: {error.Message}, Procedimiento: {error.Procedure}");
                            }
                            ErrorMessage = $"Error al agregar la propuesta: {ex.Message}";
                            return Page();
                        }
                        catch (Exception ex)
                        {
                            // Manejo de errores generales
                            Debug.WriteLine($"Error general: {ex.Message}");
                            ErrorMessage = $"Error al agregar la propuesta: {ex.Message}";
                            return Page();
                        }
                    }

                    // Si hay errores al agregar extensionistas o organizaciones
                    if (ExtensionistErrorMessages.Count > 0 || OrganizationErrorMessages.Count > 0)
                    {
                        ErrorMessage = "Errores al agregar algunos extensionistas u organizaciones.";
                        return Page();
                    }

                    // Redirigir a otra página si todo fue exitoso
                    return RedirectToPage("/VieWeb/MenuExtensionista");
                }
                catch (Exception ex)
                {
                    // Manejo de errores al subir el archivo PDF
                    ErrorMessage = $"Error al agregar la propuesta: {ex.Message}";
                    Debug.WriteLine($"Error al subir el archivo PDF: {ex.Message}");
                    return Page();
                }
            }
            else
            {
                ErrorMessage = "Por favor, proporcione la ruta y el nombre del archivo.";
                return Page();
            }
        }

        private void PrintModel()
        {
            try
            {
                Debug.WriteLine("NombrePropuesta: " + NombreProyecto);
                Debug.WriteLine("IdentificacionAcuerdo: " + IdentificacionAcuerdo);
                Debug.WriteLine("TipoExtension: " + TipoExtension);
                Debug.WriteLine("Descripcion: " + Descripcion);
                Debug.WriteLine("ODS: " + ObjetivoDesarrolloSostenible);
                Debug.WriteLine("Escuela: " + Escuela);
                Debug.WriteLine("Introduccion: " + Introduccion);
                Debug.WriteLine("PerfilesProfesionales: " + PerfilesProfesionales);
                Debug.WriteLine("ParticipacionEstudiantil: " + ParticipacionEstudiantil);
                Debug.WriteLine("RiesgosCumplimiento: " + RiesgosCumplimiento);
                Debug.WriteLine("ViabilidadFinanciera: " + ViabilidadFinanciera);
                Debug.WriteLine("ImpactoSocial: " + ImpactoSocial);
                Debug.WriteLine("ImpactoAcademico: " + ImpactoAcademico);
                Debug.WriteLine("FechaAprobacion: " + FechaAprobacion);
                Debug.WriteLine("FechaSolicitud: " + FechaSolicitud);
                Debug.WriteLine("DeclaracionFinal: " + DeclaracionFinal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error en PrintModel: " + ex.Message);
            }
        }
    }

    public class Extensionist
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Condicion { get; set; }
        public string TipoNombramiento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class Organization
    {
        public string Organizacion { get; set; }
        public string Contacto { get; set; }
    }
}
