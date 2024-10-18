using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProyectoVie.Pages.VieWeb
{
    public class EvaluarModel : PageModel
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
        public int PorcentajeAfinidad { get; set; }

        // Problema
        [BindProperty]
        public int NoCumpleProblema { get; set; }
        [BindProperty]
        public int CumpleParcialProblema { get; set; }
        [BindProperty]
        public int CumpleProblema { get; set; }
        [BindProperty]
        public int PorcentajeProblema { get; set; }

        // Estrategia
        [BindProperty]
        public int NoCumpleEstrategia { get; set; }
        [BindProperty]
        public int CumpleParcialEstrategia { get; set; }
        [BindProperty]
        public int CumpleEstrategia { get; set; }
        [BindProperty]
        public int PorcentajeEstrategia { get; set; }

        // Información técnica
        [BindProperty]
        public int NoCumpleInformacion { get; set; }
        [BindProperty]
        public int CumpleParcialInformacion { get; set; }
        [BindProperty]
        public int CumpleInformacion { get; set; }
        [BindProperty]
        public int PorcentajeInformacion { get; set; }

        // Viabilidad financiera
        [BindProperty]
        public int NoCumpleFinanciera { get; set; }
        [BindProperty]
        public int CumpleParcialFinanciera { get; set; }
        [BindProperty]
        public int CumpleFinanciera { get; set; }
        [BindProperty]
        public int PorcentajeFinanciera { get; set; }

        // Participación estudiantil
        [BindProperty]
        public int NoCumpleParticipacion { get; set; }
        [BindProperty]
        public int CumpleParcialParticipacion { get; set; }
        [BindProperty]
        public int CumpleParticipacion { get; set; }
        [BindProperty]
        public int PorcentajeParticipacion { get; set; }

        // Total
        [BindProperty]
        public int PorcentajeTotal { get; set; }
        public void OnGet()
        {
            // Intenta obtener el parámetro de la URL
            if (Request.Query.ContainsKey("propuestaId"))
            {
                // Asegúrate de que el valor se puede convertir a un entero
                if (int.TryParse(Request.Query["propuestaId"], out var propuestaId))
                {
                    PropuestaId = propuestaId; // Asigna el valor a la propiedad
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

        public IActionResult OnPost()
        {
            // Los porcentajes ya son de tipo decimal

            decimal porcentajeAfinidad = PorcentajeAfinidad;
            decimal porcentajeProblema = PorcentajeProblema;
            decimal porcentajeEstrategia = PorcentajeEstrategia;
            decimal porcentajeInformacion = PorcentajeInformacion;
            decimal porcentajeFinanciera = PorcentajeFinanciera;
            decimal porcentajeParticipacion = PorcentajeParticipacion;
            decimal porcentajeTotal = PorcentajeTotal;




            // Cadena de conexión a la base de datos
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO CALIFICACION 
                             (Nota, Fecha_Revision, ID_Propuesta, No_Cumple_Afinidad, Cumple_Parcial_Afinidad, Cumple_Afinidad, Porcentaje_Afinidad, 
                              No_Cumple_Problema, Cumple_Parcial_Problema, Cumple_Problema, Porcentaje_Problema, 
                              No_Cumple_Estrategia, Cumple_Parcial_Estrategia, Cumple_Estrategia, Porcentaje_Estrategia,
                              No_Cumple_Informacion, Cumple_Parcial_Informacion, Cumple_Informacion, Porcentaje_Informacion,
                              No_Cumple_Financiera, Cumple_Parcial_Financiera, Cumple_Financiera, Porcentaje_Financiera,
                              No_Cumple_Participacion, Cumple_Parcial_Participacion, Cumple_Participacion, Porcentaje_Participacion) 
                             VALUES 
                             (@PorcentajeTotal, GETDATE(), @PropuestaId, @NoCumpleAfinidad, @CumpleParcialAfinidad, @CumpleAfinidad, @PorcentajeAfinidad, 
                              @NoCumpleProblema, @CumpleParcialProblema, @CumpleProblema, @PorcentajeProblema, 
                              @NoCumpleEstrategia, @CumpleParcialEstrategia, @CumpleEstrategia, @PorcentajeEstrategia, 
                              @NoCumpleInformacion, @CumpleParcialInformacion, @CumpleInformacion, @PorcentajeInformacion, 
                              @NoCumpleFinanciera, @CumpleParcialFinanciera, @CumpleFinanciera, @PorcentajeFinanciera, 
                              @NoCumpleParticipacion, @CumpleParcialParticipacion, @CumpleParticipacion, @PorcentajeParticipacion)";

                    
                    // Actualiza el estado en la tabla PROPUESTA
                    string query2 = @"UPDATE PROPUESTA 
                      SET ID_Estado = @nuevoEstado 
                      WHERE Id = @PropuestaId";

                    SqlCommand command = new SqlCommand(query, connection);
                    SqlCommand command2 = new SqlCommand(query2, connection);

                    command.Parameters.AddWithValue("@PorcentajeTotal", porcentajeTotal);
                    command.Parameters.AddWithValue("@PropuestaId", PropuestaId);
                    command.Parameters.AddWithValue("@NoCumpleAfinidad", NoCumpleAfinidad);
                    command.Parameters.AddWithValue("@CumpleParcialAfinidad", CumpleParcialAfinidad);
                    command.Parameters.AddWithValue("@CumpleAfinidad", CumpleAfinidad);
                    command.Parameters.AddWithValue("@PorcentajeAfinidad", porcentajeAfinidad);

                    command.Parameters.AddWithValue("@NoCumpleProblema", NoCumpleProblema);
                    command.Parameters.AddWithValue("@CumpleParcialProblema", CumpleParcialProblema);
                    command.Parameters.AddWithValue("@CumpleProblema", CumpleProblema);
                    command.Parameters.AddWithValue("@PorcentajeProblema", porcentajeProblema);

                    command.Parameters.AddWithValue("@NoCumpleEstrategia", NoCumpleEstrategia);
                    command.Parameters.AddWithValue("@CumpleParcialEstrategia", CumpleParcialEstrategia);
                    command.Parameters.AddWithValue("@CumpleEstrategia", CumpleEstrategia);
                    command.Parameters.AddWithValue("@PorcentajeEstrategia", porcentajeEstrategia);

                    command.Parameters.AddWithValue("@NoCumpleInformacion", NoCumpleInformacion);
                    command.Parameters.AddWithValue("@CumpleParcialInformacion", CumpleParcialInformacion);
                    command.Parameters.AddWithValue("@CumpleInformacion", CumpleInformacion);
                    command.Parameters.AddWithValue("@PorcentajeInformacion", porcentajeInformacion);

                    command.Parameters.AddWithValue("@NoCumpleFinanciera", NoCumpleFinanciera);
                    command.Parameters.AddWithValue("@CumpleParcialFinanciera", CumpleParcialFinanciera);
                    command.Parameters.AddWithValue("@CumpleFinanciera", CumpleFinanciera);
                    command.Parameters.AddWithValue("@PorcentajeFinanciera", porcentajeFinanciera);

                    command.Parameters.AddWithValue("@NoCumpleParticipacion", NoCumpleParticipacion);
                    command.Parameters.AddWithValue("@CumpleParcialParticipacion", CumpleParcialParticipacion);
                    command.Parameters.AddWithValue("@CumpleParticipacion", CumpleParticipacion);
                    command.Parameters.AddWithValue("@PorcentajeParticipacion", porcentajeParticipacion);

                    // Determina el nuevo estado según el porcentaje
                    int nuevoEstado = porcentajeTotal >= 70 ? 4 : 1;
                    command2.Parameters.AddWithValue("@nuevoEstado", nuevoEstado);
                    command2.Parameters.AddWithValue("@PropuestaId", PropuestaId); 
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Estado actualizado correctamente.");
                    

                    command.ExecuteNonQuery();
                    Console.WriteLine("Datos insertados exitosamente.");
                    return RedirectToPage("/VieWeb/MenuAdmin");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Page();
            }
        }


    }
}
