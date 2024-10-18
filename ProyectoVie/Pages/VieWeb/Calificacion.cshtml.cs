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
        public decimal PorcentajeAfinidad { get; set; }

        // Problema
        [BindProperty]
        public int NoCumpleProblema { get; set; }
        [BindProperty]
        public int CumpleParcialProblema { get; set; }
        [BindProperty]
        public int CumpleProblema { get; set; }
        [BindProperty]
        public decimal PorcentajeProblema { get; set; }

        // Estrategia
        [BindProperty]
        public int NoCumpleEstrategia { get; set; }
        [BindProperty]
        public int CumpleParcialEstrategia { get; set; }
        [BindProperty]
        public int CumpleEstrategia { get; set; }
        [BindProperty]
        public decimal PorcentajeEstrategia { get; set; }

        // Información técnica
        [BindProperty]
        public int NoCumpleInformacion { get; set; }
        [BindProperty]
        public int CumpleParcialInformacion { get; set; }
        [BindProperty]
        public int CumpleInformacion { get; set; }
        [BindProperty]
        public decimal PorcentajeInformacion { get; set; }

        // Viabilidad financiera
        [BindProperty]
        public int NoCumpleFinanciera { get; set; }
        [BindProperty]
        public int CumpleParcialFinanciera { get; set; }
        [BindProperty]
        public int CumpleFinanciera { get; set; }
        [BindProperty]
        public decimal PorcentajeFinanciera { get; set; }

        // Participación estudiantil
        [BindProperty]
        public int NoCumpleParticipacion { get; set; }
        [BindProperty]
        public int CumpleParcialParticipacion { get; set; }
        [BindProperty]
        public int CumpleParticipacion { get; set; }
        [BindProperty]
        public decimal PorcentajeParticipacion { get; set; }

        // Total
        [BindProperty]
        public decimal PorcentajeTotal { get; set; }

        public void OnGet()
        {
            // Intenta obtener el parámetro de la URL
            if (Request.Query.ContainsKey("propuestaId"))
            {
                // Asegúrate de que el valor se puede convertir a un entero
                if (int.TryParse(Request.Query["propuestaId"], out var propuestaId))
                {
                    PropuestaId = propuestaId; // Asigna el valor a la propiedad

                    // Cadena de conexión a la base de datos
                    string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "SELECT * FROM CALIFICACION WHERE ID_Propuesta = @idPropuesta";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@idPropuesta", PropuestaId); // Agregar el parámetro

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    PorcentajeTotal = reader["Nota"] != DBNull.Value ? (decimal)reader["Nota"] : 0;
                                    NoCumpleAfinidad = (bool)reader["No_Cumple_Afinidad"] ? 1 : 0;
                                    CumpleParcialAfinidad = (bool)reader["Cumple_Parcial_Afinidad"] ? 1 : 0;
                                    CumpleAfinidad = (bool)reader["Cumple_Afinidad"] ? 1 : 0;
                                    PorcentajeAfinidad = reader["Porcentaje_Afinidad"] != DBNull.Value ? (decimal)reader["Porcentaje_Afinidad"] : 0;

                                    NoCumpleProblema = (bool)reader["No_Cumple_Problema"] ? 1 : 0;
                                    CumpleParcialProblema = (bool)reader["Cumple_Parcial_Problema"] ? 1 : 0;
                                    CumpleProblema = (bool)reader["Cumple_Problema"] ? 1 : 0;
                                    PorcentajeProblema = reader["Porcentaje_Problema"] != DBNull.Value ? (decimal)reader["Porcentaje_Problema"] : 0;

                                    NoCumpleEstrategia = (bool)reader["No_Cumple_Estrategia"] ? 1 : 0;
                                    CumpleParcialEstrategia = (bool)reader["Cumple_Parcial_Estrategia"] ? 1 : 0;
                                    CumpleEstrategia = (bool)reader["Cumple_Estrategia"] ? 1 : 0;
                                    PorcentajeEstrategia = reader["Porcentaje_Estrategia"] != DBNull.Value ? (decimal)reader["Porcentaje_Estrategia"] : 0;

                                    NoCumpleInformacion = (bool)reader["No_Cumple_Informacion"] ? 1 : 0;
                                    CumpleParcialInformacion = (bool)reader["Cumple_Parcial_Informacion"] ? 1 : 0;
                                    CumpleInformacion = (bool)reader["Cumple_Informacion"] ? 1 : 0;
                                    PorcentajeInformacion = reader["Porcentaje_Informacion"] != DBNull.Value ? (decimal)reader["Porcentaje_Informacion"] : 0;

                                    NoCumpleFinanciera = (bool)reader["No_Cumple_Financiera"] ? 1 : 0;
                                    CumpleParcialFinanciera = (bool)reader["Cumple_Parcial_Financiera"] ? 1 : 0;
                                    CumpleFinanciera = (bool)reader["Cumple_Financiera"] ? 1 : 0;
                                    PorcentajeFinanciera = reader["Porcentaje_Financiera"] != DBNull.Value ? (decimal)reader["Porcentaje_Financiera"] : 0;

                                    NoCumpleParticipacion = (bool)reader["No_Cumple_Participacion"] ? 1 : 0;
                                    CumpleParcialParticipacion = (bool)reader["Cumple_Parcial_Participacion"] ? 1 : 0;
                                    CumpleParticipacion = (bool)reader["Cumple_Participacion"] ? 1 : 0;
                                    PorcentajeParticipacion = reader["Porcentaje_Participacion"] != DBNull.Value ? (decimal)reader["Porcentaje_Participacion"] : 0;
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
                // Manejo de errores si el parámetro no existe
                ModelState.AddModelError(string.Empty, "No se encontró el ID de propuesta.");
            }
        }
    }
}
