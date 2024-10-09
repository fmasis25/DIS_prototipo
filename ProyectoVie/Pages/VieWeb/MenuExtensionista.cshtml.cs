using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProyectoVie.Pages.VieWeb
{
    public class MenuExtensionistaModel : PageModel
    {
        public List<Dictionary<string, object>> Propuestas { get; set; }

        public void OnGet()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0; // Obtener el ID del usuario de la sesión
            Propuestas = new List<Dictionary<string, object>>();

            // Conexión a la base de datos
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Ejecutar el procedimiento almacenado
                SqlCommand command = new SqlCommand("ConsultarPropuestasPorUsuario", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID_Usuario", userId); // Enviar el ID del usuario como parámetro

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var propuesta = new Dictionary<string, object>
                    {
                        { "ID", reader["ID"] },
                        { "NombrePropuesta", reader["NombrePropuesta"].ToString() },
                        { "Estado", reader["Estado"].ToString() },
                        { "ID_Estado", reader["ID_Estado"] }
                    };
                    Propuestas.Add(propuesta);
                }
            }
        }
    }
}
