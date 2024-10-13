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
                        Contrasena = reader["Contrasena"].ToString()
                    });
                }
            }
        }

        // Eliminar Usuario
        public IActionResult OnPostEliminar(int cedula)
        {
            try
            {
                string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Ejecutar el comando DELETE para eliminar el usuario
                    SqlCommand command = new SqlCommand("DELETE FROM USUARIO WHERE Cedula = @Cedula", connection);
                    command.Parameters.AddWithValue("@Cedula", cedula);
                    command.ExecuteNonQuery();
                }

                return RedirectToPage("/VieWeb/Usuarios");
            }
            catch (Exception ex)
            {
                // Manejar errores
                return Page();
            }
        }

        // Editar Usuario
        public IActionResult OnPostEditar(Usuario usuario)
        {
            try
            {
                string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Ejecutar el comando UPDATE para actualizar la información del usuario
                    SqlCommand command = new SqlCommand("UPDATE USUARIO SET Nombre = @Nombre, Correo = @Correo, Contrasena = @Contrasena, ID_rol = @ID_rol WHERE Cedula = @Cedula", connection);
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Correo", usuario.Correo);
                    command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena);
                    command.Parameters.AddWithValue("@ID_rol", usuario.Rol);
                    command.Parameters.AddWithValue("@Cedula", usuario.Cedula);
                    command.ExecuteNonQuery();
                }

                return RedirectToPage("/VieWeb/Usuarios");
            }
            catch (Exception ex)
            {
                // Manejar errores
                return Page();
            }
        }
    }

public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Rol { get; set; } // Texto que representa el Rol
        public string Cedula { get; set; }
        public string Contrasena { get; set; } // Añadir propiedad para contraseña
        public int ID_rol { get; set; } // Asegurarse de incluir esta propiedad
    }

}
