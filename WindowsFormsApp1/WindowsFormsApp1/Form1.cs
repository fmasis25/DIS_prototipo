using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void EscuelasPorPropuesta_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Llamar al procedimiento almacenado para obtener los datos de escuelas
                    SqlCommand command = new SqlCommand("sp_ContarEscuelasPorPropuesta", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Limpiar series anteriores
                    EscuelasPorPropuesta.Series.Clear();

                    // Crear nueva serie de columnas
                    Series series = new Series("Suma Total Escuelas");
                    series.ChartType = SeriesChartType.Column;
                    EscuelasPorPropuesta.Series.Add(series);

                    // Cargar datos en la serie
                    foreach (DataRow row in dataTable.Rows)
                    {
                        series.Points.AddXY(row["Escuela"].ToString(), Convert.ToInt32(row["SumaTotalEscuelas"]));
                    }

                    // Configurar el área del gráfico
                    var chartArea = EscuelasPorPropuesta.ChartAreas[0];

                    // Personalizar el título de los ejes
                    chartArea.AxisX.Title = "Escuela";
                    chartArea.AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold); // Título en negrita y mayor tamaño
                    chartArea.AxisY.Title = "Suma Total Escuelas";
                    chartArea.AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold); // Título en negrita y mayor tamaño

                    // Rotar las etiquetas del eje X a vertical
                    chartArea.AxisX.LabelStyle.Angle = -90;

                    // Asegurar que todas las etiquetas se muestren
                    chartArea.AxisX.Interval = 1;

                    // Mostrar todos los números en el eje Y
                    chartArea.AxisY.Interval = 1;

                    // Asegurar que el eje Y comience en 0
                    chartArea.AxisY.IsStartedFromZero = true;

                    // Eliminar la cuadrícula de fondo
                    chartArea.AxisX.MajorGrid.Enabled = false;
                    chartArea.AxisY.MajorGrid.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void PropuestasPorEstado_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Llamar al procedimiento almacenado para obtener los datos de propuestas por estado
                    SqlCommand command = new SqlCommand("sp_ContarPropuestasPorEstado", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Limpiar las series anteriores
                    PropuestasPorEstadoChart.Series.Clear();

                    // Crear una nueva serie para el gráfico circular
                    Series series = new Series("PropuestasPorEstado");
                    series.ChartType = SeriesChartType.Pie; // Gráfico circular
                    PropuestasPorEstadoChart.Series.Add(series);

                    // Cargar los datos en el gráfico
                    foreach (DataRow row in dataTable.Rows)
                    {
                        // Agregar datos al gráfico circular (nombre del estado y cantidad de propuestas)
                        series.Points.AddXY(row["EstadoProyecto"].ToString(), Convert.ToInt32(row["CantidadPropuestas"]));
                    }

                    // Personalizar los colores del gráfico circular
                    Color[] colores = {
                    
                    Color.FromArgb(220, 20, 60),  // Rojo intenso
                    Color.FromArgb(128, 128, 128), // Gris medio
                    Color.FromArgb(0, 51, 102), // Azul oscuro
                    Color.FromArgb(192, 192, 192)// Gris claro
                    
                    
                    };

                    // Asignar colores personalizados a cada punto de datos
                    for (int i = 0; i < series.Points.Count; i++)
                    {
                        series.Points[i].Color = colores[i % colores.Length];
                    }

                    // Personalizar el gráfico
                    series["PieLabelStyle"] = "Outside"; // Mostrar las etiquetas fuera del gráfico
                    series.Label = "#PERCENT{P1}";       // Mostrar porcentaje en etiquetas
                    series.LegendText = "#VALX";          // Nombre del estado como leyenda

                    // Opcional: Personalización del gráfico
                    PropuestasPorEstadoChart.Titles.Clear();
                    PropuestasPorEstadoChart.Titles.Add("Cantidad de Propuestas por Estado de Proyecto");
                    PropuestasPorEstadoChart.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            EscuelasPorPropuesta_Click(this, EventArgs.Empty);
            PropuestasPorEstado_Click(this, EventArgs.Empty); // Llama a la carga inicial del gráfico
            EscuelasPorEstado_Click(this, EventArgs.Empty);
            NotaPromedioPorEscuela_Click(this, EventArgs.Empty);
        }


        private void PropuestasPorEstadoChart_Click(object sender, EventArgs e)
        {

        }

        private void EscuelasPorEstado_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Llamar al procedimiento almacenado
                    SqlCommand command = new SqlCommand("sp_ContarEscuelasPorEstado", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Limpiar las series anteriores y la leyenda
                    EscuelasPorEstadoChart.Series.Clear();
                    EscuelasPorEstadoChart.Legends.Clear(); // Limpia leyendas anteriores para evitar múltiples leyendas

                    // Crear una nueva leyenda personalizada
                    Legend legend = new Legend("Estados");
                    legend.Docking = Docking.Right;
                    EscuelasPorEstadoChart.Legends.Add(legend);

                    // Definir colores personalizados (azul oscuro, rojo intenso, gris claro, gris medio)
                    Color[] colores = { Color.FromArgb(0, 51, 102), // Azul oscuro
                        Color.FromArgb(192, 192, 192), // Gris claro
                        Color.FromArgb(220, 20, 60),// Rojo intenso
                        Color.FromArgb(128, 128, 128)}; // Gris medio

                    // Control para asignar colores a las series
                    int colorIndex = 0;

                    // Agrupar los datos por escuela y agregar series para cada estado de proyecto
                    foreach (DataRow row in dataTable.Rows)
                    {
                        string escuela = row["Escuela"].ToString();
                        string estadoProyecto = row["EstadoProyecto"].ToString();
                        int cantidadPropuestas = Convert.ToInt32(row["TotalPropuestas"]);

                        // Solo continuar si la cantidad de propuestas es mayor que 0
                        if (cantidadPropuestas > 0)
                        {
                            // Verificar si ya existe una serie para este estado
                            Series series = EscuelasPorEstadoChart.Series.FindByName(estadoProyecto);
                            if (series == null)
                            {
                                // Crear una nueva serie para este estado
                                series = new Series(estadoProyecto);
                                series.ChartType = SeriesChartType.StackedBar; // Gráfico de barras apiladas
                                series.IsValueShownAsLabel = true; // Mostrar los valores encima de las barras

                                // Asignar el color desde la lista
                                series.Color = colores[colorIndex % colores.Length];
                                colorIndex++; // Cambiar el color para la siguiente serie

                                EscuelasPorEstadoChart.Series.Add(series);
                            }

                            // Agregar el valor a la serie
                            series.Points.AddXY(escuela, cantidadPropuestas);
                        }
                    }

                    // Configurar el área del gráfico
                    var chartArea = EscuelasPorEstadoChart.ChartAreas[0];

                    // Personalizar los ejes
                    chartArea.AxisX.Title = "Escuela";
                    chartArea.AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold);
                    chartArea.AxisY.Title = "Cantidad de Propuestas";
                    chartArea.AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold);

                    // Mostrar todas las etiquetas del eje X
                    chartArea.AxisX.Interval = 1;
                    chartArea.AxisX.LabelStyle.Angle = -90; // Etiquetas en vertical

                    // Mostrar los números completos en el eje Y
                    chartArea.AxisY.Interval = 1;

                    // Personalización adicional
                    EscuelasPorEstadoChart.Titles.Clear();
                    EscuelasPorEstadoChart.Titles.Add("Propuestas por Escuela y Estado de Proyecto");
                    EscuelasPorEstadoChart.Titles[0].Font = new Font("Arial", 16, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void NotaPromedioPorEscuela_Click(object sender, EventArgs e)
        {
            string connectionString = "Server=tcp:serverprogra.database.windows.net,1433;Initial Catalog=VIE;Persist Security Info=False;User ID=Prograadmin;Password=proyectoVIE123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Llamar al procedimiento almacenado
                    SqlCommand command = new SqlCommand("sp_NotaPromedioPorEscuela", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Limpiar las series anteriores
                    NotaPromedioChart.Series.Clear();

                    // Crear una nueva serie para el gráfico de barras
                    Series series = new Series("Nota Promedio");
                    series.ChartType = SeriesChartType.Bar; // Gráfico de barras
                    NotaPromedioChart.Series.Add(series);

                    // Cargar datos en la serie
                    foreach (DataRow row in dataTable.Rows)
                    {
                        series.Points.AddXY(row["Escuela"].ToString(), Convert.ToDouble(row["NotaPromedio"]));
                    }

                    // Personalizar el gráfico
                    series.Color = Color.FromArgb(0, 51, 102); // Azul oscuro

                    // Configurar el área del gráfico
                    var chartArea = NotaPromedioChart.ChartAreas[0];

                    // Personalizar los ejes
                    chartArea.AxisX.Title = "Escuela";
                    chartArea.AxisX.TitleFont = new Font("Arial", 14, FontStyle.Bold);
                    chartArea.AxisY.Title = "Nota Promedio";
                    chartArea.AxisY.TitleFont = new Font("Arial", 14, FontStyle.Bold);

                    // Mostrar todas las etiquetas del eje X
                    chartArea.AxisX.Interval = 1;
                    chartArea.AxisX.LabelStyle.Angle = -90; // Etiquetas en vertical

                    // Personalización adicional (opcional)
                    NotaPromedioChart.Legends[0].Enabled = true; // Habilitar la leyenda
                    NotaPromedioChart.Titles.Clear();
                    NotaPromedioChart.Titles.Add("Nota Promedio de Propuestas por Escuela");
                    NotaPromedioChart.Titles[0].Font = new Font("Arial", 16, FontStyle.Bold);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}

