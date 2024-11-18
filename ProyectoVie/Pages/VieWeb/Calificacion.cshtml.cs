using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ProyectoVie.Pages.VieWeb
{
    public class CalificacionModel : PageModel
    {
        [BindProperty]
        public int PropuestaId { get; set; }

        // Afinidad
        [BindProperty]
        public int NoCumpleAfinidad { get; set; }
        [BindProperty]
        public int CumpleParcialAfinidad { get; set; }
        [BindProperty]
        public int CumpleAfinidad { get; set; }
        [BindProperty]
        public string? PorcentajeAfinidad { get; set; }

        // Problema
        [BindProperty]
        public int NoCumpleProblema { get; set; }
        [BindProperty]
        public int CumpleParcialProblema { get; set; }
        [BindProperty]
        public int CumpleProblema { get; set; }
        [BindProperty]
        public string? PorcentajeProblema { get; set; }

        // Estrategia
        [BindProperty]
        public int NoCumpleEstrategia { get; set; }
        [BindProperty]
        public int CumpleParcialEstrategia { get; set; }
        [BindProperty]
        public int CumpleEstrategia { get; set; }
        [BindProperty]
        public string? PorcentajeEstrategia { get; set; }

        // Informaci�n t�cnica
        [BindProperty]
        public int NoCumpleInformacion { get; set; }
        [BindProperty]
        public int CumpleParcialInformacion { get; set; }
        [BindProperty]
        public int CumpleInformacion { get; set; }
        [BindProperty]
        public string? PorcentajeInformacion { get; set; }

        // Viabilidad financiera
        [BindProperty]
        public int NoCumpleFinanciera { get; set; }
        [BindProperty]
        public int CumpleParcialFinanciera { get; set; }
        [BindProperty]
        public int CumpleFinanciera { get; set; }
        [BindProperty]
        public string? PorcentajeFinanciera { get; set; }

        // Participaci�n estudiantil
        [BindProperty]
        public int NoCumpleParticipacion { get; set; }
        [BindProperty]
        public int CumpleParcialParticipacion { get; set; }
        [BindProperty]
        public int CumpleParticipacion { get; set; }
        [BindProperty]
        public string? PorcentajeParticipacion { get; set; }

        // Total
        [BindProperty]
        public decimal PorcentajeTotal { get; set; }

        public void OnGet()
        {

            if (Request.Query.ContainsKey("propuestaId"))
            {

                if (int.TryParse(Request.Query["propuestaId"], out var propuestaId))
                {
                    PropuestaId = propuestaId; // Asigna el valor a la propiedad

                    // Cadena de conexi�n a la base de datos
                    string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "SELECT * FROM CALIFICACION WHERE ID_Propuesta = @idPropuesta";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@idPropuesta", PropuestaId); // Agregar el par�metro

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    PorcentajeTotal = reader["Nota"] != DBNull.Value ? (decimal)reader["Nota"] : 0;
                                    NoCumpleAfinidad = (bool)reader["No_Cumple_Afinidad"] ? 1 : 0;
                                    CumpleParcialAfinidad = (bool)reader["Cumple_Parcial_Afinidad"] ? 1 : 0;
                                    CumpleAfinidad = (bool)reader["Cumple_Afinidad"] ? 1 : 0;
                                    PorcentajeAfinidad = reader["Comentario_Afinidad"] != DBNull.Value ? (string)reader["Comentario_Afinidad"] : "Sin comentario";

                                    NoCumpleProblema = (bool)reader["No_Cumple_Problema"] ? 1 : 0;
                                    CumpleParcialProblema = (bool)reader["Cumple_Parcial_Problema"] ? 1 : 0;
                                    CumpleProblema = (bool)reader["Cumple_Problema"] ? 1 : 0;
                                    PorcentajeProblema = reader["Comentario_Problema"] != DBNull.Value ? (string)reader["Comentario_Problema"] : "Sin comentario";

                                    NoCumpleEstrategia = (bool)reader["No_Cumple_Estrategia"] ? 1 : 0;
                                    CumpleParcialEstrategia = (bool)reader["Cumple_Parcial_Estrategia"] ? 1 : 0;
                                    CumpleEstrategia = (bool)reader["Cumple_Estrategia"] ? 1 : 0;
                                    PorcentajeEstrategia = reader["Comentario_Estrategia"] != DBNull.Value ? (string)reader["Comentario_Estrategia"] : "Sin comentario";

                                    NoCumpleInformacion = (bool)reader["No_Cumple_Informacion"] ? 1 : 0;
                                    CumpleParcialInformacion = (bool)reader["Cumple_Parcial_Informacion"] ? 1 : 0;
                                    CumpleInformacion = (bool)reader["Cumple_Informacion"] ? 1 : 0;
                                    PorcentajeInformacion = reader["Comentario_Informacion"] != DBNull.Value ? (string)reader["Comentario_Informacion"] : "Sin comentario";

                                    NoCumpleFinanciera = (bool)reader["No_Cumple_Financiera"] ? 1 : 0;
                                    CumpleParcialFinanciera = (bool)reader["Cumple_Parcial_Financiera"] ? 1 : 0;
                                    CumpleFinanciera = (bool)reader["Cumple_Financiera"] ? 1 : 0;
                                    PorcentajeFinanciera = reader["Comentario_Financiera"] != DBNull.Value ? (string)reader["Comentario_Financiera"] : "Sin comentario";

                                    NoCumpleParticipacion = (bool)reader["No_Cumple_Participacion"] ? 1 : 0;
                                    CumpleParcialParticipacion = (bool)reader["Cumple_Parcial_Participacion"] ? 1 : 0;
                                    CumpleParticipacion = (bool)reader["Cumple_Participacion"] ? 1 : 0;
                                    PorcentajeParticipacion = reader["Comentario_Participacion"] != DBNull.Value ? (string)reader["Comentario_Participacion"] : "Sin comentario";
                                }
                                else
                                {
                                    Console.WriteLine("No se encontraron datos para el ID de propuesta proporcionado.");
                                }
                            }
                        }
                        Console.WriteLine("Consulta realizada exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
                else
                {
                    // Manejo de errores si no se puede convertir
                    ModelState.AddModelError(string.Empty, "El ID de propuesta no es válido.");
                }
            }
            else
            {
                // Manejo de errores si el par�metro no existe
                ModelState.AddModelError(string.Empty, "No se encontró el ID de propuesta.");
            }
        }
    }
}