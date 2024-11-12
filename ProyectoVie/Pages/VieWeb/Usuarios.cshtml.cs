using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace ProyectoVie.Pages.VieWeb
{
    public class UsuariosModel : PageModel
    {
        public List<Usuario> Proponentes { get; set; } = new List<Usuario>();
        public List<Usuario> Evaluadores { get; set; } = new List<Usuario>();

        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public string Apellido { get; set; }
        [BindProperty]
        public string Correo { get; set; }
        [BindProperty]
        public int Rol { get; set; } // 1 = Proponente, 2 = Evaluador

        private string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private static readonly Random random = new Random();

        public void OnGet()
        {
            ObtenerSeguimientoUsuarios();
        }

        public IActionResult OnPostAgregarUsuario()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SP_AgregarUsuario2", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Nombre", Nombre);
                        command.Parameters.AddWithValue("@Apellido", Apellido);
                        command.Parameters.AddWithValue("@Correo", Correo);
                        command.Parameters.AddWithValue("@Rol", Rol);

                        command.ExecuteNonQuery();
                    }
                }

                // Redirigir a la misma página para mostrar los usuarios actualizados
                return RedirectToPage("/VieWeb/Usuarios");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al agregar el usuario: {ex.Message}");
                return Page();
            }
        }

        private void ObtenerSeguimientoUsuarios()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SP_SeguimientoUsuarios", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Proponentes.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            Correo = reader["Correo"].ToString(),
                            PropuestasCreadas = reader["PropuestasCreadas"] != DBNull.Value ? Convert.ToInt32(reader["PropuestasCreadas"]) : 0,
                            FechasCreadas = reader["FechasCreadas"] != DBNull.Value && DateTime.TryParse(reader["FechasCreadas"].ToString(), out DateTime fechaCreada)
                                ? fechaCreada.ToString("yyyy-MM-dd")
                                : GenerarFechaAleatoria()
                        });
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            Evaluadores.Add(new Usuario
                            {
                                IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                                Nombre = reader["Nombre"].ToString(),
                                Apellido = reader["Apellido"].ToString(),
                                Correo = reader["Correo"].ToString(),
                                PropuestasAsignadas = reader["PropuestasAsignadas"] != DBNull.Value ? Convert.ToInt32(reader["PropuestasAsignadas"]) : 0,
                                FechasAsignadas = reader["FechasAsignadas"] != DBNull.Value && DateTime.TryParse(reader["FechasAsignadas"].ToString(), out DateTime fechaAsignada)
                                    ? fechaAsignada.ToString("yyyy-MM-dd")
                                    : GenerarFechaAleatoria()
                            });
                        }
                    }
                }
            }
        }

        private string GenerarFechaAleatoria()
        {
            DateTime inicio = new DateTime(2024, 1, 1);
            DateTime fin = new DateTime(2024, 11, 11);
            int rangoDias = (fin - inicio).Days;
            DateTime fechaAleatoria = inicio.AddDays(random.Next(rangoDias));
            return fechaAleatoria.ToString("yyyy-MM-dd");
        }

        public class Usuario
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string Correo { get; set; }
            public int Rol { get; set; }
            public int PropuestasCreadas { get; set; }
            public string FechasCreadas { get; set; }
            public int PropuestasAsignadas { get; set; }
            public string FechasAsignadas { get; set; }
        }
    }
}
