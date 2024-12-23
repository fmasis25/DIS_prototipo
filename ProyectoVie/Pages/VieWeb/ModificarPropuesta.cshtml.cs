﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProyectoVie.Pages.VieWeb
{
    public class ModificarPropuestaModel : PageModel
    {
        public Proposal Propuesta { get; set; } = new Proposal();
        public List<Extensionist> Extensionists { get; set; } = new List<Extensionist>();
        public List<Organization> Organizations { get; set; } = new List<Organization>();

        // Propiedad para almacenar mensajes de error
        [TempData]
        public string ErrorMessage { get; set; }

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
            public string Introduccion { get; set; } // Nuevo campo
            public string PerfilesProfesionales { get; set; } // Nuevo campo
            public string ParticipacionEstudiantil { get; set; } // Nuevo campo
            public string RiesgosCumplimiento { get; set; } // Nuevo campo
            public string ViabilidadFinanciera { get; set; } // Nuevo campo
            public string ImpactoSocial { get; set; } // Nuevo campo
            public string ImpactoAcademico { get; set; } // Nuevo campo
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

            try
            {
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
                                Propuesta.TipoExtension = reader.GetString(reader.GetOrdinal("TipoExtension"));
                                Propuesta.Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"));
                                Propuesta.ObjetivoDesarrolloSostenible = reader.GetString(reader.GetOrdinal("ODS"));
                                Propuesta.DeclaracionFinal = reader.GetString(reader.GetOrdinal("DeclaracionFinal"));
                                Propuesta.FechaAprobacion = reader.GetDateTime(reader.GetOrdinal("FechaAprobacion"));
                                Propuesta.FechaSolicitud = reader.GetDateTime(reader.GetOrdinal("FechaSolicitud"));
                                Propuesta.RutaArchivo = reader.GetString(reader.GetOrdinal("RutaArchivo"));

                                // Nuevos campos
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
                                ErrorMessage = "No se encontraron detalles para la propuesta seleccionada.";
                                return; // No hacer nada más en caso de error
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
                                ErrorMessage = "No se encontraron extensionistas para la propuesta seleccionada.";
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
                                ErrorMessage = "No se encontraron organizaciones para la propuesta seleccionada.";
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error en la base de datos: {ex.Message}");
                ErrorMessage = "Ocurrió un error al intentar acceder a la base de datos.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inesperado: {ex.Message}");
                ErrorMessage = "Ocurrió un error inesperado.";
            }
        }

        // Método para manejar el envío del formulario
        public async Task<IActionResult> OnPostAsync(int propuestaId)
        {
            // Aquí se obtienen los datos del formulario
            Propuesta.Id = propuestaId; // Asegúrate de establecer el ID de la propuesta
            Propuesta.IdentificacionAcuerdo = Request.Form["IdentificacionAcuerdo"];
            Propuesta.FechaAprobacion = DateTime.Parse(Request.Form["FechaAprobacion"]);
            Propuesta.Descripcion = Request.Form["Descripcion"];
            Propuesta.NombreProyecto = Request.Form["NombreProyecto"];
            Propuesta.TipoExtension = Request.Form["TipoExtension"];
            Propuesta.ObjetivoDesarrolloSostenible = Request.Form["ObjetivoDesarrolloSostenible"];
            Propuesta.DeclaracionFinal = Request.Form["DeclaracionFinal"];

            // Nuevos campos
            Propuesta.Introduccion = Request.Form["Introduccion"];
            Propuesta.PerfilesProfesionales = Request.Form["PerfilesProfesionales"];
            Propuesta.ParticipacionEstudiantil = Request.Form["ParticipacionEstudiantil"];
            Propuesta.RiesgosCumplimiento = Request.Form["RiesgosCumplimiento"];
            Propuesta.ViabilidadFinanciera = Request.Form["ViabilidadFinanciera"];
            Propuesta.ImpactoSocial = Request.Form["ImpactoSocial"];
            Propuesta.ImpactoAcademico = Request.Form["ImpactoAcademico"];

            // Procedimiento para actualizar la propuesta
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("ModificarPropuesta", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Parámetros para la actualización de los datos de la propuesta
                        command.Parameters.AddWithValue("@IdPropuesta", Propuesta.Id);
                        command.Parameters.AddWithValue("@NombreProyecto", Propuesta.NombreProyecto);
                        command.Parameters.AddWithValue("@IdentificacionAcuerdo", Propuesta.IdentificacionAcuerdo);
                        command.Parameters.AddWithValue("@Descripcion", Propuesta.Descripcion);
                        command.Parameters.AddWithValue("@TipoExtension", Propuesta.TipoExtension);
                        command.Parameters.AddWithValue("@ObjetivoDesarrolloSostenible", Propuesta.ObjetivoDesarrolloSostenible);
                        command.Parameters.AddWithValue("@FechaAprobacion", Propuesta.FechaAprobacion);
                        command.Parameters.AddWithValue("@Introduccion", Propuesta.Introduccion);
                        command.Parameters.AddWithValue("@PerfilesProfesionales", Propuesta.PerfilesProfesionales);
                        command.Parameters.AddWithValue("@ParticipacionEstudiantil", Propuesta.ParticipacionEstudiantil);
                        command.Parameters.AddWithValue("@RiesgosCumplimiento", Propuesta.RiesgosCumplimiento);
                        command.Parameters.AddWithValue("@ViabilidadFinanciera", Propuesta.ViabilidadFinanciera);
                        command.Parameters.AddWithValue("@ImpactoSocial", Propuesta.ImpactoSocial);
                        command.Parameters.AddWithValue("@ImpactoAcademico", Propuesta.ImpactoAcademico);

                        // Ejecutar el procedimiento
                        await command.ExecuteNonQueryAsync();
                        return RedirectToPage("/VieWeb/ConsultarPropuesta", new { propuestaId = Propuesta.Id });
                    }
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"Error en la base de datos: {ex.Message}");
                ErrorMessage = "Ocurrió un error al intentar actualizar la propuesta.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inesperado: {ex.Message}");
                ErrorMessage = "Ocurrió un error inesperado.";
            }
            return Page(); // Si hay error, recarga la página actual
        }
    }
}
