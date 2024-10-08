using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ProyectoVie.Pages.VieWeb
{
    public class UsuariosModel : PageModel
    {
        public List<Usuario> Usuarios { get; set; }

        public void OnGet()
        {
            Usuarios = new List<Usuario>();
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM USUARIO", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usuarios.Add(new Usuario
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Rol = reader["ID_rol"].ToString(),
                        Cedula = reader["Cedula"].ToString(),
                    });
                }
            }
        }

        public IActionResult OnPostEliminarUsuario(int cedula)
        {
            try
            {
                string connectionString = "your_connection_string_here";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("EliminarUsuario", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InCedula", cedula);
                    command.ExecuteNonQuery();
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                // Manejar el error
                return Page();
            }
        }
    }

    public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; }
        public string Cedula { get; set; }
    }
}
