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
            // Validaci�n b�sica de entradas vac�as
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Por favor, ingrese su correo electr�nico y contrase�a.";
                return Page();
            }

            try
            {
                // Conexi�n a la base de datos
                using (SqlConnection connection = new SqlConnection("Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("IniciarSesion", connection))
                    {
                        // Especificamos que estamos utilizando un procedimiento almacenado
                        command.CommandType = CommandType.StoredProcedure;

                        // Pasamos los par�metros de entrada (correo y contrase�a)
                        command.Parameters.AddWithValue("@Correo", Email);
                        command.Parameters.AddWithValue("@Contrasena", Password);

                        // Par�metros de salida para ID del usuario y rol
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

                        // Verificamos si la autenticaci�n fue exitosa
                        if (idUsuario == -1 || rol == -1)
                        {
                            ErrorMessage = "Correo electr�nico o contrase�a incorrectos.";
                            return Page();
                        }

                        // Guardamos el ID y el Rol en la sesi�n para futuros usos
                        HttpContext.Session.SetInt32("UserID", idUsuario);
                        HttpContext.Session.SetInt32("UserRole", rol);

                        // Redirigir seg�n el rol del usuario
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
                // Captura de cualquier error durante la ejecuci�n
                ErrorMessage = $"Error de inicio de sesi�n: {ex.Message}";
                return Page();
            }
        }
    }
}
