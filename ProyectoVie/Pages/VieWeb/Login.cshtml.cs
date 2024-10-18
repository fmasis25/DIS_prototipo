using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Microsoft.Data.SqlClient;
using System;
using Microsoft.AspNetCore.Http;

namespace ProyectoVie.Pages.VieWeb
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            // Validación básica de entradas vacías
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Por favor, ingrese su correo electrónico y contraseña.";
                return Page();
            }

            try
            {
                // Conexión a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("IniciarSesion", connection))
                    {
                        // Especificamos que estamos utilizando un procedimiento almacenado
                        command.CommandType = CommandType.StoredProcedure;

                        // Pasamos los parámetros de entrada (correo y contraseña)
                        command.Parameters.AddWithValue("@Correo", Email);
                        command.Parameters.AddWithValue("@Contrasena", Password);

                        // Parámetros de salida para ID del usuario y rol
                        SqlParameter idUsuarioParam = new SqlParameter("@id_usuario", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(idUsuarioParam);

                        SqlParameter rolParam = new SqlParameter("@rol", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(rolParam);

                        // Ejecutamos el procedimiento almacenado
                        command.ExecuteNonQuery();

                        // Recuperamos los valores de salida
                        int idUsuario = idUsuarioParam.Value != DBNull.Value ? Convert.ToInt32(idUsuarioParam.Value) : -1;
                        int rol = rolParam.Value != DBNull.Value ? Convert.ToInt32(rolParam.Value) : -1;

                        // Verificamos si la autenticación fue exitosa
                        if (idUsuario == -1 || rol == -1)
                        {
                            ErrorMessage = "Correo electrónico o contraseña incorrectos.";
                            return Page();
                        }

                        // Guardamos el ID y el Rol en la sesión para futuros usos
                        HttpContext.Session.SetInt32("UserID", idUsuario);
                        HttpContext.Session.SetInt32("UserRole", rol);

                        // Redirigir según el rol del usuario
                        if (rol == 1)  // Extensionista
                        {
                            return RedirectToPage("/VieWeb/MenuExtensionista");
                        }
                        else if (rol == 2)  // Administrador
                        {
                            return RedirectToPage("/VieWeb/MenuAdmin");
                        }
                        else
                        {
                            ErrorMessage = "Rol de usuario no reconocido.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura de cualquier error durante la ejecución
                ErrorMessage = $"Error de inicio de sesión: {ex.Message}";
                return Page();
            }
        }
    }
}
