using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ProyectoVie.Pages.VieWeb
{
    public class MiPerfilModel : PageModel
    {
        public DataTable Usuario { get; set; }
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public string Apellido { get; set; }
        [BindProperty]
        public int Cedula { get; set; }
        [BindProperty]
        public string Correo { get; set; }
        [BindProperty]
        public string Contrasena { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                // Obtener el ID del usuario desde la sesión
                int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
                if (userId == 0)
                {
                    ErrorMessage = "No se ha encontrado el ID del usuario en la sesión.";
                    return Page();
                }

                // Conexión a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM USUARIO WHERE ID = @UserID", connection);
                    command.Parameters.AddWithValue("@UserID", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    Usuario = new DataTable();
                    adapter.Fill(Usuario);

                    if (Usuario.Rows.Count == 0)
                    {
                        ErrorMessage = "Usuario no encontrado.";
                        return Page();
                    }

                    // Llenar los campos con los datos actuales del usuario
                    Nombre = Usuario.Rows[0]["Nombre"].ToString();
                    Apellido = Usuario.Rows[0]["Apellido"].ToString();
                    Cedula = Convert.ToInt32(Usuario.Rows[0]["Cedula"]);
                    Correo = Usuario.Rows[0]["Correo"].ToString();
                    Contrasena = Usuario.Rows[0]["Contrasena"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cargar el perfil: {ex.Message}";
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
                if (userId == 0)
                {
                    ErrorMessage = "No se ha encontrado el ID del usuario en la sesión.";
                    return Page();
                }

                // Actualización de los datos del perfil
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("UPDATE USUARIO SET Nombre = @Nombre, Apellido = @Apellido, Cedula = @Cedula, Correo = @Correo, Contrasena = @Contrasena WHERE ID = @UserID", connection);
                    command.Parameters.AddWithValue("@Nombre", Nombre);
                    command.Parameters.AddWithValue("@Apellido", Apellido);
                    command.Parameters.AddWithValue("@Cedula", Cedula);
                    command.Parameters.AddWithValue("@Correo", Correo);
                    command.Parameters.AddWithValue("@Contrasena", Contrasena);
                    command.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        SuccessMessage = "Los cambios se han guardado exitosamente.";
                    }
                    else
                    {
                        ErrorMessage = "No se han realizado cambios.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al actualizar el perfil: {ex.Message}";
            }

            return Page();
        }
    }
}
