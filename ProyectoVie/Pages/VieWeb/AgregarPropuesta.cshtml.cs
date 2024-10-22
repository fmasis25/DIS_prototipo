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
        public string IdentificacionAcuerdo { get; set; }

        [BindProperty]
        public string TipoExtension { get; set; }

        [BindProperty]
        public string Descripcion { get; set; }

        [BindProperty]
        public string ObjetivoDesarrolloSostenible { get; set; }

        [BindProperty]
        public string DeclaracionFinal { get; set; }

        [BindProperty]
        public DateTime FechaAprobacion { get; set; }

        [BindProperty]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now; // Valor por defecto

        [BindProperty]
        public int ID_UsuarioPromotor { get; set; }

        [BindProperty]
        public int ID_Estado { get; set; } = 2;

        [BindProperty]
        public string RutaArchivo { get; set; } // Ruta del archivo como string

        [BindProperty]
        public string NombreArchivo { get; set; } // Nombre del archivo

        // Nuevo atributo para manejar la carga del archivo
        [BindProperty]
        public IFormFile PdfFile { get; set; } // Archivo PDF

        public string ErrorMessage { get; set; }

        // Nueva propiedad para el grupo extensionista
        [BindProperty]
        public List<Extensionist> Extensionists { get; set; } = new List<Extensionist>();

        // Nueva propiedad para el grupo de organizaciones
        [BindProperty]
        public List<Organization> Organizations { get; set; } = new List<Organization>();

        public List<string> ExtensionistErrorMessages { get; set; } = new List<string>(); // Lista para mensajes de error de extensionistas
        public List<string> OrganizationErrorMessages { get; set; } = new List<string>(); // Lista para mensajes de error de organizaciones

        public async Task<IActionResult> OnPostAsync()
        {
            Debug.WriteLine("OnPostAsync ha comenzado");

            ID_UsuarioPromotor = HttpContext.Session.GetInt32("UserID") ?? 0; // Obtener el ID del usuario de la sesión

            Debug.WriteLine("ID_UsuarioPromotor: " + ID_UsuarioPromotor);

            Debug.WriteLine("Modelo es válido, procesando datos...");

            // Guardar el archivo PDF en la ruta especificada
            if (PdfFile != null && PdfFile.Length > 0)
            {
                Debug.WriteLine("Archivo PDF detectado");
                PrintModel();

                // Define la ruta relativa donde se guardará el archivo
                string pdfDirectory = Path.Combine("Pages", "PDF");
                RutaArchivo = Path.Combine(pdfDirectory, PdfFile.FileName); // Ajusta según tu estructura de carpetas

                try
                {
                    // Asegúrate de que la carpeta existe
                    Directory.CreateDirectory(pdfDirectory);

                    // Guarda el archivo en la ruta especificada
                    using (var stream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), RutaArchivo), FileMode.Create))
                    {
                        await PdfFile.CopyToAsync(stream);
                    }

                    // Conexión a la base de datos
                    using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                    {
                        PrintModel();
                        connection.Open();

                        // Agregar el comando para llamar al procedimiento almacenado
                        using (SqlCommand command = new SqlCommand("AgregarPropuestas", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;

                            // Agrega los parámetros a la consulta
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

                            // Agregar el parámetro de salida
                            SqlParameter outIdParam = new SqlParameter("@OutID_Propuesta", SqlDbType.Int)
                            {
                                Direction = ParameterDirection.Output
                            };
                            command.Parameters.Add(outIdParam);

                            // Ejecutar el comando
                            await command.ExecuteNonQueryAsync();

                            // Obtener el ID de la propuesta generada
                            int idPropuesta = (int)outIdParam.Value; // Declara y asigna idPropuesta aquí
                            Debug.WriteLine($"ID_propuesta generado: {idPropuesta}");

                            // ** Insertar cada Extensionista utilizando el ID_propuesta obtenido **
                            if (Extensionists != null && Extensionists.Count > 0)
                            {
                                Debug.WriteLine($"Se encontraron {Extensionists.Count} extensionistas para insertar.");
                                foreach (var extensionist in Extensionists)
                                {
                                    try
                                    {
                                        using (SqlCommand cmdExtensionist = new SqlCommand("InsertarGrupoExtensionistas", connection))
                                        {
                                            cmdExtensionist.CommandType = CommandType.StoredProcedure;

                                            // Agrega los parámetros para la inserción de extensionistas
                                            cmdExtensionist.Parameters.AddWithValue("@Nombre", extensionist.Nombre);
                                            cmdExtensionist.Parameters.AddWithValue("@Apellido", extensionist.Apellido);
                                            cmdExtensionist.Parameters.AddWithValue("@ID_propuesta", idPropuesta); // Usa el ID generado
                                            cmdExtensionist.Parameters.AddWithValue("@Condicion", extensionist.Condicion);
                                            cmdExtensionist.Parameters.AddWithValue("@Nombramiento", extensionist.TipoNombramiento);
                                            cmdExtensionist.Parameters.AddWithValue("@FechaInicio", extensionist.FechaInicio);
                                            cmdExtensionist.Parameters.AddWithValue("@FechaFin", extensionist.FechaFin);

                                            Debug.WriteLine($"Insertando extensionista: {extensionist.Nombre} {extensionist.Apellido}");

                                            await cmdExtensionist.ExecuteNonQueryAsync();
                                            Debug.WriteLine($"Extensionista {extensionist.Nombre} {extensionist.Apellido} insertado correctamente.");
                                        }
                                    }
                                    catch (SqlException sqlEx)
                                    {
                                        string errorMessage = $"Error al insertar extensionista {extensionist.Nombre} {extensionist.Apellido}: {sqlEx.Message}";
                                        ExtensionistErrorMessages.Add(errorMessage);
                                        Debug.WriteLine(errorMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorMessage = $"Error inesperado al insertar extensionista {extensionist.Nombre} {extensionist.Apellido}: {ex.Message}";
                                        ExtensionistErrorMessages.Add(errorMessage);
                                        Debug.WriteLine(errorMessage);
                                    }
                                }
                            }
                            else
                            {
                                Debug.WriteLine("No se encontraron extensionistas para insertar.");
                            }

                            // ** Insertar cada Organización utilizando el ID_propuesta obtenido **
                            if (Organizations != null && Organizations.Count > 0)
                            {
                                Debug.WriteLine($"Se encontraron {Organizations.Count} organizaciones para insertar.");
                                foreach (var organization in Organizations)
                                {
                                    try
                                    {
                                        using (SqlCommand cmdOrganization = new SqlCommand("AgregarInstitucion", connection))
                                        {
                                            cmdOrganization.CommandType = CommandType.StoredProcedure;

                                            // Agrega los parámetros para la inserción de organizaciones
                                            cmdOrganization.Parameters.AddWithValue("@NombreInstitucion", organization.Organizacion);
                                            cmdOrganization.Parameters.AddWithValue("@Contacto", organization.Contacto);
                                            cmdOrganization.Parameters.AddWithValue("@ID_propuesta", idPropuesta); // Usa el ID generado

                                            Debug.WriteLine($"Insertando organización: {organization.Organizacion}");

                                            await cmdOrganization.ExecuteNonQueryAsync();
                                            Debug.WriteLine($"Organización {organization.Organizacion} insertada correctamente.");
                                        }
                                    }
                                    catch (SqlException sqlEx)
                                    {
                                        string errorMessage = $"Error al insertar organización {organization.Organizacion}: {sqlEx.Message}";
                                        OrganizationErrorMessages.Add(errorMessage);
                                        Debug.WriteLine(errorMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorMessage = $"Error inesperado al insertar organización {organization.Organizacion}: {ex.Message}";
                                        OrganizationErrorMessages.Add(errorMessage);
                                        Debug.WriteLine(errorMessage);
                                    }
                                }
                            }
                            else
                            {
                                Debug.WriteLine("No se encontraron organizaciones para insertar.");
                            }
                        }
                    }

                    if (ExtensionistErrorMessages.Count > 0 || OrganizationErrorMessages.Count > 0)
                    {
                        ErrorMessage = "Se encontraron errores al agregar algunos extensionistas u organizaciones: " +
                            string.Join(", ", ExtensionistErrorMessages) +
                            string.Join(", ", OrganizationErrorMessages);
                        return Page();
                    }

                    return RedirectToPage("/VieWeb/MenuExtensionista");
                }
                catch (Exception ex)
                {
                    ErrorMessage = $"Error al agregar la propuesta: {ex.Message}";
                    Debug.WriteLine(ErrorMessage);
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
                Debug.WriteLine("DeclaracionFinal: " + DeclaracionFinal);
                Debug.WriteLine("FechaAprobacion: " + FechaAprobacion);
                Debug.WriteLine("FechaSolicitud: " + FechaSolicitud);
                Debug.WriteLine("ID_UsuarioPromotor: " + ID_UsuarioPromotor);
                Debug.WriteLine("ID_Estado: " + ID_Estado);
                Debug.WriteLine("RutaArchivo: " + RutaArchivo);
                Debug.WriteLine("NombreArchivo: " + NombreArchivo);

                // Imprimir extensionists
                if (Extensionists != null)
                {
                    for (int i = 0; i < Extensionists.Count; i++)
                    {
                        Debug.WriteLine($"Extensionist {i + 1}:");
                        Debug.WriteLine($"  Nombre: {Extensionists[i].Nombre}");
                        Debug.WriteLine($"  Apellido: {Extensionists[i].Apellido}");
                        Debug.WriteLine($"  Condicion: {Extensionists[i].Condicion}");
                        Debug.WriteLine($"  Nombramiento: {Extensionists[i].TipoNombramiento}");
                        Debug.WriteLine($"  FechaInicio: {Extensionists[i].FechaInicio}");
                        Debug.WriteLine($"  FechaFin: {Extensionists[i].FechaFin}");
                    }
                }

                // Imprimir organizaciones
                if (Organizations != null)
                {
                    for (int i = 0; i < Organizations.Count; i++)
                    {
                        Debug.WriteLine($"Organización {i + 1}:");
                        Debug.WriteLine($"  Nombre: {Organizations[i].Organizacion}");
                        Debug.WriteLine($"  Contacto: {Organizations[i].Contacto}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al imprimir el modelo: " + ex.Message);
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
}
