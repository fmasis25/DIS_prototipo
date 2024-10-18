using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using static ProyectoVie.Pages.VieWeb.AgregarPropuestaModel;

namespace ProyectoVie.Pages.VieWeb
{
    public class EvaluadoresModel : PageModel
    {
        public List<Usuarios> UsuariosConRol2 { get; set; }
        [BindProperty]
        public int PropuestaId { get; set; }
        public IActionResult OnGet()
        {
            // Intenta obtener el parámetro de la URL
            if (Request.Query.ContainsKey("propuestaId"))
            {
                // Asegúrate de que el valor se puede convertir a un entero
                if (int.TryParse(Request.Query["propuestaId"], out var propuestaId))
                {
                    PropuestaId = propuestaId; // Asigna el valor a la propiedad
                    Console.WriteLine($"Propuesta: {PropuestaId}");
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
            UsuariosConRol2 = new List<Usuarios>();

            try
            {
                // Conectar a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("SELECT U.ID, U.Nombre + ' ' + U.Apellido Nombre, R.Nombre Rol FROM USUARIO U LEFT JOIN ROL R ON U.Id_Rol = R.id WHERE ID_rol = 2", connection))
                    {
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Usuarios usuario = new Usuarios
                            {
                                ID = reader.GetInt32(0),  // ID del usuario
                                Nombre = reader.IsDBNull(1) ? null : reader.GetString(1),  // Nombre del usuario
                                Rol = reader.IsDBNull(2) ? null : reader.GetString(2)  // Rol del usuario
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

        public IActionResult OnPostAsignar(int usuarioId, int PropuestaId)
        {
            Console.WriteLine($"ID del usuario asignado: {usuarioId}");
            Console.WriteLine($"Propuesta: {PropuestaId}");

            // Lógica para insertar en la tabla de asignaciones
            try
            {
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();

                    string query = "INSERT INTO Asignaciones (IdUsuario, IdPropuesta) VALUES (@UsuarioId, @PropuestaId)";
                    string query2 = @"UPDATE PROPUESTA 
                      SET ID_Estado = @nuevoEstado 
                      WHERE Id = @PropuestaId";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlCommand command2 = new SqlCommand(query2, connection);
                    
                    command.Parameters.AddWithValue("@UsuarioId", usuarioId);
                    command.Parameters.AddWithValue("@PropuestaId", PropuestaId);

                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} fila(s) insertada(s).");

                    command2.Parameters.AddWithValue("@nuevoEstado", 3);
                    command2.Parameters.AddWithValue("@PropuestaId", PropuestaId);
                    command2.ExecuteNonQuery();
                    Console.WriteLine("Estado actualizado correctamente.");

                }
            }
            catch (SqlException ex)
            {
                // Manejo de errores
                Console.WriteLine($"Error al insertar en la tabla de asignaciones: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Error al asignar evaluador. Inténtalo de nuevo.");
                return Page(); // Retorna a la misma página en caso de error
            }

            return RedirectToPage("/VieWeb/MenuAdmin"); // Redirecciona después de la inserción
        }

    }

    // Clase para representar un usuario
    public class Usuarios
    {
        public string? Nombre { get; set; }
        public string? Rol { get; set; }
        public int ID { get; set; }  // Asegúrate de tener un ID aquí
    }
}
