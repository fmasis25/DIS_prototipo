using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace ProyectoVie.Pages.VieWeb
{
    public class UsuariosModel : PageModel
    {
        public List<Usuario> Usuarios { get; set; } = new List<Usuario>();

        [BindProperty]
        public Usuario NuevoUsuario { get; set; } = new Usuario();

        public string MensajeError { get; set; }
        public string MensajeExito { get; set; }

        private string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // Método para obtener la lista de usuarios desde la base de datos
        public void OnGet()
        {
            ObtenerUsuarios(null); // Se obtiene todos los usuarios si no se envía una cédula
        }

        private void ObtenerUsuarios(int? cedula)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SP_ConsultarUsuarios", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                if (cedula.HasValue)
                {
                    command.Parameters.AddWithValue("@InCedula", cedula.Value); // Enviamos la cédula si existe
                }
                else
                {
                    command.Parameters.AddWithValue("@InCedula", DBNull.Value); // Si no hay cédula, enviamos NULL
                }

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Usuarios.Add(new Usuario
                    {
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        Correo = reader["Correo"].ToString(),
                        Rol = reader["ID_rol"].ToString(),
                        Cedula = reader["Cedula"].ToString(),
                        Contrasena = reader["Contrasena"].ToString(),
                        FechaRegistro = DateTime.Parse(reader["Fecha_Registro"].ToString())
                    });
                }
            }
        }

        // Método para agregar un nuevo usuario
        public IActionResult OnPostAgregarUsuario()
        {
            if (NuevoUsuario == null)
            {
                MensajeError = "No se ha recibido la información del nuevo usuario.";
                return Page();
            }

            if (NuevoUsuario.Rol != "1" && NuevoUsuario.Rol != "2")
            {
                MensajeError = "El rol solo puede ser 1 (Admin) o 2 (Usuario).";
                ObtenerUsuarios(null);
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("AgregarUsuario", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InNombre", NuevoUsuario.Nombre);
                    command.Parameters.AddWithValue("@InApellido", NuevoUsuario.Apellido);
                    command.Parameters.AddWithValue("@InCedula", NuevoUsuario.Cedula);
                    command.Parameters.AddWithValue("@InCorreo", NuevoUsuario.Correo);
                    command.Parameters.AddWithValue("@InContrasena", NuevoUsuario.Contrasena);
                    command.Parameters.AddWithValue("@InID_rol", NuevoUsuario.Rol);
                    command.Parameters.AddWithValue("@InFecha_Registro", DateTime.Now);
                    command.Parameters.AddWithValue("@InID_escuela", 1); // Ajusta según sea necesario

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MensajeExito = "Usuario agregado exitosamente.";
                    }
                    else
                    {
                        MensajeError = "No se pudo agregar el usuario.";
                    }
                }

                return RedirectToPage("/VieWeb/Usuarios");
            }
            catch (Exception ex)
            {
                MensajeError = "Error al agregar el usuario: " + ex.Message;
                ObtenerUsuarios(null);
                return Page();
            }
        }
        // Método para guardar (actualizar) un usuario
        public JsonResult OnPostGuardarUsuario(int cedula, string correo, string rol, string contrasena)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SP_ModificarUsuario2", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InCedula", cedula);
                    command.Parameters.AddWithValue("@InCorreo", correo);
                    command.Parameters.AddWithValue("@InContrasena", contrasena);
                    command.Parameters.AddWithValue("@InID_Rol", rol);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return new JsonResult(new { success = true, message = "Usuario actualizado exitosamente." });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "No se encontró el usuario." });
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error al actualizar el usuario: " + ex.Message });
            }
        }

        // Método para eliminar un usuario
        public JsonResult OnPostEliminarUsuario(int cedula)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SP_EliminarUsuario2", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@InCedula", cedula);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return new JsonResult(new { success = true, message = "Usuario eliminado exitosamente." });
                    }
                    else
                    {
                        return new JsonResult(new { success = false, message = "No se encontró el usuario." });
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error al eliminar el usuario: " + ex.Message });
            }
        }

        public class Usuario
        {
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Correo { get; set; }
            public string Rol { get; set; }
            public string Cedula { get; set; }
            public string Contrasena { get; set; }
            public DateTime FechaRegistro { get; internal set; }
        }
    }
}