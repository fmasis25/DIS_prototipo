using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProyectoVie.Pages.VieWeb
{
    public class MenuAdminModel : PageModel
    {
        public List<Dictionary<string, object>> Propuestas { get; set; }
        public List<Dictionary<string, object>> EstadosPropuesta { get; set; }
        public string SelectedEstadoId { get; set; }

        public void OnGet(string estado = "0")
        {
            // Estado por defecto, si no se selecciona en el dropdown
            SelectedEstadoId = estado;

            if (estado == "Asignadas")
            {
                // Si el valor seleccionado es "Asignadas", llamar a la función correspondiente
                OnGetAsignadas();
                return;
            }

            // Si el valor no es "Asignadas", seguimos con el flujo normal
            Propuestas = new List<Dictionary<string, object>>();
            EstadosPropuesta = new List<Dictionary<string, object>>();

            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener todos los estados de propuesta (para el dropdown)
                SqlCommand commandEstados = new SqlCommand("SELECT ID, Nombre FROM ESTADO_PROYECTO", connection);
                SqlDataReader readerEstados = commandEstados.ExecuteReader();

                while (readerEstados.Read())
                {
                    var estadoPropuesta = new Dictionary<string, object>
                    {
                        { "ID", readerEstados["ID"] },
                        { "Nombre", readerEstados["Nombre"].ToString() }
                    };
                    EstadosPropuesta.Add(estadoPropuesta);
                }

                readerEstados.Close(); // Cerrar el primer reader antes de ejecutar otro comando

                SqlCommand commandPropuestas;

                if (estado == "0")
                {
                    // Mostrar todas las propuestas si estado es 0 (sin filtro de estado)
                    commandPropuestas = new SqlCommand("SELECT P.ID, P.NombrePropuesta, E.Nombre AS Estado, P.ID_Estado FROM PROPUESTA AS P INNER JOIN ESTADO_PROYECTO AS E ON P.ID_Estado = E.ID", connection);
                }
                else
                {
                    // Mostrar propuestas según el estado seleccionado
                    commandPropuestas = new SqlCommand("ConsultarPropuestasPorEstado", connection);
                    commandPropuestas.CommandType = CommandType.StoredProcedure;
                    commandPropuestas.Parameters.AddWithValue("@EstadoId", int.Parse(estado)); // Parámetro del estado seleccionado
                }

                SqlDataReader readerPropuestas = commandPropuestas.ExecuteReader();

                while (readerPropuestas.Read())
                {
                    var propuesta = new Dictionary<string, object>
                    {
                        { "ID", readerPropuestas["ID"] },
                        { "NombrePropuesta", readerPropuestas["NombrePropuesta"].ToString() },
                        { "Estado", readerPropuestas["Estado"].ToString() },
                        { "ID_Estado", readerPropuestas["ID_Estado"] }
                    };
                    Propuestas.Add(propuesta);
                }

                readerPropuestas.Close();
            }
        }

        // Nueva función para obtener propuestas asignadas al usuario de la sesión
        public void OnGetAsignadas()
        {
            Propuestas = new List<Dictionary<string, object>>();
            EstadosPropuesta = new List<Dictionary<string, object>>();

            // Obtener el ID del usuario de la sesión
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;

            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Obtener todos los estados de propuesta (para el dropdown)
                SqlCommand commandEstados = new SqlCommand("SELECT ID, Nombre FROM ESTADO_PROYECTO", connection);
                SqlDataReader readerEstados = commandEstados.ExecuteReader();

                while (readerEstados.Read())
                {
                    var estadoPropuesta = new Dictionary<string, object>
                    {
                        { "ID", readerEstados["ID"] },
                        { "Nombre", readerEstados["Nombre"].ToString() }
                    };
                    EstadosPropuesta.Add(estadoPropuesta);
                }

                readerEstados.Close(); // Cerrar el primer reader antes de ejecutar otro comando

                // Consultar propuestas asignadas al usuario logueado
                SqlCommand commandPropuestasAsignadas = new SqlCommand(
                    @"SELECT P.ID, P.NombrePropuesta, E.Nombre AS Estado, P.ID_Estado 
                      FROM PROPUESTA AS P
                      INNER JOIN ESTADO_PROYECTO AS E ON P.ID_Estado = E.ID
                      INNER JOIN ASIGNACIONES AS A ON P.ID = A.IdPropuesta
                      WHERE A.IdUsuario = @UserId", connection);

                commandPropuestasAsignadas.Parameters.AddWithValue("@UserId", userId);

                SqlDataReader readerPropuestasAsignadas = commandPropuestasAsignadas.ExecuteReader();

                while (readerPropuestasAsignadas.Read())
                {
                    var propuesta = new Dictionary<string, object>
                    {
                        { "ID", readerPropuestasAsignadas["ID"] },
                        { "NombrePropuesta", readerPropuestasAsignadas["NombrePropuesta"].ToString() },
                        { "Estado", readerPropuestasAsignadas["Estado"].ToString() },
                        { "ID_Estado", readerPropuestasAsignadas["ID_Estado"] }
                    };
                    Propuestas.Add(propuesta);
                }

                readerPropuestasAsignadas.Close();
            }
        }
    }
}
