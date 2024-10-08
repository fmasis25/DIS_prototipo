using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProyectoVie.Pages.VieWeb
{
    public class AgregarPropuestaModel : PageModel
    {
        [BindProperty]
        public string NombrePropuesta { get; set; }

        [BindProperty]
        public string IdentificacionAcuerdo { get; set; }

        [BindProperty]
        public string Descripcion { get; set; }

        [BindProperty]
        public string TipoExtension { get; set; }

        [BindProperty]
        public string ODS { get; set; }

        [BindProperty]
        public string DeclaracionFinal { get; set; }

        [BindProperty]
        public DateTime? FechaAprobacion { get; set; }

        [BindProperty]
        public IFormFile PdfUpload { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Define la ruta para guardar los archivos
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "obj", "PDFs");

            // Crea el directorio si no existe
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string filePath = null;

            // Verifica si se subió un archivo
            if (PdfUpload != null && PdfUpload.Length > 0)
            {
                // Define la ruta completa para guardar el archivo
                filePath = Path.Combine(uploadsFolder, PdfUpload.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await PdfUpload.CopyToAsync(stream);
                }
            }

            // Conexión a la base de datos
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Insertar propuesta
                string insertPropuestaQuery = @"
                    INSERT INTO PROPUESTA 
                    (NombrePropuesta, IdentificacionAcuerdo, Descripcion, TipoExtension, ODS, DeclaracionFinal, FechaAprobacion, FechaSolicitud, ID_UsuarioPromotor, ID_Estado, RutaArchivo, NombreArchivo) 
                    VALUES 
                    (@NombrePropuesta, @IdentificacionAcuerdo, @Descripcion, @TipoExtension, @ODS, @DeclaracionFinal, @FechaAprobacion, @FechaSolicitud, @ID_UsuarioPromotor, @ID_Estado, @RutaArchivo, @NombreArchivo)";

                using (SqlCommand command = new SqlCommand(insertPropuestaQuery, connection))
                {
                    command.Parameters.AddWithValue("@NombrePropuesta", NombrePropuesta);
                    command.Parameters.AddWithValue("@IdentificacionAcuerdo", IdentificacionAcuerdo);
                    command.Parameters.AddWithValue("@Descripcion", Descripcion);
                    command.Parameters.AddWithValue("@TipoExtension", TipoExtension);
                    command.Parameters.AddWithValue("@ODS", ODS);
                    command.Parameters.AddWithValue("@DeclaracionFinal", DeclaracionFinal);
                    command.Parameters.AddWithValue("@FechaAprobacion", FechaAprobacion);
                    command.Parameters.AddWithValue("@FechaSolicitud", DateTime.Now); // Fecha de solicitud
                    command.Parameters.AddWithValue("@ID_UsuarioPromotor", DBNull.Value); // ID de usuario nulo
                    command.Parameters.AddWithValue("@ID_Estado", 2); // ID estado establecido en 2
                    command.Parameters.AddWithValue("@RutaArchivo", filePath);
                    command.Parameters.AddWithValue("@NombreArchivo", PdfUpload.FileName); // Nombre del archivo cargado

                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToPage("MenuExtensionista");
        }
    }
}
