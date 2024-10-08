using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace EvaluarPropuesta
{
    public class EvaluarPropuestaController : Controller
    {
        // GET: Renderiza la vista principal con los datos de la tabla
        public IActionResult Index()
        {
            var model = new EvaluarPropuestaViewModel
            {
                TableData = new List<TableRowData>
                {
                    new TableRowData { Variable = "Disciplinas", Descripcion = "Descripción de variable", NC = false, CPar = "10%", CNota = "Actual", Estadisticas = "..." },
                    new TableRowData { Variable = "Problema", Descripcion = "...", NC = false, CPar = "20%", CNota = "...", Estadisticas = "..." },
                    new TableRowData { Variable = "Abordaje", Descripcion = "...", NC = false, CPar = "...", CNota = "...", Estadisticas = "..." },
                    new TableRowData { Variable = "Financiera", Descripcion = "...", NC = false, CPar = "20%", CNota = "...", Estadisticas = "..." },
                    new TableRowData { Variable = "Usuarios", Descripcion = "...", NC = false, CPar = "...", CNota = "...", Estadisticas = "..." },
                    new TableRowData { Variable = "Técnica", Descripcion = "...", NC = false, CPar = "...", CNota = "...", Estadisticas = "..." },
                    new TableRowData { Variable = "Participación", Descripcion = "...", NC = false, CPar = "10%", CNota = "...", Estadisticas = "..." },
                },
                NotaTotal = 60, // Nota inicial o calculada
                ValoracionAdicional = "" // Inicializar como vacío
            };

            return View(model);
        }

        // POST: Maneja el envío del formulario
        [HttpPost]
        public IActionResult EvaluarPropuesta(EvaluarPropuestaViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Aquí procesamos la evaluación
                // Podríamos actualizar la base de datos o realizar alguna acción necesaria
                // Por ejemplo, guardar los datos en la base de datos (no implementado en este código)

                // Redirigir a una página de éxito si la evaluación fue procesada correctamente
                return RedirectToAction("Success");
            }

            // Si hay algún error en los datos, volvemos a mostrar la vista con el modelo y los errores
            return View("Index", model);
        }

        // GET: Renderiza una vista de éxito luego de procesar la propuesta
        public IActionResult Success()
        {
            return View();
        }
    }

    // ViewModel que se envía a la vista
    public class EvaluarPropuestaViewModel
    {
        // Inicializar con una lista vacía
        public List<TableRowData> TableData { get; set; } = new List<TableRowData>();

        // Inicializar ValoracionAdicional con una cadena vacía
        public string ValoracionAdicional { get; set; } = string.Empty;

        public int NotaTotal { get; set; } // Nota total de la evaluación
    }

    // Clase que representa los datos de cada fila de la tabla
    public class TableRowData
    {
        public string Variable { get; set; } = string.Empty; // Inicializar con una cadena vacía
        public string Descripcion { get; set; } = string.Empty; // Inicializar con una cadena vacía
        public bool NC { get; set; } // Por defecto es false
        public string CPar { get; set; } = string.Empty; // Inicializar con una cadena vacía
        public string CNota { get; set; } = string.Empty; // Inicializar con una cadena vacía
        public string Estadisticas { get; set; } = string.Empty; // Inicializar con una cadena vacía
    }

}
