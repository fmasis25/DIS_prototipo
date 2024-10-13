using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProyectoVie.Pages.VieWeb
{
    public class MiPerfilModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Apellido { get; set; }

        [BindProperty]
        public string Correo { get; set; }

        [BindProperty]
        public string Contrasena { get; set; }

        public int Rol { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            // Verificar si el ID del usuario está en la sesión
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                ErrorMessage = "No se ha encontrado el ID del usuario en la sesión.";
                return;
            }

            try
            {
                // Conexión a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT Nombre, Apellido, Correo, Contrasena, ID_rol FROM USUARIO WHERE ID = @UserID", connection);
                    command.Parameters.AddWithValue("@UserID", userId.Value);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Nombre = reader["Nombre"].ToString();
                        Apellido = reader["Apellido"].ToString();
                        Correo = reader["Correo"].ToString();
                        Contrasena = reader["Contrasena"].ToString();
                        Rol = Convert.ToInt32(reader["ID_rol"]);
                    }
                    else
                    {
                        ErrorMessage = "No se encontraron datos del usuario.";
                    }
                }
            }
            catch (SqlException ex)
            {
                ErrorMessage = "Error al cargar los datos del usuario: " + ex.Message;
            }
        }

        public IActionResult OnPost()
        {
            // Verificar si el ID del usuario está en la sesión
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                ErrorMessage = "No se ha encontrado el ID del usuario en la sesión.";
                return Page();
            }

            try
            {
                // Conexión a la base de datos para actualizar los datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE USUARIO SET Nombre = @Nombre, Apellido = @Apellido, Correo = @Correo, Contrasena = @Contrasena WHERE ID = @UserID", connection);
                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Apellido", Apellido);
                    command.Parameters.AddWithValue("@Correo", Correo);
                    command.Parameters.AddWithValue("@Contrasena", Contrasena);
                    command.Parameters.AddWithValue("@UserID", userId.Value);

                    command.ExecuteNonQuery();
                }

                SuccessMessage = "Cambios guardados correctamente.";
            }
            catch (SqlException ex)
            {
                ErrorMessage = "Error al guardar los cambios: " + ex.Message;
            }

            return Page();
        }
    }
}
