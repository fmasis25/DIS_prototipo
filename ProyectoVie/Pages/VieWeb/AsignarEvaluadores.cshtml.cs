using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ProyectoVie.Pages.VieWeb
{
    public class AsignarEvaluadoresModel : PageModel
    {
        public List<Usuario> UsuariosConRol2 { get; set; }

        public void OnGet()
        {
            UsuariosConRol2 = new List<Usuario>();
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT ID, CONCAT(Nombre, ' ', Apellido) AS NombreCompleto FROM USUARIO WHERE ID_rol = 2", connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    UsuariosConRol2.Add(new Usuario
                    {
                        ID = Convert.ToInt32(reader["ID"]),
                        NombreCompleto = reader["NombreCompleto"].ToString()
                    });
                }
            }
        }

        public IActionResult OnGetAsignar(int usuarioId)
        {
            // Código para realizar la asignación de la propuesta al usuario seleccionado
            // Aquí debes definir cómo se asigna la propuesta (depende de la lógica de tu aplicación)

            // Ejemplo de consulta para asignar una propuesta al usuario:
            try
            {
                string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("AsignarPropuesta", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@usuarioId", usuarioId);
                    // Agregar otros parámetros según sea necesario, como la ID de la propuesta
                    command.ExecuteNonQuery();
                }

                return RedirectToPage("/VieWeb/AsignarPropuesta");
            }
            catch (Exception ex)
            {
                // Manejar el error
                return Page();
            }
        }

        public class Usuario
        {
            public int ID { get; set; }
            public string NombreCompleto { get; set; }
        }
    }
}
