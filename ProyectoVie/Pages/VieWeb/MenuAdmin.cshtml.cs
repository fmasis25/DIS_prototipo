using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProyectoVie.Pages.VieWeb
{
    public class MenuAdminModel : PageModel
    {
        public List<Dictionary<string, object>> Propuestas { get; set; }
        public List<Dictionary<string, object>> EstadosPropuesta { get; set; }
        public int SelectedEstadoId { get; set; }

        public void OnGet(int estadoId = 0)
        {
            // Estado por defecto, si no se selecciona en el dropdown
            SelectedEstadoId = estadoId;

            Propuestas = new List<Dictionary<string, object>>();
            EstadosPropuesta = new List<Dictionary<string, object>>();

            // Conexión a la base de datos
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener todos los estados de propuesta (para el dropdown)
                SqlCommand commandEstados = new SqlCommand("SELECT ID, Nombre FROM ESTADO_PROYECTO", connection);
                SqlDataReader readerEstados = commandEstados.ExecuteReader();

                while (readerEstados.Read())
                {
                    var estado = new Dictionary<string, object>
                    {
                        { "ID", readerEstados["ID"] },
                        { "Nombre", readerEstados["Nombre"].ToString() }
                    };
                    EstadosPropuesta.Add(estado);
                }

                readerEstados.Close(); // Cerrar el primer reader antes de ejecutar otro comando

                // Consultar las propuestas según el estado seleccionado (si estadoId == 0, se muestran todas)
                SqlCommand commandPropuestas;

                if (estadoId == 0)
                {
                    // Aquí ajustaré para que llame al procedimiento correcto y devuelva las columnas necesarias
                    commandPropuestas = new SqlCommand("ConsultarTodasPropuesta", connection);
                }
                else
                {
                    // Aquí se llama al procedimiento almacenado para propuestas por estado
                    commandPropuestas = new SqlCommand("ConsultarPropuestasPorEstado", connection);
                    commandPropuestas.CommandType = CommandType.StoredProcedure;
                    commandPropuestas.Parameters.AddWithValue("@EstadoId", estadoId); // Parámetro del estado seleccionado
                }

                SqlDataReader readerPropuestas = commandPropuestas.ExecuteReader();

                while (readerPropuestas.Read())
                {
                    try
                    {
                        var propuesta = new Dictionary<string, object>
                        {
                            { "ID", readerPropuestas["ID"] },
                            { "NombrePropuesta", readerPropuestas["NombrePropuesta"].ToString() },
                            { "Estado", readerPropuestas["Estado"].ToString() },  // Validando que esta columna esté disponible
                            { "ID_Estado", readerPropuestas["ID_Estado"] }
                        };
                        Propuestas.Add(propuesta);
                    }
                    catch (IndexOutOfRangeException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                readerPropuestas.Close();
            }
        }
    }
}
