using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace ProyectoVie.Pages.VieWeb
{
    public class EvaluadoresModel : PageModel
    {
        public List<Usuario> UsuariosConRol2 { get; set; }

        public IActionResult OnGet()
        {
            UsuariosConRol2 = new List<Usuario>();

            try
            {
                // Conectar a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT Nombre, ID_rol FROM USUARIO WHERE ID_rol = 2", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Usuario usuario = new Usuario
                            {
                                Nombre = reader.GetString(0),
                                ID_rol = reader.GetByte(1) // Asumiendo que es tinyint
                            };

                            UsuariosConRol2.Add(usuario);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // Manejo de errores
                return Content($"Error al cargar los usuarios: {ex.Message}");
            }

            return Page();
        }

        public IActionResult OnPostAsignar(int usuarioId)
        {
            // Aquí se realiza la lógica para asignar al evaluador (lo puedes ajustar según tu necesidad)
            return RedirectToPage("/VieWeb/MenuAdmin");
        }
    }

    // Clase para representar un usuario
    public class Usuarios
    {
        public string Nombre { get; set; }
        public byte ID_rol { get; set; }
    }
}
