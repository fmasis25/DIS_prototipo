using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace ProyectoVie.Pages.VieWeb
{
    public class GraficosModel : PageModel
    {
        private string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public List<Escuela> EscuelasPorPropuesta { get; set; }
        public List<PropuestaPorEstado> PropuestasPorEstado { get; set; }
        public List<EscuelaPorEstado> EscuelasPorEstado { get; set; }
        public List<NotaPromedio> NotasPromedioPorEscuela { get; set; }

        public void OnGet()
        {
            // Llamamos a los métodos que obtienen datos desde los procedimientos almacenados.
            EscuelasPorPropuesta = ObtenerEscuelasPorPropuesta();
            PropuestasPorEstado = ObtenerPropuestasPorEstado();
            EscuelasPorEstado = ObtenerEscuelasPorEstado();
            NotasPromedioPorEscuela = ObtenerNotasPromedioPorEscuela();
        }

        // Métodos que ejecutan procedimientos almacenados para obtener datos.
        private List<Escuela> ObtenerEscuelasPorPropuesta()
        {
            var result = new List<Escuela>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ContarEscuelasPorPropuesta", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Escuela
                            {
                                Nombre = reader["Escuela"].ToString(),
                                TotalEscuelas = Convert.ToInt32(reader["SumaTotalEscuelas"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        private List<PropuestaPorEstado> ObtenerPropuestasPorEstado()
        {
            var result = new List<PropuestaPorEstado>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ContarPropuestasPorEstado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new PropuestaPorEstado
                            {
                                EstadoProyecto = reader["EstadoProyecto"].ToString(),
                                CantidadPropuestas = Convert.ToInt32(reader["CantidadPropuestas"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        private List<EscuelaPorEstado> ObtenerEscuelasPorEstado()
        {
            var result = new List<EscuelaPorEstado>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_ContarEscuelasPorEstado", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new EscuelaPorEstado
                            {
                                Escuela = reader["Escuela"].ToString(),
                                EstadoProyecto = reader["EstadoProyecto"].ToString(),
                                TotalPropuestas = Convert.ToInt32(reader["TotalPropuestas"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        private List<NotaPromedio> ObtenerNotasPromedioPorEscuela()
        {
            var result = new List<NotaPromedio>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_NotaPromedioPorEscuela", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new NotaPromedio
                            {
                                Escuela = reader["Escuela"].ToString(),
                                NotaPromedio1 = Convert.ToDouble(reader["NotaPromedio"])
                            });
                        }
                    }
                }
            }
            return result;
        }
    }

    public class Escuela
    {
        public string Nombre { get; set; }
        public int TotalEscuelas { get; set; }
    }

    public class PropuestaPorEstado
    {
        public string EstadoProyecto { get; set; }
        public int CantidadPropuestas { get; set; }
    }

    public class EscuelaPorEstado
    {
        public string Escuela { get; set; }
        public string EstadoProyecto { get; set; }
        public int TotalPropuestas { get; set; }
    }

    public class NotaPromedio
    {
        public string Escuela { get; set; }
        public double NotaPromedio1 { get; set; }
    }
}
